using BenchmarkDotNet.Attributes;
using MedoUuid7 = Medo.Uuid7;
using FoodTechSpanUuid7 = FoodTech.Span.Uuid7;
using FoodTechSpanCtorSpanUuid7 = FoodTech.Span.SpanCtor.Uuid7;
using FoodTechBufferUuid7 = FoodTech.Span.BufferGenerate.Uuid7;
using FoodTechSpanUuidToStringRefact = FoodTech.Span.SpanStringRefact.Uuid7;

namespace MyBenchmarks;

[AllStatisticsColumn]
public class ToStringTests
{
    private MedoUuid7 _uuid71;
    private FoodTechSpanUuid7 _uuid72;
    private FoodTechBufferUuid7 _uuid73;
    private FoodTechSpanUuidToStringRefact _uuid74;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _uuid71 = new MedoUuid7();
        _uuid72 = new FoodTechSpanUuid7();
        _uuid73 = new FoodTechBufferUuid7();
        _uuid74 = new FoodTechSpanUuidToStringRefact();
    }

    [Benchmark(Baseline = true)]
    public string Medo_ToString() => _uuid71.ToString();

    [Benchmark]
    public string FoodTechSpan_ToString() => _uuid72.ToString();

    [Benchmark]
    public string FoodTechBuffer_ToString() => _uuid73.ToString();

    [Benchmark]
    public string FoodTechSpanRefact_ToString() => _uuid74.ToString();

}