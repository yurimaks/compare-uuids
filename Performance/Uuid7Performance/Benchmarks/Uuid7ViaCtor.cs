using BenchmarkDotNet.Attributes;
using MedoUuid7 = Medo.Uuid7;
using Uuid7 = FoodTech.Uuid.Final.Uuid7;
//using FoodTechSpanCtorSpanUuid7 = FoodTech.Span.SpanCtor.Uuid7;
//using FoodTechBufferUuid7 = FoodTech.Span.BufferGenerate.Uuid7;

namespace MyBenchmarks;

[AllStatisticsColumn]
public class Uuid7ViaCtor
{
    private string OriginalString;

    [GlobalSetup]
    public void GlobalSetup()
    {
        OriginalString = "12345678-9abc-def0-1234-56789abcdef0";
    }
    
    
    [Benchmark(Baseline = true)]
    public Guid Guid_Ctor() => new Guid();
    
    [Benchmark]
    public Guid Guid_Ctor_Str() => new Guid(OriginalString);

    [Benchmark]
    public Uuid7 Uuid7_Ctor() => new Uuid7();

    [Benchmark]
    public Uuid7 Uuid7_Ctor_Str() => new Uuid7(OriginalString);
    
    //[Benchmark]
    //public MedoUuid7 Medo_Ctor() => new MedoUuid7();
    
    // [Benchmark]
    // public FoodTechSpanCtorSpanUuid7 FoodTechSpan_CtorSpan() => new FoodTechSpanCtorSpanUuid7();
    //
    // [Benchmark]
    // public FoodTechBufferUuid7 FoodTechBuffer_Ctor() => new FoodTechBufferUuid7();
}