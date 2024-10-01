using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;

public class TestMaquina : TestAdo

{
    
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
}
