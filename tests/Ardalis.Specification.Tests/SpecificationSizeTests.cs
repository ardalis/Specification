#if NET8_0_OR_GREATER

using ManagedObjectSize;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Tests;

public class SpecificationSizeTests
{
    private readonly ITestOutputHelper _output;

    public SpecificationSizeTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Spec_Empty()
    {
        var spec = new Specification<Customer>();

        var size = ObjectSize.GetObjectInclusiveSize(spec);

        size.Should().BeLessThan(105);
        PrintObjectSize(spec);
    }

    private record Customer(int Id);

    private void PrintObjectSize(object obj, [CallerArgumentExpression(nameof(obj))] string caller = "")
    {
        _output.WriteLine("");
        _output.WriteLine(caller);
        _output.WriteLine($"Inclusive: {ObjectSize.GetObjectInclusiveSize(obj):N0}");
        _output.WriteLine($"Exclusive: {ObjectSize.GetObjectExclusiveSize(obj):N0}");
    }
}

#endif
