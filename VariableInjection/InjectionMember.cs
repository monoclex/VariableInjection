using SwissILKnife;

using System;
using System.Reflection;

namespace VariableInjection
{
	internal class InjectionMember
	{
		public InjectionMember(MemberInfo member)
		{
			MemberType = member.TypeOf();
			SetDelegate = MemberUtils.GetSetMethod(member);
		}

		public Type MemberType { get; }
		public Action<object, object> SetDelegate { get; }
	}
}