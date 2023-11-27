using BenchmarkDotNet.Attributes;
using MedoUuid7 = Medo.Uuid7;
using FoodTechSpanUuid7 = FoodTech.Span.Uuid7;
using FoodTechSpanCtorSpanUuid7 = FoodTech.Span.SpanCtor.Uuid7;
using FoodTechBufferUuid7 = FoodTech.Span.BufferGenerate.Uuid7;

namespace MyBenchmarks;

[AllStatisticsColumn]
public class UuidCreationsViaCtor
{
    [Benchmark(Baseline = true)]
    public MedoUuid7 Medo_Ctor() => new MedoUuid7();

    [Benchmark]
    public FoodTechSpanUuid7 FoodTechSpan_Ctor() => new FoodTechSpanUuid7();

    [Benchmark]
    public FoodTechSpanCtorSpanUuid7 FoodTechSpan_CtorSpan() => new FoodTechSpanCtorSpanUuid7();

    [Benchmark]
    public FoodTechBufferUuid7 FoodTechBuffer_Ctor() => new FoodTechBufferUuid7();
}