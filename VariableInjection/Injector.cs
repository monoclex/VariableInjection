using System;
using System.Collections.Concurrent;

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
			_typeIndexCache = new ConcurrentDictionary<Type, int>();
		}

		private Type[] _types;
		private readonly Action<object, object>[] _sets;
		private ConcurrentDictionary<Type, int> _typeIndexCache;

		public void Inject(object source, params object[] values)
		{
			// this is *really* inefficient but we're worrying about nanoseconds here, it's probably fine.
			// if you're really performance concerned, you'd just _a = a; _b = b; like a normal person and not use DI at all

			foreach (var i in values)
			{
				// try to directly get the type
				var type = i.GetType();

				if (_typeIndexCache.TryGetValue(type, out int index))
				{
					_sets[index](source, i);
					continue;
				}

				index = _types.IndexOf(type);

				if (index != -1)
				{
					_sets[index](source, i);
					_typeIndexCache.TryAdd(type, index);
				}
				else
				{
					// search down the types for an interface that could match
					for (index = 0; index < _types.Length; index++)
					{
						if (_types[index].IsAssignableFrom(type))
						{
							_sets[index](source, i);
							_typeIndexCache.TryAdd(type, index);
							break;
						}
					}
				}
			}
		}
	}
}