﻿using Mapsui.Styles;
using NUnit.Framework;

namespace Mapsui.Tests.Styles;

[TestFixture]
internal class VectorStyleTests
{
    [Test]
    public void DefaultConstructor_SetsExpectedDefaults()
    {
        var style = new VectorStyle();

        Assert.That(style.Line, Is.Not.Null);
        Assert.That(style.Outline, Is.Not.Null);
        Assert.That(style.Fill, Is.Not.Null);

        Assert.That(style.Line!.Color, Is.EqualTo(Color.Black));
        Assert.That(style.Line.Width, Is.EqualTo(1));

        Assert.That(style.Outline!.Color, Is.EqualTo(Color.Gray));
        Assert.That(style.Outline.Width, Is.EqualTo(1));

        Assert.That(style.Fill!.Color, Is.EqualTo(Color.White));
    }

    [Test]
    public void Equals_ReturnsFalse_ForDifferentLine()
    {
        var style1 = new VectorStyle();
        var style2 = new VectorStyle { Line = new Pen(Color.Red, 2) };

        Assert.That(style1.Equals(style2), Is.False);
        Assert.That(style1 == style2, Is.False);
        Assert.That(style1 != style2, Is.True);
    }

    [Test]
    public void Equals_ReturnsFalse_ForDifferentOutline()
    {
        var style1 = new VectorStyle();
        var style2 = new VectorStyle { Outline = new Pen(Color.Blue, 1) };

        Assert.That(style1.Equals(style2), Is.False);
    }

    [Test]
    public void Equals_ReturnsFalse_ForDifferentFill()
    {
        var style1 = new VectorStyle();
        var style2 = new VectorStyle { Fill = new Brush(Color.Red) };

        Assert.That(style1.Equals(style2), Is.False);
    }

    [Test]
    public void GetHashCode_IsDifferent_ForDifferentStyles()
    {
        var style1 = new VectorStyle();
        var style2 = new VectorStyle { Line = new Pen(Color.Red, 2) };

        Assert.That(style1.GetHashCode(), Is.Not.EqualTo(style2.GetHashCode()));
    }
}
