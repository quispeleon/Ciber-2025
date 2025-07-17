
using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;
public class TestAlquiler : TestAdo
{   
    private IDAO Ado;
    public TestAlquiler() : base()
    {
        Ado = new ADOD(Conexion);
    }


    [Theory]
    [InlineData(3, 9, 2, 3)]
    [InlineData(4, 10, 2, 5)]
    [InlineData(5, 11, 2, 1)]
    [InlineData(6, 12, 2, 3)]
    [InlineData(7, 13, 2, 1)]
    public void TesstAlquilerAgregar(int idCuenta, int idMaquina, int tipo, int tiempo)
    {
        var alquiler1 = new Alquiler
        {
            Ncuenta = idCuenta,
            Nmaquina = idMaquina,
            Tipo = tipo,
            CantidadTiempo = TimeSpan.FromHours(tiempo),
            Pagado = true
        };
        
        Ado.AgregarAlquiler(alquiler1,true);
        var obteberAlquiler = Ado.ObtenerAlquilerPorId(alquiler1.IdAlquiler);

        Assert.NotNull(obteberAlquiler);

    }
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void TraerAlquiler(int idAlquiler){
        var alquiler = Ado.ObtenerAlquilerPorId(idAlquiler);
        Assert.NotNull(alquiler);
    }
    // [Theory]
    // [InlineData(2)]
    // [InlineData(3)]
    // public void EliminarAlquilerr(int idAlquiler){
    //     var alquiler = Ado.ObtenerAlquilerPorId(idAlquiler);
    //     Ado.EliminarAlquiler(idAlquiler);
    //     Assert.Null(alquiler);
        
    // }


    // async 



    [Theory]
    
    [InlineData(8, 14, 2, 3)]
    [InlineData(9, 15, 2, 3)]
    [InlineData(10, 16, 2, 2)]
    public async Task TestAlquilerAgregarAsync(int idCuenta, int idMaquina, int tipo, int tiempo)
    {
        var alquiler = new Alquiler
        {
            Ncuenta = idCuenta,
            Nmaquina = idMaquina,
            Tipo = tipo,
            CantidadTiempo = TimeSpan.FromHours(tiempo),
            Pagado = true
        };

        await Ado.AgregarAlquilerAsync(alquiler, true);
        var obtenerAlquiler = await Ado.ObtenerAlquilerPorIdAsync(alquiler.IdAlquiler);

        Assert.NotNull(obtenerAlquiler);
        Assert.Equal(idCuenta, obtenerAlquiler.Ncuenta);
        Assert.Equal(idMaquina, obtenerAlquiler.Nmaquina);
        Assert.Equal(tipo, obtenerAlquiler.Tipo);
        Assert.Equal(TimeSpan.FromHours(tiempo), obtenerAlquiler.CantidadTiempo);
    }
    


}

