using Bunit;
using NUnit.Framework;

namespace VOZ.GUI.Test.Components.Pages;

[TestFixture]
internal sealed class StartTest : IDisposable
{
    private BunitContext _bunitContext = default!;

    [TearDown]
    public void TearDown() => Dispose();

    public void Dispose() => _bunitContext.Dispose();
}
