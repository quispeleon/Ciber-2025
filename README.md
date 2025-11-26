# Sistema de Gestión de Ciber

## Descripción
Sistema completo de gestión para un cibercafé desarrollado con ASP.NET Core MVC, Dapper y MySQL.

## Características

### 🏠 Dashboard Principal
- Vista general del estado del ciber
- Estadísticas en tiempo real
- Acceso rápido a todas las funcionalidades

### 👥 Gestión de Cuentas
- Crear nuevas cuentas de usuarios
- Editar información de cuentas existentes
- Ver detalles de cada cuenta
- Eliminar cuentas (con confirmación)

### 💻 Gestión de Máquinas
- Agregar nuevas máquinas al sistema
- Actualizar estado y características
- Filtrar por máquinas disponibles/ocupadas
- Eliminar máquinas del sistema

### ⏰ Gestión de Alquileres
- Crear nuevos alquileres
- Dos tipos de alquiler:
  - Tipo 1: Por tiempo específico
  - Tipo 2: Por cantidad de tiempo con estado de pago
- Ver detalles de alquileres activos
- Eliminar alquileres

### 📊 Historial de Alquileres
- Registrar alquileres completados
- Ver historial completo
- Calcular duración y totales
- Consultar detalles de cada registro

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core 9.0 MVC
- **Base de Datos**: MySQL con Dapper ORM
- **Frontend**: Bootstrap 5 + Font Awesome
- **Arquitectura**: Clean Architecture (Core, Dapper, MVC)

## Estructura del Proyecto

```
src/
├── Ciber.core/          # Entidades del dominio
├── Ciber.Dapper/        # Capa de acceso a datos
├── Ciber.MVC/           # Aplicación web MVC
├── Ciber.Test/          # Pruebas unitarias
└── MinimalAPI/           # API mínima alternativa
```

## Configuración

### 1. Base de Datos
Asegúrate de tener MySQL instalado y ejecuta los scripts en la carpeta `Scripts/`:

```sql
-- Ejecutar en orden:
-- 00 DDL.sql (estructura de tablas)
-- 01 SPF.sql (procedimientos almacenados)
-- 02 Triggers.sql (triggers)
-- 03 inserts.sql (datos iniciales)
-- 04 Roles.sql (roles y permisos)
```

### 2. Cadena de Conexión
Configura la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MySQL": "server=localhost;user=root;password=root;database=5to_ciber;CharSet=utf8mb4"
  }
}
```

### 3. Ejecutar la Aplicación

```bash
# Navegar al directorio del proyecto MVC
cd src/Ciber.MVC

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

La aplicación estará disponible en: `https://localhost:5001` o `http://localhost:5000`

## Funcionalidades Principales

### Dashboard
- **Total de Cuentas**: Número de usuarios registrados
- **Máquinas Disponibles**: Computadoras libres para usar
- **Máquinas Ocupadas**: Computadoras en uso
- **Alquileres Activos**: Sesiones en curso

### Navegación
- Menú principal con dropdowns organizados
- Acceso rápido a todas las funcionalidades
- Diseño responsive para móviles y tablets

### Interfaz de Usuario
- Diseño moderno con Bootstrap 5
- Iconos Font Awesome para mejor UX
- Alertas de éxito y error
- Confirmaciones para acciones destructivas
- Formularios con validación

## Características Técnicas

### Controladores
- **HomeController**: Dashboard principal
- **CuentaController**: CRUD de cuentas
- **MaquinaController**: CRUD de máquinas + filtros
- **AlquilerController**: Gestión de alquileres
- **HistorialController**: Registro de historial

### Vistas
- Layout responsive con navegación mejorada
- Vistas CRUD completas para todas las entidades
- Formularios con validación del lado cliente
- Tablas con acciones rápidas
- Modales de confirmación

### Base de Datos
- Procedimientos almacenados para operaciones complejas
- Triggers para automatización
- Índices optimizados
- Relaciones bien definidas

## Próximas Mejoras

- [ ] Sistema de autenticación y autorización
- [ ] Reportes y estadísticas avanzadas
- [ ] Notificaciones en tiempo real
- [ ] API REST completa
- [ ] Integración con sistemas de pago
- [ ] App móvil complementaria

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.