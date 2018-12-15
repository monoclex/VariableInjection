using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace VariableInjection.Tests
{
	public class MultipleInjectors
	{
		public class InjectMe
		{
			[Inject(), Inject(1),            Inject(3), Inject(4)] public string String;
			[Inject(),            Inject(2), Inject(3)           ] public int Int;
			[Inject(), Inject(1),            Inject(3), Inject(4)] public char Char;
			[Inject(),            Inject(2),            Inject(4)] public float Float;
		}

		public Injector GetInjector(int injectId = 0)
			=> new InjectionBuilder<InjectMe>().MapAttributed(injectId).Build();

		public InjectMe GenerateInjected(int injectId = 0)
			=> GenerateInjected(GetInjector(injectId));

		public const string StringValue = "E";
		public const int IntValue = 420;
		public const char CharValue = 'E';
		public const float FloatValue = 1337F;

		public InjectMe GenerateInjected(Injector injector)
		{
			var injectMe = new InjectMe();
			injector.Inject(injectMe, StringValue, IntValue, CharValue, FloatValue);
			return injectMe;
		}

		[Fact]
		public void InjectsAll()
		{
			var injected = GenerateInjected(0);

			injected.String.Should().NotBe(default);
			injected.Int.Should().NotBe(default);
			injected.Char.Should().NotBe(default);
			injected.Float.Should().NotBe(default);
		}

		[Fact]
		public void Injects_1()
		{
			var injected = GenerateInjected(1);

			injected.String.Should().NotBe(default);
			injected.Int.Should().Be(default);
			injected.Char.Should().NotBe(default);
			injected.Float.Should().Be(default);
		}

		[Fact]
		public void Injects_2()
		{
			var injected = GenerateInjected(2);

			injected.String.Should().Be(default);
			injected.Int.Should().NotBe(default);
			injected.Char.Should().Be(default);
			injected.Float.Should().NotBe(default);
		}

		[Fact]
		public void Injects_3()
		{
			var injected = GenerateInjected(3);

			injected.String.Should().NotBe(default);
			injected.Int.Should().NotBe(default);
			injected.Char.Should().NotBe(default);
			injected.Float.Should().Be(default);
		}

		[Fact]
		public void Injects_4()
		{
			var injected = GenerateInjected(4);

			injected.String.Should().NotBe(default);
			injected.Int.Should().Be(default);
			injected.Char.Should().NotBe(default);
			injected.Float.Should().NotBe(default);
		}
	}
}
