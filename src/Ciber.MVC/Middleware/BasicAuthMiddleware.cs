using System.Net;
using System.Text;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _username;
    private readonly string _password;

    public BasicAuthMiddleware(RequestDelegate next, string username, string password)
    {
        _next = next;
        _username = username;
        _password = password;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string authHeader = context.Request.Headers["Authorization"];

        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            string decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var parts = decodedUsernamePassword.Split(':');
            if (parts.Length == 2)
            {
                var username = parts[0];
                var password = parts[1];

                if (username == _username && password == _password)
                {
                    await _next(context);
                    return;
                }
            }
        }

        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Ciber\"";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Usuario o contraseña incorrectos.");
    }
}
