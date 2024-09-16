using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;

public class TestMaquina : TestAdo

{
    public TestMaquina() : base() { }
    [Fact]
    public void TesstMaquina(){
        var maquina1 = new Maquina{
            Estado = true,
            Caracteristicas = "julio aaa"
        }; 
        Ado.AgregarMaquina(maquina1);
      
    }

}
