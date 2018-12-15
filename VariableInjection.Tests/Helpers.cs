using System.Reflection;

namespace VariableInjection.Tests
{
	internal static class Helpers
	{
		public static InjectionBuilder<T> MakeBuilder<T>(params MemberInfo[] members)
		{
			var builder = new InjectionBuilder<T>();

			builder.AddMembers(members);

			return builder;
		}

		public static T Inject<T>(this T item, Injector injector, params object[] values)
		{
			injector.Inject(item, values);
			return item;
		}
	}
}