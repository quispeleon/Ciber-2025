namespace Ciber.core;

public class Tipo
{
    public int IdTipo { get; set; }
    public string TipoDescripcion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty; // Nuevo campo
    public bool RequierePagoPrevio { get; set; } = false; // Nuevo campo
}