# LibraryCatalog

API de consulta para el catálogo de una biblioteca, desarrollada con .NET 10, ASP.NET Core, Clean Architecture, principios de Domain-Driven Design y Entity Framework Core con SQL Server.

Este README documenta el contexto funcional, la arquitectura implementada y el estado exacto del proyecto. También funciona como handoff para integrantes del equipo o asistentes de IA que deban continuar las siguientes fases.

## Contexto del proyecto

Una biblioteca necesita organizar y consultar la información existente de su catálogo. El sistema debe trabajar con:

- Libros.
- Autores.
- Categorías.

La primera versión es exclusivamente de lectura. No se requieren operaciones para registrar, modificar o eliminar información.

Los tres casos de uso que se implementarán posteriormente mediante CQRS son:

1. Consultar todos los libros.
2. Consultar un libro por su identificador.
3. Consultar los libros pertenecientes a una categoría.

Las respuestas deberán incluir, según corresponda, el identificador, título, ISBN, año de publicación, autor y categoría del libro.

## Tecnologías

- .NET 10.
- ASP.NET Core Web API.
- Entity Framework Core 10.
- SQL Server.
- MSTest 4.
- OpenAPI.
- Git y GitHub.

## Estado actual

La fase de arquitectura y dominio está terminada.

Actualmente el repositorio contiene:

- Una única solución multiproyecto.
- Las capas Domain, Application, Infrastructure y API.
- El proyecto de pruebas del dominio.
- Las entidades `Libro`, `Autor` y `Categoria`.
- Reglas básicas e invariantes del dominio.
- Relaciones uno-a-muchos configuradas con EF Core.
- `LibraryCatalogDbContext` preparado para SQL Server.
- Contrato e implementación del repositorio de lectura de libros.
- Registros de inyección de dependencias por capa.
- 16 pruebas unitarias aprobadas.
- API compilable y ejecutable con OpenAPI habilitado.

Todavía no existen migraciones, datos iniciales, queries CQRS, handlers ni endpoints del catálogo. Por ese motivo, el documento OpenAPI presenta actualmente una colección `paths` vacía. Este es el comportamiento esperado.

## Estructura de la solución

La única solución válida es `LibraryCatalog.slnx`, ubicada en la raíz del repositorio.

```text
LibraryCatalog.slnx
├── Core
│   ├── LibraryCatalog.Domain
│   └── LibraryCatalog.Application
├── Infrastructure
│   └── LibraryCatalog.Infrastructure
├── Presentation
│   └── LibraryCatalog.Api
└── Tests
    └── LibraryCatalog.Domain.Tests
```

Estructura física principal:

```text
src/
├── LibraryCatalog.Domain/
│   ├── Entities/
│   └── Exceptions/
├── LibraryCatalog.Application/
│   └── Contracts/Repositories/
├── LibraryCatalog.Infrastructure/
│   └── Persistence/
│       ├── Configurations/
│       └── Repositories/
└── LibraryCatalog.Api/

tests/
└── LibraryCatalog.Domain.Tests/
    └── Entities/
```

## Dependencias entre capas

```text
Domain
   ↑
Application
   ↑
Infrastructure
   ↑
API

Domain
   ↑
Domain.Tests
```

Referencias configuradas:

| Proyecto | Referencias permitidas |
|---|---|
| `LibraryCatalog.Domain` | Ninguna |
| `LibraryCatalog.Application` | Domain |
| `LibraryCatalog.Infrastructure` | Application y Domain |
| `LibraryCatalog.Api` | Application e Infrastructure |
| `LibraryCatalog.Domain.Tests` | Domain |

No deben introducirse referencias inversas. Domain no puede depender de EF Core, SQL Server, Application, Infrastructure ni API.

## Responsabilidad de cada capa

### Domain

Contiene entidades, relaciones conceptuales, reglas de negocio y excepciones propias. No conoce detalles de persistencia ni transporte HTTP.

### Application

Contiene contratos y será responsable de los casos de uso CQRS, DTO y handlers. Actualmente expone `ILibroRepository` y el punto de registro `AddApplicationServices`.

### Infrastructure

Implementa persistencia con EF Core y SQL Server. Contiene el contexto, configuraciones Fluent API, repositorios de lectura y `AddInfrastructureServices`.

### API

Es el punto de composición y ejecución. Registra Application e Infrastructure, controladores y OpenAPI. Los endpoints se agregarán en una fase posterior.

## Modelo de dominio

### Relaciones

