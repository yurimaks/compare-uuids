using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace MyBenchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .AddDiagnoser(MemoryDiagnoser.Default)
                .AddJob(Job
                    .ShortRun
                    .WithToolchain(CsProjCoreToolchain.NetCoreApp80)
                    //.WithPlatform(Platform.X64)
                    .WithToolchain(InProcessNoEmitToolchain.Instance));

            var summary = BenchmarkRunner.Run(types:new[]
            {
                typeof(UuidCreationsViaCtor),
                typeof(UuidCreationsViaStatic),
                typeof(ToStringTests),
                typeof(ParseString)
            }, config);
        }
    }
}