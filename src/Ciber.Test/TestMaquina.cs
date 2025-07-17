using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;

public class TestMaquina : TestAdo

{
    
    
    private IDAO Ado;
    public TestMaquina() : base()
    {
        Ado = new ADOD(Conexion);
    }

    
    [Fact]
    public void TesstMaquina()
    {
        var maquina1 = new Maquina
        {
            Estado = true,
            Caracteristicas = "julio aaa"
        };
        Ado.AgregarMaquina(maquina1);

    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public void TestObtenerMaquinaPorId(int id)
    {
        var Maquinaid = Ado.ObtenerMaquinaPorId(id);

        Assert.NotNull(Maquinaid);
        Assert.Equal(id, Maquinaid.Nmaquina);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    public void TestActulizarMaquina(int id)
    {
        var  maquina1 = Ado.ObtenerMaquinaPorId(id);
        maquina1.Caracteristicas = "Windows 13 Actualizado";  // Update the correct value
        Ado.ActualizarMaquina(maquina1);

        var maquina = Ado.ObtenerMaquinaPorId(maquina1.Nmaquina);

        Assert.NotNull(maquina);
        Assert.Equal("Windows 13 Actualizado", maquina.Caracteristicas);  // Assert the expected value
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void TestEliminarMaquina(int idMaquina)
    {

        Ado.EliminarMaquina(idMaquina);
        var maquin1 = Ado.ObtenerMaquinaPorId(idMaquina);
        Assert.Null(maquin1);
    }

    // async 
    [Fact]
    public async Task TestAgregarYObtenerMaquinaAsync()
    {
        var maquina = new Maquina { Estado = true, Caracteristicas = "Intel i5, 8GB RAM" };

        await Ado.AgregarMaquinaAsync(maquina);
        var obtenida = await Ado.ObtenerMaquinaPorIdAsync(maquina.Nmaquina);

        Assert.NotNull(obtenida);
        Assert.Equal(maquina.Caracteristicas, obtenida.Caracteristicas);
    }

    [Fact]
    public async Task TestActualizarMaquinaAsync()
    {
        var maquina = new Maquina { Estado = true, Caracteristicas = "Vieja" };
        await Ado.AgregarMaquinaAsync(maquina);

        maquina.Caracteristicas = "Nueva";
        await Ado.ActualizarMaquinaAsync(maquina);

        var obtenida = await Ado.ObtenerMaquinaPorIdAsync(maquina.Nmaquina);
        Assert.Equal("Nueva", obtenida.Caracteristicas);
    }

    [Fact]
    public async Task TestEliminarMaquinaAsync()
    {
        var maquina = new Maquina { Estado = false, Caracteristicas = "Eliminar esta" };
        await Ado.AgregarMaquinaAsync(maquina);
        await Ado.EliminarMaquinaAsync(maquina.Nmaquina);

        var obtenida = await Ado.ObtenerMaquinaPorIdAsync(maquina.Nmaquina);
        Assert.Null(obtenida);
    }

}

