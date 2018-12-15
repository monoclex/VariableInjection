using System;
using System.Diagnostics;

namespace VariableInjection.Demo
{
	internal class Program
	{
		private static void Main()
		{
			for (int i = 0; i < 2; i++)
			{
				Primer();
			}

			new SetGetterOnly().Run();
			Console.ReadLine();
		}

		private static void Primer()
		{
			var stp = Stopwatch.StartNew();
			InjectorProvider<TestClass>.GetInjector().Inject(new TestClass(), "str", 1234, new IInterfaceImplementor());
			stp.Stop();
			Console.WriteLine($"{stp.ElapsedMilliseconds}");
		}
	}

	public class TestClass
	{
		[Inject] public string StringData;
		[Inject] public int IntData;
		[Inject] public IInterface Interface;
	}

	public interface IInterface
	{
	}

	public class IInterfaceImplementor : IInterface
	{
	}

	public static class InjectorProvider<T>
	{
		private static Injector _injector;
		public static Injector Injector { get => _injector ?? (_injector = GetInjector()); set => _injector = value; }

		public static Injector GetInjector()
			=> new InjectionBuilder<T>()
				.MapAttributed()
				.Build();
	}
}