using BenchmarkDotNet.Attributes;
using MassTransit;
using Medo;
using Scheduletter.Extensions.Utils;
using UUIDNext;

namespace UUID_Demo;

public class UUID7ImplementationBenchmarks
{
    [Benchmark]
    public Guid NewGuid() => Guid.NewGuid();

    [Benchmark]
    public Guid CtorGuid() => new Guid();

    [Benchmark]
    public Uuid7 NewUuid_Medo() => Uuid7.NewUuid7();

    [Benchmark]
    public Uuid7 CtorUuid_Medo() => new Uuid7();

    [Benchmark]
    public Guid UUIDNext_Uuid() => Uuid.NewDatabaseFriendly(Database.PostgreSql);

    [Benchmark]
    public Guid UUIDNext_NewSequential() => Uuid.NewSequential();

    [Benchmark]
    public Guid UUIDExtension() => UuidExtensions.Uuid7.Guid();

    [Benchmark]
    public Guid DariosImplementation() => UuidV7.NewGuid();

    [Benchmark]
    public NewId NewId_Ctor() => new NewId();

    [Benchmark]
    public NewId NewStatic_NewId() => NewId.Next();

    [Benchmark]
    public FSH.NewId.NewId FshNewId() => FSH.NewId.NewId.Next();
}