using FluentAssertions;

using System.Linq;

using Xunit;

namespace VariableInjection.Tests
{
	public class CanInjectTests
	{
		public class InjectMe
		{
			public string Property { get; set; }
			public int Field;
		}

		[Fact]
		public void InjectsProperty()
		{
			var injector = Helpers.MakeBuilder<InjectMe>(typeof(InjectMe).GetProperties().First()).Build();

			const string change = "test";

			new InjectMe().Inject(injector, change)
				.Property.Should().Be(change);
		}

		[Fact]
		public void InjectsField()
		{
			var injector = Helpers.MakeBuilder<InjectMe>(typeof(InjectMe).GetFields().First()).Build();

			const int change = 1234;

			new InjectMe().Inject(injector, change)
				.Field.Should().Be(change);
		}
	}
}