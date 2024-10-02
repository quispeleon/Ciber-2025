# use json para tu db
```json
{
  "ConnectionStrings": {
    "CiberDb": "Server=localhost;Database=5to_Ciber;Uid=5to_agbd;pwd=Trigg3rs!;Allow User Variables=True",
    "Ciber": "Server=localhost;Database=Ciber;Uid=5to_agbd;pwd=Trigg3rs!;Allow User Variables=True"

  }
}
```

# En el Program.cs
```c#
using Microsoft.Extensions.Configuration;
var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

                        var cadena = configuration.GetConnectionString("CiberDb");

                        var cadena1 = configuration.GetConnectionString("Ciber");

```