using System;

/*
 * Attribute to auto-discover fields/props to inject.
 * Via the usage of injection ids, you can have multiple of these.
 * You can create different unique injectors per class.
 */

namespace VariableInjection
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public sealed class InjectAttribute : Attribute
	{
		public const int DefaultInjectId = 0;

		public InjectAttribute(int injectId = DefaultInjectId) => InjectId = injectId;

		public int InjectId { get; }
	}
}