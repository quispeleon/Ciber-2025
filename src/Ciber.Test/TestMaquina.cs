using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;

public class TestMaquina

{
    public TestMaquina() : base() { }
    [Fact]
    public void TestMaquina(){
        var maquina = new Maquina{
            Estado = true,
            Caracteristicas = "Programador de lol en bacarrota"

        }
    }

}
