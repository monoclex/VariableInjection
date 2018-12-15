using FluentAssertions;

using Xunit;

namespace VariableInjection.Tests
{
	public class InheritenceTests
	{
		public interface IOne
		{
		}

		public interface ITwo
		{
		}

		public interface IThree
		{
		}

		public class OneImplementor : IOne
		{
		}

		public class TwoImplementor : ITwo
		{
		}

		public class ThreeImplementor : IThree
		{
		}

		public class InjectMe
		{
			[Inject] public IOne One;
			[Inject] public ITwo Two;
			[Inject] public IThree Three;
		}

		[Fact]
		public void ShouldInject()
		{
			var injector = new InjectionBuilder<InjectMe>()
				.MapAttributed()
				.Build();

			var injectMe = new InjectMe();
			injector.Inject(injectMe, new OneImplementor(), new TwoImplementor(), new ThreeImplementor());

			injectMe.One.Should().NotBeNull();
			injectMe.Two.Should().NotBeNull();
			injectMe.Three.Should().NotBeNull();
		}
	}
}