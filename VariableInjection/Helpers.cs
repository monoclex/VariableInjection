using System;
using System.Linq.Expressions;
using System.Reflection;

namespace VariableInjection
{
	internal static class Helpers
	{
		// https://stackoverflow.com/a/672212/3780113

		public static MemberInfo GetMemberInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> propertyLambda)
		{
			var type = typeof(TSource);

			if (!(propertyLambda.Body is MemberExpression memberExpression))
				throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a method, not a property/field.");

			var member = memberExpression.Member;

			if (type != member.ReflectedType &&
				!type.IsSubclassOf(member.ReflectedType))
				throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a member that is not from type {type}.");

			return member;
		}

		public static Type TypeOf(this MemberInfo member)
		{
			return member is PropertyInfo propInfo ?
				propInfo.PropertyType
				: ((FieldInfo)member).FieldType;
		}

		public static int IndexOf<T>(this T[] array, T lookup)
			where T : class
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == lookup)
				{
					return i;
				}
			}

			return -1;
		}
	}
}