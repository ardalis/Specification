﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.UnitTests.EvaluatorTests
{
  public class SearchExtension_Like
  {
    [Theory]
    [InlineData(true, "%", "")]
    [InlineData(true, "%", " ")]
    [InlineData(true, "%", "asdfa asdf asdf")]
    [InlineData(true, "%", "%")]
    [InlineData(false, "_", "")]
    [InlineData(true, "_", " ")]
    [InlineData(true, "_", "4")]
    [InlineData(true, "_", "C")]
    [InlineData(true, "_", "c")]
    [InlineData(false, "_", "CX")]
    [InlineData(false, "_", "cx")]
    [InlineData(true, "[A]", "a")]
    [InlineData(false, "[A]", "ab")]
    [InlineData(false, "[ABCD]", "")]
    [InlineData(true, "[ABCD]", "A")]
    [InlineData(true, "[ABCD]", "b")]
    [InlineData(false, "[ABCD]", "X")]
    [InlineData(false, "[ABCD]", "AB")]
    [InlineData(true, "[B-D]", "C")]
    [InlineData(true, "[B-D]", "D")]
    [InlineData(false, "[B-D]", "A")]
    [InlineData(false, "[^B-D]", "C")]
    [InlineData(false, "[^B-D]", "D")]
    [InlineData(true, "[^B-D)]", "A")]
    [InlineData(true, "%TEST[ABCD]XXX", "lolTESTBXXX")]
    [InlineData(false, "%TEST[ABCD]XXX", "lolTESTZXXX")]
    [InlineData(false, "%TEST[^ABCD]XXX", "lolTESTBXXX")]
    [InlineData(true, "%TEST[^ABCD]XXX", "lolTESTZXXX")]
    [InlineData(true, "%TEST[B-D]XXX", "lolTESTBXXX")]
    [InlineData(true, "%TEST[^B-D)]XXX", "lolTESTZXXX")]
    [InlineData(true, "%Stuff.txt", "Stuff.txt")]
    [InlineData(true, "%Stuff.txt", "MagicStuff.txt")]
    [InlineData(false, "%Stuff.txt", "MagicStuff.txt.img")]
    [InlineData(false, "%Stuff.txt", "Stuff.txt.img")]
    [InlineData(false, "%Stuff.txt", "MagicStuff001.txt.img")]
    [InlineData(true, "Stuff.txt%", "Stuff.txt")]
    [InlineData(false, "Stuff.txt%", "MagicStuff.txt")]
    [InlineData(false, "Stuff.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "Stuff.txt%", "Stuff.txt.img")]
    [InlineData(false, "Stuff.txt%", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff.txt%", "Stuff.txt")]
    [InlineData(true, "%Stuff.txt%", "MagicStuff.txt")]
    [InlineData(true, "%Stuff.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "%Stuff.txt%", "Stuff.txt.img")]
    [InlineData(false, "%Stuff.txt%", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff%.txt", "Stuff.txt")]
    [InlineData(true, "%Stuff%.txt", "MagicStuff.txt")]
    [InlineData(false, "%Stuff%.txt", "MagicStuff.txt.img")]
    [InlineData(false, "%Stuff%.txt", "Stuff.txt.img")]
    [InlineData(false, "%Stuff%.txt", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff%.txt", "MagicStuff001.txt")]
    [InlineData(true, "Stuff%.txt%", "Stuff.txt")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff.txt")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "Stuff%.txt%", "Stuff.txt.img")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff001.txt.img")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff001.txt")]
    [InlineData(true, "%Stuff%.txt%", "Stuff.txt")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff.txt")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "%Stuff%.txt%", "Stuff.txt.img")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff001.txt")]
    [InlineData(true, "_Stuff_.txt_", "1Stuff3.txt4")]
    [InlineData(false, "_Stuff_.txt_", "1Stuff.txt4")]
    [InlineData(false, "_Stuff_.txt_", "1Stuff3.txt")]
    [InlineData(false, "_Stuff_.txt_", "Stuff3.txt4")]
    public void ReturnsExpectedResult_GivenPatternAndInput(bool expectedResult, string pattern, string input)
    {
      var result = input.Like(pattern);

      result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("[", "asd")]
    [InlineData("[]", "asd")]
    public void ShouldThrowInvalidSearchPattern_GivenInvalidPattern(string pattern, string input)
    {
      Action action = () => input.Like(pattern);

      action.Should().Throw<InvalidSearchPatternException>().WithMessage($"Invalid search pattern: {pattern}");
    }
  }
}
