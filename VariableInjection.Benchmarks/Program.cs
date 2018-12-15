using BenchmarkDotNet.Attributes;
using System;
using VariableInjection.Demo;

namespace VariableInjection.Benchmarks
{
	/*
	class Program
	{
		static void Main()
		{
			// prime the pump
			new TestClass("", 0); //sets the injector under the hood

			BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();

			Console.ReadLine();
		}
	}

	public class Benchmarks
	{
		[Benchmark]
		public Injector BuildInjector()
			=> InjectorProvider<TestClass>.GetInjector();

		[Benchmark]
		public TestClass InjectValues()
			=> new TestClass("test", 1337);
	}
	*/
}
