using BenchmarkDotNet.Attributes;
using FoodTech.Uuid.Final;
using MedoUuid7 = Medo.Uuid7;

namespace MyBenchmarks;

[AllStatisticsColumn]
public class ParseString
{
    public string OriginalString;

    [GlobalSetup]
    public void GlobalSetup()
    {
        OriginalString = "12345678-9abc-def0-1234-56789abcdef0";
    }

    [Benchmark(Baseline = true)]
    public Guid GuidParse()
    {
        return Guid.Parse(OriginalString);
    }
    
    [Benchmark]
    public Uuid7 UuidParse_Ctor()
    {
        return Uuid7.Parse(OriginalString);
    }
    
    [Benchmark]
    public Uuid7 UuidParse()
    {
        return Uuid7.Parse((ReadOnlySpan<char>)OriginalString);
    }
    
    [Benchmark]
    public Uuid7 UuidParse_TryParse()
    {
        bool result = Uuid7.TryParse(OriginalString, null, out Uuid7 parsedValue);
        return parsedValue;
    }

    [Benchmark]
    public Uuid7 UuidParse_TryParse2()
    {
        bool result = Uuid7.TryParse(OriginalString, null, out Uuid7 parsedValue);
        return parsedValue;
    }
    
    // [Benchmark(Baseline = true)]
    // public MedoUuid7 Medo_FromString()
    // {
    //     return MedoUuid7.FromString(OriginalString);
    // }
    //
    // [Benchmark]
    // public MedoUuid7 Medo_Parse()
    // {
    //     return MedoUuid7.Parse(OriginalString);
    // }
    //
    // [Benchmark]
    // public MedoUuid7 Medo_TryParse()
    // {
    //     MedoUuid7.TryParse(OriginalString, null, out var result);
    //     return result;
    // }
}