using System;
using BaseKit.Common;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class MoneyTests
{
    [Fact]
    public void Constructor_Throws_WhenCurrencyEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => new Money(100, ""));
    }

    [Fact]
    public void Constructor_NormalizesCurrencyToUpperInvariant()
    {
        var money = new Money(100, "rial");
        Assert.Equal("RIAL", money.Currency);
    }

    [Fact]
    public void Addition_SumsAmounts_WhenSameCurrency()
    {
        var a = new Money(100, "IRR");
        var b = new Money(50, "IRR");

        Assert.Equal(new Money(150, "IRR"), a + b);
    }

    [Fact]
    public void Addition_Throws_WhenCurrenciesDiffer()
    {
        var a = new Money(100, "IRR");
        var b = new Money(50, "USD");

        Assert.Throws<InvalidOperationException>(() => a + b);
    }

    [Fact]
    public void Subtraction_SubtractsAmounts_WhenSameCurrency()
    {
        var a = new Money(100, "IRR");
        var b = new Money(30, "IRR");

        Assert.Equal(new Money(70, "IRR"), a - b);
    }

    [Fact]
    public void Multiplication_ScalesAmount()
    {
        var money = new Money(100, "IRR");
        Assert.Equal(new Money(250, "IRR"), money * 2.5m);
    }

    [Theory]
    [InlineData(100, "IRR", 100, "IRR", true)]
    [InlineData(100, "IRR", 50, "IRR", false)]
    [InlineData(100, "IRR", 100, "USD", false)]
    public void Equality_ComparesAmountAndCurrency(decimal amount1, string currency1, decimal amount2, string currency2, bool expected)
    {
        var a = new Money(amount1, currency1);
        var b = new Money(amount2, currency2);

        Assert.Equal(expected, a == b);
        Assert.Equal(!expected, a != b);
    }

    [Theory]
    [InlineData(50, 100, true)]
    [InlineData(100, 50, false)]
    public void LessThan_ComparesAmounts_WhenSameCurrency(decimal amount1, decimal amount2, bool expected)
    {
        var a = new Money(amount1, "IRR");
        var b = new Money(amount2, "IRR");

        Assert.Equal(expected, a < b);
    }

    [Fact]
    public void Comparison_Throws_WhenCurrenciesDiffer()
    {
        var a = new Money(100, "IRR");
        var b = new Money(100, "USD");

        Assert.Throws<InvalidOperationException>(() => a < b);
    }

    [Fact]
    public void ToString_FormatsAmountAndCurrency()
    {
        var money = new Money(1234567, "IRR");
        Assert.Equal("1,234,567 IRR", money.ToString());
    }

    [Fact]
    public void Zero_CreatesMoneyWithZeroAmount()
    {
        var money = Money.Zero("IRR");
        Assert.Equal(0, money.Amount);
        Assert.Equal("IRR", money.Currency);
    }

    [Fact]
    public void ToMoney_CreatesMoneyFromDecimal()
    {
        var money = 500m.ToMoney("IRR");
        Assert.Equal(new Money(500, "IRR"), money);
    }
}
