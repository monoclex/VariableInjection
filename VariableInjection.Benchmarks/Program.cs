using BenchmarkDotNet.Attributes;

using System;

using VariableInjection.Demo;

namespace VariableInjection.Benchmarks
{
	internal class Program
	{
		private static void Main()
		{
			BenchmarkDotNet.Running.BenchmarkRunner.Run<Benchmarks>();

			Console.ReadLine();
		}
	}

	public class Benchmarks
	{
		private object _instance;
		private IInterfaceImplementor _implementor;
		private Injector _injector;

		[GlobalSetup]
		public void GlobalSetup()
		{
			_implementor = new IInterfaceImplementor();
			_injector = InjectorProvider<TestClass>.Injector;
			_instance = new TestClass();
		}

		[Benchmark]
		public Injector BuildInjector()
			=> InjectorProvider<TestClass>.GetInjector();

		[Benchmark]
		public void FreshPipeInjection()
			=> InjectorProvider<TestClass>.GetInjector()
				.Inject(new TestClass(), "string", 1234, new IInterfaceImplementor());

		[Benchmark]
		public void InjectValues()
			=> _injector
				.Inject(_instance, "str", 1234);

		[Benchmark]
		public void InjectInterface()
			=> _injector
				.Inject(_instance, _implementor);

		[Benchmark]
		public void InjectAll()
			=> _injector
				.Inject(_instance, 1234, _implementor, "str");
	}
}