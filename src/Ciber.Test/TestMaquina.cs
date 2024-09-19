using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;

public class TestMaquina : TestAdo

{
    private List<Maquina>  _maquinapruebas;
    public TestMaquina() : base() {

        _maquinapruebas = new List<Maquina>{
            new Maquina{
                Estado = true,
                Caracteristicas = "Windows xp"

            },
            new Maquina{
                Estado = false,
                Caracteristicas = "Windows 10"
            }
            
        };
        foreach (var maquina in _maquinapruebas){
            Ado.AgregarMaquina(maquina);
        }

     }
    [Fact]
    public void TesstMaquina(){
        var maquina1 = new Maquina{
            Estado = true,
            Caracteristicas = "julio aaa"
        }; 
        Ado.AgregarMaquina(maquina1);
      
    }

    public void TestObtenerMaquinaPorId()
    {
        var Maquinaid = Ado.ObtenerMaquinaPorId(_maquinapruebas[0].Nmaquina);

        Assert.NotNull(Maquinaid);
        Assert.Equal(_maquinapruebas[0].Nmaquina, Maquinaid.Nmaquina);
    }

    [Fact]
    public void  TestActulizarMaquina(){
        var Maquinaid = Ado.ObtenerMaquinaPorId(_maquinapruebas[0].Nmaquina);
        var maquina1 = _maquinapruebas[1];
        maquina1.Caracteristicas = "Windows 11 Actualizado";
        Ado.ActualizarMaquina(maquina1);

        var maquina = Ado.ObtenerMaquinaPorId(maquina1.Nmaquina);

        Assert.NotNull(maquina);
        Assert.Equal(maquina.Caracteristicas,"Windows 11 Actualizado");

    }
    [Theory]
    [InlineData(57)]
    public void TestEliminarMaquina(int idMaquina){

        Ado.EliminarMaquina(idMaquina);
        var maquin1 = Ado.ObtenerMaquinaPorId(idMaquina);
        Assert.Null(maquin1);
    }
}
