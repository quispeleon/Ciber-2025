namespace Ciber.core;

public static class Enums
{
    public static class EstadoMaquina
    {
        public const string Disponible = "Disponible";
        public const string Ocupada = "Ocupada";
        public const string Mantenimiento = "Mantenimiento";
        public const string Reservada = "Reservada";
    }
    
    public static class EstadoAlquiler
    {
        public const string Activo = "Activo";
        public const string Finalizado = "Finalizado";
        public const string Cancelado = "Cancelado";
        public const string PendientePago = "Pendiente Pago";
    }
    
    public static class TipoTransaccion
    {
        public const string Recarga = "Recarga";
        public const string Consumo = "Consumo";
        public const string Devolucion = "Devolucion";
    }
    
    public static class TipoAlquiler
    {
        public const int Libre = 1;
        public const int HoraDefinida = 2;
        public const int Reserva = 3;
    }
}