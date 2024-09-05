using Ciber.core;
using Ciber.Dapper;
using Xunit;

namespace Ciber.Test;
public class TestCuenta : TestAdo
{
    public TestCuenta() : base() { }

    [Fact]
    public void TestAgregarCuenta()
    {
        // Arrange
        var cuenta = new Cuenta
        {
            Nombre = "Juan Perez",
            Pass = "123456",
            Dni = 987654321,
            HoraRegistrada = DateTime.Now
        };

        // Act
        Ado.AgregarCuenta(cuenta);
        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        // Assert
        Assert.NotNull(cuentaObtenida);
        Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
        Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
    }

    [Fact]
    public void TestObtenerCuentaPorId()
    {
        // Arrange
        var cuenta = new Cuenta
        {
            Nombre = "Pedro Lopez",
            Pass = "789123",
            Dni = 1122334455,
            HoraRegistrada = DateTime.Now
        };

        Ado.AgregarCuenta(cuenta);

        // Act
        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        // Assert
        Assert.NotNull(cuentaObtenida);
        Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
        Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
    }

    [Fact]
    public void TestActualizarCuenta()
    {
        // Arrange
        var cuenta = new Cuenta
        {
            Nombre = "Carlos Diaz",
            Pass = "passOriginal",
            Dni = 6655443322,
            HoraRegistrada = DateTime.Now
        };
        Ado.AgregarCuenta(cuenta);

        // Act
        cuenta.Nombre = "Carlos Diaz Updated";
        cuenta.Pass = "passUpdated";
        Ado.ActualizarCuenta(cuenta);
        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        // Assert
        Assert.NotNull(cuentaObtenida);
        Assert.Equal("Carlos Diaz Updated", cuentaObtenida.Nombre);
    }

    [Fact]
    public void TestEliminarCuenta()
    {
        // Arrange
        var cuenta = new Cuenta
        {
            Nombre = "Delete Test",
            Pass = "deletepass",
            Dni = 9988776655,
            HoraRegistrada = DateTime.Now
        };

        Ado.AgregarCuenta(cuenta);

        // Act
        Ado.EliminarCuenta(cuenta.Ncuenta);
        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        // Assert
        Assert.Null(cuentaObtenida); // The record should not exist
    }

    [Fact]
    public void TestObtenerTodasLasCuentas()
    {
        // Arrange
        var cuenta1 = new Cuenta
        {
            Nombre = "First User",
            Pass = "pass1",
            Dni = 1111111111,
            HoraRegistrada = DateTime.Now
        };

        var cuenta2 = new Cuenta
        {
            Nombre = "Second User",
            Pass = "pass2",
            Dni = 2222222222,
            HoraRegistrada = DateTime.Now
        };

        Ado.AgregarCuenta(cuenta1);
        Ado.AgregarCuenta(cuenta2);

        // Act
        var cuentas = Ado.ObtenerTodasLasCuentas();

        // Assert
        Assert.NotEmpty(cuentas);
        Assert.Contains(cuentas, c => c.Nombre == "First User");
        Assert.Contains(cuentas, c => c.Nombre == "Second User");
    }
}