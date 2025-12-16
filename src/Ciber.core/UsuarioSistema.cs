namespace Ciber.core;

public class UsuarioSistema
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Rol { get; set; } = ""; // ADMIN_GENERAL / ADMIN_FINANZAS
    public bool Activo { get; set; }
}
