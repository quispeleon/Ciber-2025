using Ciber.core;
using Ciber.Dapper;

namespace Ciber.Test;
public class TestAdo
{
    protected readonly Iciber Ado;
    private const string _cadena = "Server=localhost;Database=Ciber`````````````````````````````;Uid=gerenteSuper;pwd=passGerente;Allow User Variables=True";
    public TestAdo() => Ado = new AAdaper(_cadena);
    public TestAdo(string cadena) => Ado = new AAdaper(cadena);
}
