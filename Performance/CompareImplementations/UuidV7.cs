namespace Scheduletter.Extensions.Utils;

using System.Buffers.Binary;
using System.Security.Cryptography;

public static class UuidV7
{
    /// <summary>
    /// Generate a UUID version 7 based on RFC draft at https://github.com/uuid6/uuid6-ietf-draft/
    /// </summary>
    private class Generator
    {
        private readonly int _sequenceMaxValue;

        private long _lastUsedTimestamp;
        private long _timestampOffset;
        private ushort _monotonicSequence;

        public Generator()
        {
            _sequenceMaxValue = (1 << SequenceBitSize) - 1;

            _lastUsedTimestamp = 0;
            _timestampOffset = 0;
            _monotonicSequence = 0;
        }

        public Guid New() => New(DateTime.UtcNow);

        private void SetSequence(Span<byte> bytes, ref long timestamp)
        {
            ushort sequence;
            long originalTimestamp = timestamp;

            lock (this)
            {
                sequence = GetSequenceNumber(ref timestamp);
                if (sequence > _sequenceMaxValue)
                {
                    // if the sequence is greater than the max value, we take advantage
                    // of the anti-rewind mechanism to simulate a slight change in clock time
                    timestamp = originalTimestamp + 1;
                    sequence = GetSequenceNumber(ref timestamp);
                }
            }

            BinaryPrimitives.TryWriteUInt16BigEndian(bytes, sequence);
        }

        private ushort GetSequenceNumber(ref long timestamp)
        {
            EnsureTimestampNeverMoveBackward(ref timestamp);

            if (timestamp == _lastUsedTimestamp)
            {
                _monotonicSequence += 1;
            }
            else
            {
                _lastUsedTimestamp = timestamp;
                _monotonicSequence = GetSequenceSeed();
            }

            return _monotonicSequence;
        }

        private void EnsureTimestampNeverMoveBackward(ref long timestamp)
        {
            long offsetTimestamp = timestamp + _timestampOffset;

            if (offsetTimestamp < _lastUsedTimestamp)
            {
                // if the computer clock has moved backward since the last generated UUID,
                // we add an offset to ensure the timestamp always move forward (See RFC Section 6.2)
                _timestampOffset = _lastUsedTimestamp - timestamp;
                timestamp = _lastUsedTimestamp;
            }
            else if (_timestampOffset > 0 && timestamp > _lastUsedTimestamp)
            {
                // reset the offset to reduce the drift with the actual time when possible
                _timestampOffset = 0;
            }
            else
            {
                timestamp = offsetTimestamp;
            }
        }

        private ushort GetSequenceSeed()
        {
            // following section 6.2 on "Fixed-Length Dedicated Counter Seeding", the initial value of the sequence is randomized
            Span<byte> buffer = stackalloc byte[2];
            RandomNumberGenerator.Fill(buffer);
            // Setting the highest bit to 0 mitigate the risk of a sequence overflow (see section 6.2)
            buffer[0] &= 0b0000_0111;
            return BinaryPrimitives.ReadUInt16BigEndian(buffer);
        }

        private byte Version => 7;

        private int SequenceBitSize => 12;

        private Guid New(DateTime date)
        {
            /* We implement the first example given in section 4.4.4.1 of the RFC
              0                   1                   2                   3
              0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
             +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
             |                           unix_ts_ms                          |
             +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
             |          unix_ts_ms           |  ver  |       rand_a          |
             +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
             |var|                        rand_b                             |
             +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
             |                            rand_b                             |
             +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
             */

            Span<byte> bytes = stackalloc byte[16];

            TimeSpan unixTimeStamp = date - DateTime.UnixEpoch;
            long timestampInMs = Convert.ToInt64(Math.Floor(unixTimeStamp.TotalMilliseconds));

            SetSequence(bytes[6..8], ref timestampInMs);
            SetTimestamp(bytes[0..6], timestampInMs);
            RandomNumberGenerator.Fill(bytes[8..16]);

            return CreateGuidFromBigEndianBytes(bytes);
        }

        private void SetTimestamp(Span<byte> bytes, long timestampInMs)
        {
            Span<byte> timestampInMillisecondsBytes = stackalloc byte[8];
            BinaryPrimitives.TryWriteInt64BigEndian(timestampInMillisecondsBytes, timestampInMs);
            timestampInMillisecondsBytes[2..8].CopyTo(bytes);
        }

        public static (long timestampMs, short sequence) Decode(Guid guid)
        {
            Span<byte> bytes = stackalloc byte[16];
            GuidHelper.TryWriteBigEndianBytes(guid, bytes);

            Span<byte> timestampBytes = stackalloc byte[8];
            bytes[0..6].CopyTo(timestampBytes[2..8]);
            long timestampMs = BinaryPrimitives.ReadInt64BigEndian(timestampBytes);

            var sequenceBytes = bytes[6..8];
            //remove version information
            sequenceBytes[0] &= 0b0000_1111;
            short sequence = BinaryPrimitives.ReadInt16BigEndian(sequenceBytes);

            return (timestampMs, sequence);
        }

        private Guid CreateGuidFromBigEndianBytes(Span<byte> bigEndianBytes)
        {
            SetVersion(bigEndianBytes);
            SetVariant(bigEndianBytes);
            return GuidHelper.FromBigEndianBytes(bigEndianBytes);
        }

        private void SetVersion(Span<byte> bigEndianBytes)
        {
            const int versionByte = 6;
            //Erase upper 4 bits
            bigEndianBytes[versionByte] &= 0b0000_1111;
            //Set 4 upper bits to version
            bigEndianBytes[versionByte] |= (byte)(Version << 4);
        }

        private void SetVariant(Span<byte> bigEndianBytes)
        {
            const int variantByte = 8;
            //Erase upper 2 bits
            bigEndianBytes[variantByte] &= 0b0011_1111;
            //Set 2 upper bits to variant
            bigEndianBytes[variantByte] |= 0b1000_0000;
        }
    }

    internal static class GuidHelper
    {
        public static Guid FromBigEndianBytes(Span<byte> bytes)
        {
            SwitchByteOrder(bytes);
            return new Guid(bytes);
        }

        public static bool TryWriteBigEndianBytes(Guid guid, Span<byte> bytes)
        {
            if (bytes.Length < 16 || !guid.TryWriteBytes(bytes))
            {
                return false;
            }

            SwitchByteOrder(bytes);
            return true;
        }

        private static void SwitchByteOrder(Span<byte> bigEndianBytes)
        {
            Permut(bigEndianBytes, 0, 3);
            Permut(bigEndianBytes, 1, 2);

            Permut(bigEndianBytes, 5, 4);

            Permut(bigEndianBytes, 6, 7);

            static void Permut(Span<byte> array, int indexSource, int indexDest)
            {
                (array[indexSource], array[indexDest]) = (array[indexDest], array[indexSource]);
            }
        }
    }

    private static readonly Generator _v7Generator = new();

    /// <summary>
    /// The Max UUID is special form of UUID that is specified to have all 128 bits set to 1.
    /// </summary>
    public static readonly Guid Max = new("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

    /// <summary>
    /// The Nil UUID is special form of UUID that is specified to have all 128 bits set to zero.
    /// </summary>
    public static readonly Guid Nil = new("00000000-0000-0000-0000-000000000000");

    /// <summary>
    /// Create a new UUID Version 7
    /// </summary>
    public static Guid NewGuid() => _v7Generator.New();
}
