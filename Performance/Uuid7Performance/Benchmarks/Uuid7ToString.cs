using BenchmarkDotNet.Attributes;
using FoodTech.Uuid.Final;
using MedoUuid7 = Medo.Uuid7;
using FoodTechSpanUuid7 = FoodTech.Span.Uuid7;
using FoodTechSpanCtorSpanUuid7 = FoodTech.Span.SpanCtor.Uuid7;
using FoodTechBufferUuid7 = FoodTech.Span.BufferGenerate.Uuid7;
using FoodTechSpanUuidToStringRefact = FoodTech.Span.SpanStringRefact.Uuid7;

namespace MyBenchmarks;

[AllStatisticsColumn]
public class Uuid7ToString
{
    // private MedoUuid7 _uuid71;
    // private FoodTechSpanUuid7 _uuid72;
    // private FoodTechBufferUuid7 _uuid73;
    // private FoodTechSpanUuidToStringRefact _uuid74;
    private Uuid7 _uuid7;
    private Guid _guid;

    [GlobalSetup]
    public void GlobalSetup()
    {
        // _uuid71 = new MedoUuid7();
        // _uuid72 = new FoodTechSpanUuid7();
        // _uuid73 = new FoodTechBufferUuid7();
        // _uuid74 = new FoodTechSpanUuidToStringRefact();
        _uuid7 = new Uuid7();
        _guid = new Guid();
    }

    [Benchmark(Baseline = true)]
    public string Guid_ToString() => _guid.ToString();
    
    [Benchmark]
    public string Uuid_ToString() => _uuid7.ToString();

    // [Benchmark(Baseline = true)]
    // public string Medo_ToString() => _uuid71.ToString();
    //
    // [Benchmark]
    // public string FoodTechSpan_ToString() => _uuid72.ToString();
    //
    // [Benchmark]
    // public string FoodTechBuffer_ToString() => _uuid73.ToString();
    //
    // [Benchmark]
    // public string FoodTechSpanRefact_ToString() => _uuid74.ToString();

}