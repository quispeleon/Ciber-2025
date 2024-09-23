using Ciber.core;
using Ciber.Dapper;
using Xunit;
using MySqlConnector;

namespace Ciber.Test;
public class TestAdo
{
    protected readonly IDAO Ado;
    private const string   _cadena = "Server=localhost;Database=ciber;Uid=root;pwd=1234;Allow User Variables=True";
    public TestAdo() => Ado = new CuentaRepository(_cadena);
    public TestAdo(string cadena) => Ado = new CuentaRepository(cadena);
} 