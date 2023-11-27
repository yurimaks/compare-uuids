using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using UUID_Demo;

var config = DefaultConfig.Instance
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddJob(Job
        .MediumRun
        .WithToolchain(InProcessNoEmitToolchain.Instance));

//config.WithOptions(ConfigOptions.DisableOptimizationsValidator);

var summary = BenchmarkRunner.Run<UUID7ImplementationBenchmarks>(config);