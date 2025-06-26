using Ciber.core;
using Xunit;
using System;
using System.Threading.Tasks;

namespace Ciber.Test
{
    public class TestCuenta : TestAdo
    {
        [Fact]
        public async Task TestAgregarCuentaAsync()
        {
            var cuenta = new Cuenta
            {
                Nombre = "doio 2sutup",
                Pass = "1192",
                Dni = 2391,
                HoraRegistrada = new TimeSpan(0, 0, 0)
            };

            await Ado.AgregarCuenta(cuenta);
            var cuentaObtenida = await Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

            Assert.NotNull(cuentaObtenida);
            Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
            Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
        }

        [Fact]
        public async Task TestObtenerCuentaPorIdAsync()
        {
            var cuenta = new Cuenta
            {
                Nombre = "Pedro Lopez",
                Pass = "789123",
                Dni = 1163,
                HoraRegistrada = new TimeSpan(0, 0, 0)
            };

            await Ado.AgregarCuenta(cuenta);
            var cuentaObtenida = await Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

            Assert.NotNull(cuentaObtenida);
            Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
            Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task TestActualizarCuentaAsync(int id)
        {
            var cuentaObtenida = await Ado.ObtenerCuentaPorId(id);
            Assert.NotNull(cuentaObtenida); // Asegura que existe antes de actualizar

            cuentaObtenida.Nombre = "Carlos Diaz Updated";
            cuentaObtenida.Pass = "passUpdated";

            await Ado.ActualizarCuenta(cuentaObtenida);
            var actualizada = await Ado.ObtenerCuentaPorId(id);

            Assert.Equal("Carlos Diaz Updated", actualizada.Nombre);
        }

        [Fact]
        public async Task TestEliminarCuentaAsync()
        {
            var cuenta = new Cuenta
            {
                Nombre = "Delete Test",
                Pass = "deletepass",
                Dni = 993,
                HoraRegistrada = new TimeSpan(0, 0, 0)
            };

            await Ado.AgregarCuenta(cuenta);
            await Ado.EliminarCuenta(cuenta.Ncuenta);

            var cuentaObtenida = await Ado.ObtenerCuentaPorId(cuenta.Ncuenta);
            Assert.Null(cuentaObtenida);
        }

        [Fact]
        public async Task TestObtenerTodasLasCuentasAsync()
        {
            var cuenta1 = new Cuenta
            {
                Nombre = "First User",
                Pass = "pass1",
                Dni = 156111,
                HoraRegistrada = new TimeSpan(0, 0, 0)
            };

            var cuenta2 = new Cuenta
            {
                Nombre = "Second User",
                Pass = "pass2",
                Dni = 272,
                HoraRegistrada = new TimeSpan(0, 0, 0)
            };

            await Ado.AgregarCuenta(cuenta1);
            await Ado.AgregarCuenta(cuenta2);

            var cuentas = await Ado.ObtenerTodasLasCuentas();
            Assert.NotEmpty(cuentas);
        }
    }
}
