using FluentAssertions;

using Xunit;

namespace VariableInjection.Tests
{
	public class DiscoversMembersTests
	{
		public class InjectMe
		{
			[Inject] public string PublicField;
			[Inject] private readonly int _privateField;
			[Inject] public float PublicProperty { get; set; }
			[Inject] public char _privateProperty { get; set; }

			public string GetPublicField() => PublicField;

			public int GetPrivateField() => _privateField;

			public float GetPublicProperty() => PublicProperty;

			public char GetPrivateProperty() => _privateProperty;
		}

		public Injector GetInjector()
			=> new InjectionBuilder<InjectMe>().MapAttributed().Build();

		[Fact]
		public void InjectsPublicField()
		{
			const string change = "test";

			var injected = new InjectMe().Inject(GetInjector(), change);
			injected.GetPublicField().Should().Be(change);
			injected.GetPrivateField().Should().Be(default);
			injected.GetPublicProperty().Should().Be(default);
			injected.GetPrivateProperty().Should().Be(default);
		}

		[Fact]
		public void InjectsPrivateField()
		{
			const int change = 1234;

			var injected = new InjectMe().Inject(GetInjector(), change);
			injected.GetPublicField().Should().Be(default);
			injected.GetPrivateField().Should().Be(change);
			injected.GetPublicProperty().Should().Be(default);
			injected.GetPrivateProperty().Should().Be(default);
		}

		[Fact]
		public void InjectsPublicProperty()
		{
			const float change = 1234;

			var injected = new InjectMe().Inject(GetInjector(), change);
			injected.GetPublicField().Should().Be(default);
			injected.GetPrivateField().Should().Be(default);
			injected.GetPublicProperty().Should().Be(change);
			injected.GetPrivateProperty().Should().Be(default);
		}

		[Fact]
		public void InjectsPrivateProperty()
		{
			const char change = 'E';

			var injected = new InjectMe().Inject(GetInjector(), change);
			injected.GetPublicField().Should().Be(default);
			injected.GetPrivateField().Should().Be(default);
			injected.GetPublicProperty().Should().Be(default);
			injected.GetPrivateProperty().Should().Be(change);
		}
	}
}