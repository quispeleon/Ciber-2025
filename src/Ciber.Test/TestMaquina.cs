using Ciber.core;
using Xunit;
using System.Threading.Tasks;

namespace Ciber.Test
{
    public class TestMaquina : TestAdo
    {
        [Fact]
        public async Task TestAgregarMaquinaAsync()
        {
            var maquina1 = new Maquina
            {
                Estado = true,
                Caracteristicas = "julio aaa"
            };

            await Ado.AgregarMaquina(maquina1);

            var maquinaObtenida = await Ado.ObtenerMaquinaPorId(maquina1.Nmaquina);
            Assert.NotNull(maquinaObtenida);
            Assert.Equal(maquina1.Caracteristicas, maquinaObtenida.Caracteristicas);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        public async Task TestObtenerMaquinaPorIdAsync(int id)
        {
            var maquina = await Ado.ObtenerMaquinaPorId(id);

            Assert.NotNull(maquina);
            Assert.Equal(id, maquina.Nmaquina);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        public async Task TestActualizarMaquinaAsync(int id)
        {
            var maquina1 = await Ado.ObtenerMaquinaPorId(id);
            Assert.NotNull(maquina1); // Asegura que existe

            maquina1.Caracteristicas = "Windows 13 Actualizado";
            await Ado.ActualizarMaquina(maquina1);

            var maquinaActualizada = await Ado.ObtenerMaquinaPorId(id);

            Assert.NotNull(maquinaActualizada);
            Assert.Equal("Windows 13 Actualizado", maquinaActualizada.Caracteristicas);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task TestEliminarMaquinaAsync(int idMaquina)
        {
            await Ado.EliminarMaquina(idMaquina);
            var maquina = await Ado.ObtenerMaquinaPorId(idMaquina);
            Assert.Null(maquina);
        }
    }
}