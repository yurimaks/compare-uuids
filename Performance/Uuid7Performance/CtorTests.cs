// using BenchmarkDotNet.Attributes;
// using Medo;
// using Uuid7 = Medo.Uuid7;
//
// namespace MyBenchmarks;
//
// public class CtorTests
// {
//     [Benchmark]
//     public Guid GuidStatic() => Guid.NewGuid();
//
//     [Benchmark]
//     public Guid GuidCtor() => new Guid();
//
//     [Benchmark(Baseline = true)]
//     public Uuid7_MemorySpan Uuid7Static() => Uuid7_MemorySpan.NewUuid7();
//
//     [Benchmark]
//     public Uuid7_MemorySpan Uuid7Ctor() => new Uuid7_MemorySpan();
//
//     [Benchmark]
//     public Uuid7 Uuid7ExternalStatic() => Uuid7.NewUuid7();
//
//     [Benchmark]
//     public Uuid7 Uuid7ExternalCtor() => new Uuid7();
// }