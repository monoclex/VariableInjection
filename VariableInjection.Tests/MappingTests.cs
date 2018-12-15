using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace VariableInjection.Tests
{
	public class MappingTests
	{
		public class InjectMe
		{
			public string PublicProperty { get; set; }
			public int PublicMethod() => 1;
		}

		public class OtherClass
		{
			public string Test { get; set; }
		}

		[Fact]
		public void MapPublicProperty()
		{
			var builder = new InjectionBuilder<InjectMe>()
				.Map(e => e.PublicProperty)
				.Build();

			var item = new InjectMe();

			builder.Inject(item, "test");

			item.PublicProperty.Should().Be("test");
		}

		[Fact]
		public void MapMethod()
		{
			((Action)(() =>
			{
				var builder = new InjectionBuilder<InjectMe>()
					.Map<InjectMe, Func<int>>(e => e.PublicMethod)
					.Build();
			})).Should().ThrowExactly<ArgumentException>();
		}

		[Fact]
		public void MapOutOfClass()
		{
			((Action)(() =>
			{
				var builder = new InjectionBuilder<InjectMe>()
					.Map(e => new OtherClass().Test)
					.Build();
			})).Should().ThrowExactly<ArgumentException>();
		}
	}
}