```text
Autor 1 ─────── N Libro N ─────── 1 Categoria
```

- Un autor puede escribir varios libros.
- En esta versión, cada libro tiene exactamente un autor.
- Una categoría puede contener varios libros.
- Cada libro pertenece exactamente a una categoría.

No se debe convertir la relación Libro-Autor en muchos-a-muchos sin que el equipo cambie formalmente este requisito.

### Libro

Propiedades:

- `Id`.
- `Titulo`.
- `ISBN`.
- `AnioPublicacion`.
- `AutorId` y `Autor`.
- `CategoriaId` y `Categoria`.

Reglas implementadas:

- El título es obligatorio y admite hasta 256 caracteres.
- El ISBN es obligatorio.
- Se admiten formatos ISBN-10 e ISBN-13.
- El ISBN se almacena normalizado, sin espacios ni guiones.
- El año debe ser mayor que cero y no puede estar en el futuro.
- Los identificadores de autor y categoría son obligatorios.

### Autor

Propiedades:

- `Id`.
- `Nombre`.
- Colección de libros.

El nombre es obligatorio, se normaliza eliminando espacios externos y admite hasta 150 caracteres.

### Categoria

Propiedades:

- `Id`.
- `Nombre`.
- Colección de libros.

El nombre es obligatorio, se normaliza eliminando espacios externos y admite hasta 100 caracteres.

### Convenciones del dominio

- Las entidades son `sealed`.
- Los setters son privados.
- Los identificadores nuevos utilizan `Guid.CreateVersion7()`.
- Los constructores públicos aplican las reglas de negocio.
- Los constructores privados existen para EF Core.
- Las reglas inválidas lanzan `ReglaDeNegocioException`.
- No se agregaron métodos de edición porque la versión actual es de solo lectura.

## Persistencia con Entity Framework Core

El contexto es `LibraryCatalogDbContext` y expone:

- `DbSet<Libro> Libros`.
- `DbSet<Autor> Autores`.
- `DbSet<Categoria> Categorias`.

Las configuraciones Fluent API se descubren automáticamente desde el ensamblado de Infrastructure.

Configuración relevante:

- Tablas `Libros`, `Autores` y `Categorias`.
- ISBN obligatorio, longitud máxima de 13 y un índice único.
- Nombre de categoría único.
- Relaciones mediante `AutorId` y `CategoriaId`.
- `DeleteBehavior.Restrict` en ambas relaciones.
- Acceso por campo para las colecciones privadas de libros.

No existe una migración inicial todavía. El integrante responsable de persistencia deberá crearla cuando se defina la instancia local de SQL Server.

## Repositorio de lectura

`ILibroRepository` define solamente las operaciones requeridas por los futuros casos de uso:

```csharp
Task<IReadOnlyCollection<Libro>> ObtenerTodosAsync(
    CancellationToken cancellationToken = default);

Task<Libro?> ObtenerPorIdAsync(
    Guid id,
    CancellationToken cancellationToken = default);

Task<IReadOnlyCollection<Libro>> ObtenerPorCategoriaAsync(
    Guid categoriaId,
    CancellationToken cancellationToken = default);
```

`LibroRepository` implementa estas operaciones mediante consultas `AsNoTracking`, incluyendo `Autor` y `Categoria` y ordenando las colecciones por título.

No se debe agregar un repositorio genérico con `Create`, `Update` o `Delete`, porque esas operaciones no pertenecen a esta versión.

## Inyección de dependencias

`Program.cs` registra:

```csharp
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
```

Infrastructure registra:

- `LibraryCatalogDbContext` con SQL Server.
- `ILibroRepository` con `LibroRepository` y ciclo de vida scoped.

Application todavía no registra handlers porque los casos de uso CQRS están pendientes.

## Configuración y secretos

La aplicación lee la conexión desde:

```text
ConnectionStrings:LibraryCatalog
```

La entrada versionada en `appsettings.json` está vacía. No deben subirse usuarios, contraseñas, tokens ni cadenas privadas al repositorio.

Para configurar una conexión local con User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:LibraryCatalog" "<CADENA_DE_CONEXION_LOCAL>" --project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

El identificador `UserSecretsId` del proyecto API no es un secreto; solamente permite que .NET ubique la configuración privada fuera del repositorio.

El `.gitignore` excluye `.env`, archivos locales de configuración, `.vs`, `bin`, `obj` y archivos `*.user`.

## Compilar y ejecutar

Desde la raíz del repositorio:

