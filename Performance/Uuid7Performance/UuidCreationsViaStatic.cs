using BenchmarkDotNet.Attributes;
using MedoUuid7 = Medo.Uuid7;
using FoodTechSpanUuid7 = FoodTech.Span.Uuid7;
using FoodTechBufferUuid7 = FoodTech.Span.BufferGenerate.Uuid7;

namespace MyBenchmarks;

[AllStatisticsColumn]
public class UuidCreationsViaStatic
{
    [Benchmark(Baseline = true)]
    public MedoUuid7 Medo_Ctor_Static() => MedoUuid7.NewUuid7();

    [Benchmark]
    public FoodTechSpanUuid7 FoodTechSpan_Ctor_Static() => FoodTechSpanUuid7.NewUuid7();

    [Benchmark]
    public FoodTechBufferUuid7 FoodTechBuffer_Ctor_Static() => FoodTechBufferUuid7.NewUuid7();

}