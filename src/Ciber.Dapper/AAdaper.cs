public class AAdaper : IADO
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

    public Maquina ObtenerMaquina(int nMaquina)
    {
        string sql = "SELECT * FROM Maquina WHERE Nmaquina = @Nmaquina";
        return _conexion.QueryFirstOrDefault<Maquina>(sql, new { Nmaquina = nMaquina });
    }
}

// Uso en el código principal
var adaptador = new AAdaper("cadena_de_conexion");
var maquina = new Maquina { Nmaquina = 1, estado = true, Caracteristicas = "Caracteristicas de la maquina" };
adaptador.AltaMaquina(maquina, maquina.Caracteristicas);

var maquinaRecuperada = adaptador.ObtenerMaquina(1);
Console.WriteLine($"Maquina: {maquinaRecuperada.Nmaquina}, Estado: {maquinaRecuperada.estado}, Caracteristicas: {maquinaRecuperada.Caracteristicas}");