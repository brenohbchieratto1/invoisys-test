using System.Diagnostics.CodeAnalysis;
using AutoFixture;

namespace App.InvoisysTest.UnitTest;

[SuppressMessage("Design", "CA1051:Não declarar campos de instância visíveis")]
public abstract class GlobalUsings
{
    protected readonly Fixture Fixture = new();
    protected readonly CancellationToken CancellationToken = CancellationToken.None;

    protected GlobalUsings()
    {
    }
}