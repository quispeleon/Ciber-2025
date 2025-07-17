using Ciber.core;
using Xunit;

using Ciber.Dapper;
namespace Ciber.Test;
public class TestCuenta : TestAdo
{
   

   
    private IDAO Ado;
    public TestCuenta() : base()
    {
        Ado = new ADOD(Conexion);
    }

    [Fact]
    public void TestAgregarCuenta()
    {
   
        var cuenta = new Cuenta
        {
            Nombre = "doio 2sutup",
            Pass = "1192",
            Dni = 2391,
            HoraRegistrada =  new TimeSpan(0,0,0)
        };

      
        Ado.AgregarCuenta(cuenta);
        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        Assert.NotNull(cuentaObtenida);
        Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
        Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
    }

    [Fact]
    public void TestObtenerCuentaPorId()
    {
  
        var cuenta = new Cuenta
        {
            Nombre = "Pedro Lopez",
            Pass = "789123",
            Dni = 1163,
            HoraRegistrada = new TimeSpan (0,0,0)
        };

        Ado.AgregarCuenta(cuenta);


        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        Assert.NotNull(cuentaObtenida);
        Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
        Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void TestActualizarCuenta(int id )
    {

        
        var cuentaObtenida = Ado.ObtenerCuentaPorId(id);
        cuentaObtenida.Nombre = "Carlos Diaz Updated";
        cuentaObtenida.Pass = "passUpdated";
        Ado.ActualizarCuenta(cuentaObtenida);

    
        Assert.NotNull(cuentaObtenida);
        Assert.Equal("Carlos Diaz Updated", cuentaObtenida.Nombre);
    }




    [Fact]
    public void TestEliminarCuenta()
    {
    
        var cuenta = new Cuenta
        {
            Nombre = "Delete Test",
            Pass = "deletepass",
            Dni = 993,
            HoraRegistrada = new TimeSpan(0,0,0)
        };

        Ado.AgregarCuenta(cuenta);

        
        Ado.EliminarCuenta(cuenta.Ncuenta);
        var cuentaObtenida = Ado.ObtenerCuentaPorId(cuenta.Ncuenta);

        
        Assert.Null(cuentaObtenida); 
    }

    [Fact]
    public void TestObtenerTodasLasCuentas()
    {
   
        var cuenta1 = new Cuenta
        {
            Nombre = "First User",
            Pass = "pass1",
            Dni = 156111,
            HoraRegistrada = new TimeSpan(0,0,0)
        };

        var cuenta2 = new Cuenta
        {
            Nombre = "Second User",
            Pass = "pass2",
            Dni = 272,
            HoraRegistrada = new TimeSpan(0,0,0)
        };

        Ado.AgregarCuenta(cuenta1);
        Ado.AgregarCuenta(cuenta2);

   
        var cuentas = Ado.ObtenerTodasLasCuentas();

        Assert.NotEmpty(cuentas);
    }

    // async

    [Fact]
    public async Task TestAgregarCuentaAsync()
    {
        var cuenta = new Cuenta
        {
            Nombre = "Usuario Async",
            Pass = "clave123",
            Dni = 1111,
            HoraRegistrada = new TimeSpan(0, 0, 0)
        };

        await Ado.AgregarCuentaAsync(cuenta);

        var cuentaObtenida = await Ado.ObtenerCuentaPorIdAsync(cuenta.Ncuenta);

        Assert.NotNull(cuentaObtenida);
        Assert.Equal(cuenta.Nombre, cuentaObtenida.Nombre);
        Assert.Equal(cuenta.Dni, cuentaObtenida.Dni);
    }

    [Fact]
    public async Task TestActualizarCuentaAsync()
    {
        var cuenta = new Cuenta
        {
            Nombre = "Original",
            Pass = "pass",
            Dni = 2222,
            HoraRegistrada = new TimeSpan(0, 0, 0)
        };

        await Ado.AgregarCuentaAsync(cuenta);
        cuenta.Nombre = "Actualizado";
        cuenta.Pass = "nuevoPass";

        await Ado.ActualizarCuentaAsync(cuenta);
        var cuentaObtenida = await Ado.ObtenerCuentaPorIdAsync(cuenta.Ncuenta);

        Assert.Equal("Actualizado", cuentaObtenida.Nombre);

        // Assert.Equal("nuevoPass", cuentaObtenida.Pass);

    }

    [Fact]
    public async Task TestEliminarCuentaAsync()
    {
        var cuenta = new Cuenta
        {
            Nombre = "Eliminar",
            Pass = "delete",
            Dni = 3333,
            HoraRegistrada = new TimeSpan(0, 0, 0)
        };

        await Ado.AgregarCuentaAsync(cuenta);
        await Ado.EliminarCuentaAsync(cuenta.Ncuenta);
        var cuentaObtenida = await Ado.ObtenerCuentaPorIdAsync(cuenta.Ncuenta);

        Assert.Null(cuentaObtenida);
    }

    [Fact]
    public async Task TestObtenerTodasLasCuentasAsync()
    {
        var cuenta1 = new Cuenta { Nombre = "Uno", Pass = "1", Dni = 123, HoraRegistrada = new TimeSpan(0, 0, 0) };
        var cuenta2 = new Cuenta { Nombre = "Dos", Pass = "2", Dni = 456, HoraRegistrada = new TimeSpan(0, 0, 0) };

        await Ado.AgregarCuentaAsync(cuenta1);
        await Ado.AgregarCuentaAsync(cuenta2);

        var cuentas = await Ado.ObtenerTodasLasCuentasAsync();

        Assert.Contains(cuentas, c => c.Nombre == "Uno");
        Assert.Contains(cuentas, c => c.Nombre == "Dos");
    }
}