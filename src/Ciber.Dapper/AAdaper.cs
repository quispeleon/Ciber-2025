
using Dapper;
using MySqlConnector;
using Ciber.core;


namespace Ciber.Dapper;

public class AAdaper : IADO
{
    private readonly IDbConnection _conexion;

    public AAdaper(IDbConnection conexion) => this._conexion = conexion;

    public AAdaper(string cadena) => _conexion = new MySqlConnection(cadena);


    private static readonly string _queryMaquina =
    @"INSERT INTO Maquina VALUES (@Nmaquina,@estado,@caracteristicas)";

    public void AltaMaquina(Maquina maquina ,string caracteristicas) => _conexion.Execute(
        _queryMaquina, new {
            Nmaquina = maquina.Nmaquina,
            estado = maquina.estado,
            caracteristicas = caracteristicas
        }

    ); 
}
