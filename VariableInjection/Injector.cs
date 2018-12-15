using System;

/*
 * This class takes the given input, finds a correlating type and then sets the field/property correlating to the type.
 * This is created via InjectionBuilder
 */

namespace VariableInjection
{
	public class Injector
	{
		internal Injector(Type[] types, Action<object, object>[] sets)
		{
			_types = types;
			_sets = sets;
		}

		private Type[] _types;
		private readonly Action<object, object>[] _sets;

		public void Inject(object source, params object[] values)
		{
			// this is *really* inefficient but we're worrying about nanoseconds here, it's probably fine.
			// if you're really performance concerned, you'd just _a = a; _b = b; like a normal person and not use DI at all

			foreach (var i in values)
			{
				var type = i.GetType();

				var index = _types.IndexOf(type);

				if (index != -1)
				{
					_sets[index](source, i);
				}
			}
		}
	}
}