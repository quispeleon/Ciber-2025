using Ciber.core;
using Xunit;
using System;
using System.Threading.Tasks;

namespace Ciber.Test
{
    public class TestAlquiler : TestAdo
    {
        [Theory]
        [InlineData(3, 9, 2, 3)]
        [InlineData(4, 10, 2, 5)]
        [InlineData(5, 11, 2, 1)]
        [InlineData(6, 12, 2, 3)]
        [InlineData(7, 13, 2, 1)]
        [InlineData(8, 14, 2, 3)]
        [InlineData(9, 15, 2, 3)]
        [InlineData(10, 16, 2, 2)]
        public async Task TestAlquilerAgregarAsync(int idCuenta, int idMaquina, int tipo, int tiempo)
        {
            var alquiler1 = new Alquiler
            {
                Ncuenta = idCuenta,
                Nmaquina = idMaquina,
                Tipo = tipo,
                CantidadTiempo = TimeSpan.FromHours(tiempo),
                Pagado = true
            };

            await Ado.AgregarAlquiler(alquiler1, true);

            var obtenerAlquiler = await Ado.ObtenerAlquilerPorId(alquiler1.IdAlquiler);

            Assert.NotNull(obtenerAlquiler);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async Task TraerAlquilerAsync(int idAlquiler)
        {
            var alquiler = await Ado.ObtenerAlquilerPorId(idAlquiler);
            Assert.NotNull(alquiler);
        }

        // Para usar este test asegurate de que el alquiler con ese ID exista antes
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async Task EliminarAlquilerAsync(int idAlquiler)
        {
            var alquiler = await Ado.ObtenerAlquilerPorId(idAlquiler);
            Assert.NotNull(alquiler); // Verifica que existe antes

            await Ado.EliminarAlquiler(idAlquiler);

            var alquilerEliminado = await Ado.ObtenerAlquilerPorId(idAlquiler);
            Assert.Null(alquilerEliminado);
        }
    }
}