# VariableInjection
A lightweight, easy, fast property injection framework. 

Catch it on [nuget](https://www.nuget.org/packages/VariableInjection/)

## How does it work?
We got examples o'er [here](./VariableInjection.Demo/Examples.cs).
Here's an example pulled straight from there:

```cs
	public class SimpleExample
	{
		public class InjectMe
		{
			public InjectMe(Injector injector, string stringData, int key)
			{
				// this will tell the injector to inject the values into this class
				injector.Inject(this, stringData, key);
			}

			[Inject] private int _secretKey;
			[Inject] public string StringData { get; set; }

			public int GetSecretKey()
				=> _secretKey;
		}

		public void Run()
		{
			// make an injector that will automatically inject all `[Inject]`s
			var injector = new InjectionBuilder<InjectMe>()
				.MapAttributed() // there's more than just MapAttributed FWIW
				.Build();

			var injectme = new InjectMe(injector, "stringData", 4);

			Console.WriteLine(injectme.StringData); // will write "stringData"
			Console.WriteLine(injectme.GetSecretKey()); // will write 4
		}
	}
```

There's more cool stuff you can do, please check out that file.