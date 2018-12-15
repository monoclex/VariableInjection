using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/*
 * This class adds extensions that make it easier to add properties/fields to an InjectionBuider.
 */

namespace VariableInjection
{
	public static class InjectionBuilderExtensions
	{
		public static Type injectAttributeType = typeof(InjectAttribute);

		public static InjectionBuilder<T> Map<T, TReturn>(this InjectionBuilder<T> injectionBuilder, Expression<Func<T, TReturn>> keyExpression)
		{
			var member = keyExpression.GetMemberInfo();

			injectionBuilder.AddMember(member);

			return injectionBuilder;
		}

		public static InjectionBuilder<T> MapAll<T>(this InjectionBuilder<T> injectionBuilder, Func<IEnumerable<MemberInfo>, IEnumerable<MemberInfo>> exclude = null)
		{
			const BindingFlags flags =
				BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance; // why would you want to DI a static class

			var members =
				typeof(T)
				.GetProperties(flags)
				.Cast<MemberInfo>()
				.Concat(typeof(T)
					.GetFields(flags));

			if (exclude != null)
			{
				members = exclude(members);
			}

			foreach (var i in members)
			{
				injectionBuilder.AddMember(i);
			}

			return injectionBuilder;
		}

		public static InjectionBuilder<T> MapAttributed<T>(this InjectionBuilder<T> injectionBuilder, int injectId = InjectAttribute.DefaultInjectId)
			=> injectionBuilder.MapAll(members => members.Where(member =>
			{
				return member.GetCustomAttributes(true)
						.Any(x => x.GetType() == injectAttributeType &&
							((InjectAttribute)x).InjectId == injectId);
			}));
	}
}