```powershell
dotnet restore LibraryCatalog.slnx
dotnet build LibraryCatalog.slnx
dotnet test LibraryCatalog.slnx
dotnet run --project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

Resultado esperado:

```text
Build succeeded
0 Warning(s)
0 Error(s)

Passed: 16
Failed: 0
```

Al ejecutar la API, la terminal muestra las direcciones HTTP y HTTPS. El documento OpenAPI puede consultarse en:

```text
https://localhost:<PUERTO>/openapi/v1.json
```

Un `404` en la ruta `/` es normal porque todavía no hay un endpoint raíz. La API se detiene con `Ctrl + C`.

## Pruebas existentes

El proyecto `LibraryCatalog.Domain.Tests` contiene 16 pruebas para:

- Creación válida de libros, autores y categorías.
- Generación de identificadores.
- Normalización de nombres, títulos e ISBN.
- Campos obligatorios.
- Longitudes máximas.
- Formatos ISBN-10 e ISBN-13.
- Año cero o futuro.
- Autor y categoría obligatorios.

Las nuevas reglas del dominio deben incluir sus respectivas pruebas.

## Trabajo pendiente

Las siguientes fases no forman parte de la entrega de arquitectura y dominio ya terminada:

1. Configurar la instancia local de SQL Server.
2. Crear y aplicar la migración inicial.
3. Definir los DTO de salida.
4. Implementar con CQRS las queries y handlers para:
   - Todos los libros.
   - Libro por ID.
   - Libros por categoría.
5. Registrar los handlers mediante `AddApplicationServices`.
6. Crear controladores y endpoints HTTP.
7. Agregar datos iniciales o un mecanismo acordado para cargar información.
8. Crear pruebas para Application, Infrastructure y API.

No implementar commands de escritura salvo que el alcance oficial sea modificado.

## Comandos para una futura migración

Cuando exista una cadena de conexión local válida y esté disponible `dotnet-ef`, la migración se puede crear desde la raíz con:

```powershell
dotnet ef migrations add InitialCreate `
  --project src/LibraryCatalog.Infrastructure/LibraryCatalog.Infrastructure.csproj `
  --startup-project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj `
  --output-dir Persistence/Migrations
```

Para aplicarla:

```powershell
dotnet ef database update `
  --project src/LibraryCatalog.Infrastructure/LibraryCatalog.Infrastructure.csproj `
  --startup-project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

No ejecutar estos comandos contra una base compartida sin coordinarlo con el equipo.

## Handoff para integrantes y asistentes de IA

El siguiente bloque puede copiarse junto con este README al asistente que vaya a continuar el desarrollo:

```text
Trabaja sobre el repositorio LibraryCatalog respetando el README como contexto técnico y fuente de verdad.

El proyecto usa .NET 10, Clean Architecture, DDD, EF Core, SQL Server y CQRS. La primera versión es exclusivamente de consulta. La fase de arquitectura y dominio ya está terminada y verificada con 16 pruebas.

Antes de modificar código:
1. Revisa LibraryCatalog.slnx y el estado de Git.
2. Lee las entidades y reglas existentes en LibraryCatalog.Domain.
3. Lee ILibroRepository y LibroRepository antes de crear casos de uso.
4. Conserva la dirección actual de dependencias entre proyectos.
5. No agregues operaciones Create, Update o Delete.
6. No cambies Libro-Autor a muchos-a-muchos.
7. No incluyas credenciales ni cadenas privadas en archivos versionados.

Trabajo pendiente:
- Crear los DTO de lectura.
- Implementar las tres queries CQRS y sus handlers.
- Registrar los handlers en AddApplicationServices.
- Crear los endpoints de consulta.
- Configurar la base local y crear la migración inicial cuando corresponda.
- Añadir pruebas para cada nuevo caso de uso.

Las consultas deben devolver autor y categoría, reutilizar ILibroRepository y aceptar CancellationToken. Las consultas de EF Core ya usan AsNoTracking e incluyen las navegaciones necesarias.

Al terminar cada bloque, ejecuta dotnet build y dotnet test. Mantén los commits pequeños y no mezcles cambios ajenos a la fase asignada.
```

## Criterios para considerar terminada una fase

- La solución compila sin errores ni advertencias.
- Todas las pruebas pasan.
- La API inicia sin excepciones.
- No se versionan secretos ni artefactos generados.
- Las dependencias continúan apuntando hacia Domain.
- El commit contiene un cambio lógico y revisable.
