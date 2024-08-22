using Dapper;
using MySqlConnector;
using Ciber.core;
using System.Data;
namespace Ciber.Dapper;
public class AAdaper : Iciber
{
    private readonly IDbConnection _conexion;

    public AAdaper(IDbConnection conexion) => this._conexion = conexion;

    public AAdaper(string cadena) => _conexion = new MySqlConnection(cadena);

    private static readonly string _queryMaquina =
    @"INSERT INTO Maquina (Nmaquina, estado, caracteristicas) VALUES (@Nmaquina, @estado, @caracteristicas)";

    public void AltaMaquina(Maquina maquina, string caracteristicas)
    {
        _conexion.Execute(_queryMaquina, new
        {
            Nmaquina = maquina.Nmaquina,
            estado = maquina.estado,
            caracteristicas = caracteristicas
        });
    }

    public List<Maquina> ObtenerMaquinas() => _conexion.Query<Maquina>(_queryMaquina).ToList();
}

