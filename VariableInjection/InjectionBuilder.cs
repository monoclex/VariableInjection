using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/*
 * This class builds an `Injector`. It provides methods to add members,
 * and then turns those members into an array of types and delegates (to set fields/props)
 */

namespace VariableInjection
{
	public class InjectionBuilder<T>
	{
		public InjectionBuilder() => _injMembers = new List<InjectionMember>();

		private readonly List<InjectionMember> _injMembers;
		private readonly object members;

		public void AddMember(MemberInfo member)
			=> _injMembers.Add(new InjectionMember(member));

		public void AddMembers(IEnumerable<MemberInfo> members)
			=> _injMembers.AddRange(members.Select(x => new InjectionMember(x)));

		public Injector Build()
		{
			var members = _injMembers.ToArray();

			var types = new Type[members.Length];
			var sets = new Action<object, object>[members.Length];

			for (int i = 0; i < members.Length; i++)
			{
				var member = members[i];

				types[i] = member.MemberType;
				sets[i] = member.SetDelegate;
			}

			return new Injector(types, sets);
		}
	}
}