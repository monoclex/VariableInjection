using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VariableInjection.Demo
{
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

	public class MultipleInjectorsExample
	{
		public class InjectMe
		{
			public InjectMe(Injector injector, string stringData, int key)
			{
				injector.Inject(this, stringData, key);
			}

			// 'StringA' and 'Int' belong to injector 1
			// 'StringB' and 'Int' belong to injector 2

			[Inject(1)] public string StringA { get; set; }
			[Inject(1), Inject(2)] public int Int { get; set; }
			[Inject(2)] public string StringB { get; set; }

			public override string ToString()
				=> $"{nameof(StringA)}: {StringA ?? "null"}\r\n{nameof(StringB)}: {StringB ?? "null"}\r\n{nameof(Int)}: {Int}";
		}

		public void Run()
		{
			var injector_1 = new InjectionBuilder<InjectMe>()
				.MapAttributed(1) // this number correlates to the number passed in to Inject
				.Build();

			var injector_2 = new InjectionBuilder<InjectMe>()
				.MapAttributed(2)
				.Build();

			var injectme_1 = new InjectMe(injector_1, "stringData", 4);
			var injectme_2 = new InjectMe(injector_2, "stringData", 4);

			Console.WriteLine("Injectme1: ");
			Console.WriteLine(injectme_1); // stringa should be "stringData", stringb should be null, key should be 4

			Console.WriteLine("");
			Console.WriteLine("Injectme2: ");
			Console.WriteLine(injectme_2); // stringa should be null, stringb should be "stringData", key should be 4
		}
	}

	public class PrivateInjectors
	{
		public class InjectMe
		{
			public InjectMe(Injector injector, string stringData, int key)
			{
				injector.Inject(this, stringData, key);
			}

			// both of these private - unaccessible to the public
			[Inject] private string _secret;
			[Inject] private int _secretKey;

			public override string ToString()
				=> $"Secret String: {_secret}\r\nSecret Key: {_secretKey}";
		}

		public void Run()
		{
			var injector = new InjectionBuilder<InjectMe>()
				.MapAttributed()
				.Build();

			var injectme = new InjectMe(injector, "stringData", 4); // we pass in this data and it will be injected as private

			Console.WriteLine(injectme); // write out the data

			// we can re-use the same injector and inject different data into those private variables **any time**

			injector.Inject(injectme, "changed", 420);

			Console.Write(injectme); // will be different now
		}
	}

	// so this is the cool one
	public class FluentInjectors
	{
		// let's pretend we have *no* control over this class
		// this is some foregin class we ain't allowed to touch
		// and we wanna inject stuff
		public class InjectMe
		{
			public string SomeString { get; private set; }
			public int SomeInt { get; private set; }

			public override string ToString()
				=> $"{nameof(SomeString)}: {SomeString ?? "null"}\r\n{nameof(SomeInt)}: {SomeInt}";
		}

		public void Run()
		{
			var injector = new InjectionBuilder<InjectMe>()
				.Map(e => e.SomeString) // we're "mapping" these properties to the injector
				.Map(e => e.SomeInt) // we've told it that these two things we want to inject
				.Build();

			var injectme = new InjectMe();

			Console.WriteLine(injectme); // null and 0 for somestring and someint

			// this won't compile: injectme.SomeInt = 3;

			injector.Inject(injectme, "1234", 420); // injector to the rescue

			Console.WriteLine(injectme); // wham changed!

			//
			// side note:
			//
			// there is a possibility to set { get; }ter only properties, but that would invoke guess work
			// since properties are, under the hood, just a get_PropertyName and set_PropertyName.
			// i would have to check if that method exists, and the underlying backer. check out SwissILKnife
			// - it's where the getting/setting code for props/fields lies.
			//
		}
	}

	// here's the messy one (explained by the note above, planned to make this easier in SwissILKnife)
	public class SetGetterOnly
	{
		// again, foregin
		public class InjectMe
		{
			public string Foreign { get; }

			public override string ToString()
				=> $"{nameof(Foreign)}: {Foreign ?? "null"}";
		}

		// alright, run:
		public void Run()
		{
			// this is messy and not guarenteed to work
			// if the underlying field isn't named like we want, it'll be difficult to pull this.
			// so if the property isn't an auto (only { get; } with no custom private backing field)
			// we won't be able to get it since we rely on it bing an auto.

			var name = typeof(InjectMe) // the type
				.GetProperties() // get properties
				.First() // the first one
				.Name; // get the name

			var field = typeof(InjectMe)
				.GetField($"<{name}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance); // got the field

			// so now we have the field behind the property, let's add it here

			var injectorBuilder = new InjectionBuilder<InjectMe>();
			injectorBuilder.AddMember(field);

			var injector = injectorBuilder.Build();

			// now let's test this

			var injectme = new InjectMe();

			Console.WriteLine(injectme); // should print null

			injector.Inject(injectme, "NEW VALUE");

			Console.Write(injectme); // success! NEW VALUE is in place of the getter-only property!
		}
	}
}
