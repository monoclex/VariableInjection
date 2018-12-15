using System;
using System.Diagnostics;

namespace VariableInjection.Demo
{
	class Program
	{
		static void Main()
		{
			new SetGetterOnly().Run();
			Console.ReadLine();
		}
	}
}