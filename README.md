# LibraryCatalog

API de consulta para el catálogo de una biblioteca, desarrollada con .NET 10, ASP.NET Core, Clean Architecture, principios de Domain-Driven Design y Entity Framework Core con SQL Server.

Este README documenta el contexto funcional, la arquitectura implementada y el estado exacto del proyecto. También funciona como handoff para integrantes del equipo o asistentes de IA que deban continuar las siguientes fases.

## Tabla de contenido

1. [Contexto del proyecto](#contexto-del-proyecto)
2. [Tecnologías](#tecnologías)
3. [Estado actual](#estado-actual)
4. [Estructura de la solución](#estructura-de-la-solución)
5. [Dependencias entre capas](#dependencias-entre-capas)
6. [Responsabilidad de cada capa](#responsabilidad-de-cada-capa)
7. [Modelo de dominio](#modelo-de-dominio)
8. [Persistencia con Entity Framework Core](#persistencia-con-entity-framework-core)
9. [Base de datos](#base-de-datos)
10. [Repositorio de lectura](#repositorio-de-lectura)
11. [Casos de uso CQRS](#casos-de-uso-cqrs)
12. [Inyección de dependencias](#inyección-de-dependencias)
13. [Configuración y secretos](#configuración-y-secretos)
14. [Compilar y ejecutar](#compilar-y-ejecutar)
15. [Pruebas existentes](#pruebas-existentes)
16. [Pruebas de la API](#pruebas-de-la-api)
17. [Trabajo pendiente](#trabajo-pendiente)
18. [Comandos de migración](#comandos-de-migración)
19. [Handoff para integrantes y asistentes de IA](#handoff-para-integrantes-y-asistentes-de-ia)
20. [Criterios para considerar terminada una fase](#criterios-para-considerar-terminada-una-fase)

---

## Contexto del proyecto

Una biblioteca necesita organizar y consultar la información existente de su catálogo. El sistema debe trabajar con:

- Libros.
- Autores.
- Categorías.

La primera versión es exclusivamente de lectura. No se requieren operaciones para registrar, modificar o eliminar información.

Los tres casos de uso se implementan mediante CQRS:

1. Consultar todos los libros.
2. Consultar un libro por su identificador.
3. Consultar los libros pertenecientes a una categoría.

Las respuestas deberán incluir, según corresponda, el identificador, título, ISBN, año de publicación, autor y categoría del libro.

## Tecnologías

- .NET 10.
- ASP.NET Core Web API.
- Entity Framework Core 10.
- SQL Server.
- Docker, para ejecutar el motor de base de datos.
- MSTest 4.
- OpenAPI.
- Git y GitHub.

## Estado actual

Todas las fases están terminadas. Las tres queries CQRS están implementadas en la capa Application, sus handlers están registrados y la API expone un endpoint por cada caso de uso. Los tres endpoints se probaron contra la base de datos con los datos de prueba (ver [Pruebas de la API](#pruebas-de-la-api)).

### Endpoints disponibles

| Método | Ruta | Caso de uso | Respuestas |
|---|---|---|---|
| `GET` | `/api/libros` | Query 1 — Todos los libros | `200 OK` con la lista (vacía si no hay libros) |
| `GET` | `/api/libros/{id}` | Query 2 — Libro por ID | `200 OK` con el libro, `404 Not Found` si no existe |
| `GET` | `/api/libros/categoria/{categoriaId}` | Query 3 — Libros por categoría | `200 OK` con la lista (vacía si la categoría no tiene libros) |

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
- Cadena de conexión configurada y validada al registrar el contexto.
- Migración inicial `InitialCreate` con el esquema completo.
- Datos de prueba incluidos en la migración: 6 autores, 5 categorías y 10 libros.
- Resiliencia de conexión ante fallos transitorios de red.
- `docker-compose.yml` para levantar SQL Server como proceso independiente.
- Script SQL de verificación de esquema, relaciones y datos.
- `LibroDto` y las queries 2 y 3 con sus handlers en la capa Application.
- Query 1 y su handler en la capa Application, registrado en `AddApplicationServices`.
- Handlers de las tres queries registrados en `AddApplicationServices`.
- `LibrosController` con los tres endpoints de consulta.
- Archivo `LibraryCatalog.Api.http` con las peticiones de prueba de los tres endpoints.

### Avance de los casos de uso

| Caso de uso | Estado |
|---|---|
| `LibroDto` | Completado |
| Query 1 — Todos los libros | Completado |
| Query 2 — Libro por ID | Completado |
| Query 3 — Libros por categoría | Completado |
| Registro de handlers | Completado (las tres queries) |
| Endpoints HTTP | Completado (los tres endpoints) |
| Pruebas de la API | Validadas manualmente contra la base de datos |

### Validación de la solución

La solución completa fue compilada desde la raíz del repositorio mediante:

```powershell
dotnet build
```

Resultado obtenido:

```text
LibraryCatalog.Domain           OK
LibraryCatalog.Application      OK
LibraryCatalog.Domain.Tests     OK
LibraryCatalog.Infrastructure   OK
LibraryCatalog.Api              OK

Compilación realizada correctamente
```

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
docker-compose.yml

scripts/
└── verificacion-catalogo.sql

src/
├── LibraryCatalog.Domain/
│   ├── Entities/
│   └── Exceptions/
│
├── LibraryCatalog.Application/
│   ├── Contracts/
│   │   └── Repositories/
│   │       └── ILibroRepository.cs
│   └── Queries/
│       ├── DTOs/
│       │   └── LibroDto.cs
│       ├── GetBooksList/
│       │   ├── GetBooksListQuery.cs
│       │   └── GetBooksListHandler.cs
│       ├── GetBookById/
│       │   ├── GetBookByIdQuery.cs
│       │   └── GetBookByIdHandler.cs
│       └── GetBooksByCategory/
│           ├── GetBooksByCategoryQuery.cs
│           └── GetBooksByCategoryHandler.cs
│
├── LibraryCatalog.Infrastructure/
│   └── Persistence/
│       ├── Configurations/
│       ├── Migrations/
│       ├── Repositories/
│       └── Seed/
│
└── LibraryCatalog.Api/
    └── Controllers/
        └── LibrosController.cs

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

Contiene contratos y los casos de uso CQRS, DTO y handlers. Actualmente contiene:

- `ILibroRepository`.
- `LibroDto`.
- Query 1 y su handler.
- Query 2 y su handler.
- Query 3 y su handler.
- El punto de registro `AddApplicationServices`.

### Infrastructure

Implementa persistencia con EF Core y SQL Server. Contiene el contexto, configuraciones Fluent API, repositorios de lectura, migraciones, datos de prueba y `AddInfrastructureServices`.

### API

Es el punto de composición y ejecución. Registra Application e Infrastructure, controladores y OpenAPI. Contiene `LibrosController`, que recibe los tres handlers por inyección de dependencias y expone un endpoint por cada query. El controlador no contiene lógica de negocio: construye la query, invoca el handler y traduce el resultado a una respuesta HTTP.

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

La migración inicial ya existe y está documentada en la sección [Base de datos](#base-de-datos).

## Base de datos

El esquema y los datos de prueba se crean a partir de la migración `InitialCreate`, por lo que cualquier integrante puede levantar una base idéntica sin ejecutar scripts manuales.

### Requisitos

- .NET 10 SDK.
- Docker Desktop, o una instancia local de SQL Server donde el usuario tenga permisos para crear bases de datos.
- Herramienta de línea de comandos de EF Core:

```powershell
dotnet tool install --global dotnet-ef
```

### Levantar la base de datos con Docker

Desde la raíz del repositorio:

**1. Crear el archivo `.env`** con la contraseña del motor. Está en `.gitignore`, por lo tanto no se versiona.

```text
MSSQL_SA_PASSWORD=Biblioteca_2026
```

**2. Levantar el contenedor.**

```powershell
docker compose up -d
```

**3. Registrar la cadena de conexión fuera del repositorio.**

```powershell
dotnet user-secrets set "ConnectionStrings:LibraryCatalog" "Server=localhost,1433;Database=LibraryCatalog;User Id=sa;Password=Biblioteca_2026;TrustServerCertificate=True" --project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

**4. Aplicar la migración.**

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project src/LibraryCatalog.Infrastructure/LibraryCatalog.Infrastructure.csproj --startup-project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

> La variable `ASPNETCORE_ENVIRONMENT` con valor `Development` es obligatoria, porque los User Secrets solo se cargan en ese entorno. En `cmd` se define con `set ASPNETCORE_ENVIRONMENT=Development` y en Linux o macOS con `export`.

### Alternativa sin Docker

Con una instancia local de SQL Server basta con ajustar la cadena de `src/LibraryCatalog.Api/appsettings.json` y aplicar la migración. Se omiten los pasos 1 a 3.

Si se instala SQL Server Developer con la instalación básica, se crea la instancia por defecto en `localhost` y la cadena versionada funciona sin cambios. Con SQL Server Express la instancia se llama `SQLEXPRESS`; en ese caso la cadena se registra con User Secrets en lugar de modificar el archivo versionado:

```powershell
dotnet user-secrets set "ConnectionStrings:LibraryCatalog" "Server=localhost\SQLEXPRESS;Database=LibraryCatalog;Trusted_Connection=True;TrustServerCertificate=True" --project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

```json
"ConnectionStrings": {
  "LibraryCatalog": "Server=localhost;Database=LibraryCatalog;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Modelo físico

| Tabla | Columnas | Restricciones |
|---|---|---|
| `Autores` | `Id` uniqueidentifier, `Nombre` nvarchar(150) | Clave primaria en `Id` |
| `Categorias` | `Id` uniqueidentifier, `Nombre` nvarchar(100) | Clave primaria en `Id`, índice único en `Nombre` |
| `Libros` | `Id` uniqueidentifier, `Titulo` nvarchar(256), `ISBN` nvarchar(13), `AnioPublicacion` int, `AutorId`, `CategoriaId` | Clave primaria en `Id`, índice único en `ISBN`, índices en `AutorId` y `CategoriaId` |

Las dos relaciones son uno a muchos. `Libros.AutorId` referencia `Autores.Id` y `Libros.CategoriaId` referencia `Categorias.Id`. Ambas se generan con `ON DELETE NO ACTION`, lo que impide eliminar un autor o una categoría que tenga libros asociados.

Las longitudes de las columnas coinciden con las invariantes declaradas en las entidades del dominio.

### Datos de prueba

La migración incluye 6 autores, 5 categorías y 10 libros mediante `HasData`. Los identificadores son fijos y legibles, de modo que la migración es determinista y las consultas se pueden probar sin consultar antes la base.

| Categoría | Id | Libros |
|---|---|---|
| Novela | `c0000000-0000-4000-8000-000000000001` | 3 |
| Cuento | `c0000000-0000-4000-8000-000000000002` | 2 |
| Ciencia ficción | `c0000000-0000-4000-8000-000000000003` | 2 |
| Ingeniería de software | `c0000000-0000-4000-8000-000000000004` | 2 |
| Historia | `c0000000-0000-4000-8000-000000000005` | 1 |

Los libros van de `b0000000-0000-4000-8000-000000000001` a `b0000000-0000-4000-8000-000000000010`. Por ejemplo, `b0000000-0000-4000-8000-000000000008` corresponde a *Clean Code*.

Los datos se declaran con objetos anónimos en lugar de instancias del dominio, porque el constructor de `Libro` genera el identificador con `Guid.CreateVersion7()` y eso haría que la migración cambiara en cada ejecución. Por la misma razón el ISBN se escribe ya normalizado: `HasData` no pasa por el constructor y por lo tanto no ejecuta la normalización del dominio.

### Resiliencia de conexión

El registro del contexto habilita `EnableRetryOnFailure` con 3 reintentos y 5 segundos de espera, más un `CommandTimeout` de 30 segundos. La base de datos es un proceso remoto y la conexión puede fallar de forma transitoria. Las consultas de esta versión son de solo lectura, por lo tanto idempotentes, y reintentarlas no produce efectos secundarios.

La estrategia de reintentos es incompatible con transacciones iniciadas manualmente. Si en una fase futura se agregan comandos de escritura, deberán envolverse con `CreateExecutionStrategy`.

### Verificación

Estado de la migración:

```powershell
dotnet ef migrations list --project src/LibraryCatalog.Infrastructure/LibraryCatalog.Infrastructure.csproj --startup-project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

Esquema, relaciones y datos:

```powershell
docker cp scripts/verificacion-catalogo.sql librarycatalog-sqlserver:/tmp/verificacion.sql
docker exec librarycatalog-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Biblioteca_2026" -C -i /tmp/verificacion.sql
```

Resultado esperado: 6 autores, 5 categorías, 10 libros y 0 libros huérfanos.

Reconstrucción completa desde cero, útil para comprobar que la migración es reproducible en cualquier equipo:

```powershell
dotnet ef database drop -f --project src/LibraryCatalog.Infrastructure/LibraryCatalog.Infrastructure.csproj --startup-project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
dotnet ef database update --project src/LibraryCatalog.Infrastructure/LibraryCatalog.Infrastructure.csproj --startup-project src/LibraryCatalog.Api/LibraryCatalog.Api.csproj
```

## Repositorio de lectura

`ILibroRepository` define las operaciones requeridas por los casos de uso:

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

Los handlers implementados reutilizan estas abstracciones y no acceden directamente al contexto de EF Core.

No se debe agregar un repositorio genérico con `Create`, `Update` o `Delete`, porque esas operaciones no pertenecen a esta versión.

## Casos de uso CQRS

### DTO de lectura

El DTO utilizado por las queries es:

```csharp
public sealed record LibroDto(
    Guid Id,
    string Titulo,
    string ISBN,
    int AnioPublicacion,
    Guid AutorId,
    string Autor,
    Guid CategoriaId,
    string Categoria);
```

El DTO evita exponer directamente las entidades de dominio como respuesta de los casos de uso de lectura.

### Características comunes de los handlers

- Reutilizan `ILibroRepository`.
- Proyectan las entidades de dominio hacia `LibroDto`.
- Incluyen la información del autor y la categoría.
- Aceptan `CancellationToken`.

### Query 1 — Consultar todos los libros

**Estado:** Completado.

Archivos:

```text
src/LibraryCatalog.Application/Queries/GetBooksList/
├── GetBooksListQuery.cs
└── GetBooksListHandler.cs

src/LibraryCatalog.Api/Controllers/
└── LibrosController.cs
```

Obtiene el listado completo de libros registrados en el catálogo.

```text
GET /api/libros
       ↓
LibrosController
       ↓
GetBooksListQuery
       ↓
GetBooksListHandler
       ↓
ILibroRepository.ObtenerTodosAsync()
       ↓
Colección de LibroDto
```

El handler utiliza `ObtenerTodosAsync()`, que consulta con EF Core incluyendo `Autor` y `Categoria`, y retorna una colección de `LibroDto` ordenada por título. Cada resultado incluye Id, título, ISBN, año de publicación, autor y categoría.

La query reutiliza `LibroDto` en lugar de definir un DTO propio, porque ese DTO ya contiene exactamente los campos que exige el caso de uso.

El handler se registra en `AddApplicationServices` y se inyecta directamente en `LibrosController`, sin mediador, siguiendo el mismo estilo de las queries 2 y 3.

Ejemplo de respuesta (`200 OK`):

```json
[
  {
    "id": "b0000000-0000-4000-8000-000000000001",
    "titulo": "Cien años de soledad",
    "isbn": "9788439700012",
    "anioPublicacion": 1967,
    "autorId": "a0000000-0000-4000-8000-000000000001",
    "autor": "Gabriel García Márquez",
    "categoriaId": "c0000000-0000-4000-8000-000000000001",
    "categoria": "Novela"
  }
]
```

Con la base de datos de prueba, el endpoint retorna los 10 libros. Si no hay libros registrados, retorna `200 OK` con una colección vacía.

### Query 2 — Consultar un libro por ID

**Estado:** Completado.

Archivos:

```text
src/LibraryCatalog.Application/Queries/GetBookById/
├── GetBookByIdQuery.cs
└── GetBookByIdHandler.cs
```

Busca un libro específico mediante su `Guid`.

```text
GET /api/libros/{id}
       ↓
LibrosController.GetById
       ↓
GetBookByIdQuery
       ↓
GetBookByIdHandler
       ↓
ILibroRepository.ObtenerPorIdAsync()
       ↓
LibroDto o null
```

El handler utiliza `ObtenerPorIdAsync()` y retorna un `LibroDto` cuando encuentra el libro. El resultado incluye Id, título, ISBN, año de publicación, autor y categoría.

Si el libro no existe, el handler retorna `null` y el controlador responde `404 Not Found`.

Ejemplo: `GET /api/libros/b0000000-0000-4000-8000-000000000008` (`200 OK`):

```json
{
  "id": "b0000000-0000-4000-8000-000000000008",
  "titulo": "Clean Code",
  "isbn": "9780132350884",
  "anioPublicacion": 2008,
  "autorId": "a0000000-0000-4000-8000-000000000005",
  "autor": "Robert C. Martin",
  "categoriaId": "c0000000-0000-4000-8000-000000000004",
  "categoria": "Ingeniería de software"
}
```

### Query 3 — Consultar libros por categoría

**Estado:** Completado.

Archivos:

```text
src/LibraryCatalog.Application/Queries/GetBooksByCategory/
├── GetBooksByCategoryQuery.cs
└── GetBooksByCategoryHandler.cs
```

Busca todos los libros pertenecientes a una categoría.

```text
GET /api/libros/categoria/{categoriaId}
       ↓
LibrosController.GetByCategoria
       ↓
GetBooksByCategoryQuery
       ↓
GetBooksByCategoryHandler
       ↓
ILibroRepository.ObtenerPorCategoriaAsync()
       ↓
Colección de LibroDto
```

El handler utiliza `ObtenerPorCategoriaAsync()` y retorna una colección de `LibroDto`. Cada resultado incluye Id, título, ISBN, año de publicación, autor y categoría.

Si la categoría no tiene libros o no existe, el endpoint responde `200 OK` con una colección vacía (`[]`). Con los datos de prueba, la categoría Novela (`c0000000-0000-4000-8000-000000000001`) retorna 3 libros.

## Inyección de dependencias

`Program.cs` registra:

```csharp
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
```

Infrastructure registra:

- `LibraryCatalogDbContext` con SQL Server, reintentos ante fallos transitorios y tiempo límite de comando.
- `ILibroRepository` con `LibroRepository` y ciclo de vida scoped.

Application registra los tres handlers con ciclo de vida scoped:

```csharp
services.AddScoped<GetBooksListHandler>();
services.AddScoped<GetBookByIdHandler>();
services.AddScoped<GetBooksByCategoryHandler>();
```

`LibrosController` recibe los tres handlers por constructor. Si se agrega una nueva query, su handler debe registrarse aquí antes de inyectarlo en un controlador; de lo contrario la API falla al resolver el controlador.

## Configuración y secretos

La aplicación lee la conexión desde:

```text
ConnectionStrings:LibraryCatalog
```

La entrada versionada en `appsettings.json` contiene una cadena de desarrollo con autenticación integrada de Windows, que no incluye credenciales. No deben subirse usuarios, contraseñas, tokens ni cadenas privadas al repositorio. Cualquier cadena con contraseña, como la del contenedor de Docker, se registra con User Secrets.

Si la cadena no está configurada, `AddInfrastructureServices` lanza una excepción con un mensaje explícito en lugar de fallar al abrir la conexión.

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

La compilación y las pruebas del dominio no requieren base de datos. Para ejecutar consultas contra el catálogo sí es necesario que el motor esté disponible según la sección [Base de datos](#base-de-datos).

Al ejecutar la API, la terminal muestra las direcciones HTTP y HTTPS. El documento OpenAPI puede consultarse en:

```text
https://localhost:<PUERTO>/openapi/v1.json
```

Los endpoints pueden consultarse desde el navegador o con el archivo `src/LibraryCatalog.Api/LibraryCatalog.Api.http` desde Visual Studio:

```text
http://localhost:5169/api/libros
http://localhost:5169/api/libros/{id}
http://localhost:5169/api/libros/categoria/{categoriaId}
```

Un `404` en la ruta `/` es normal porque no hay un endpoint raíz. La API se detiene con `Ctrl + C`.

El aviso `Failed to determine the https port for redirect` al ejecutar con el perfil `http` es esperado y no afecta el funcionamiento.

### Conflicto con un SQL Server instalado en Windows

Si el equipo ya tiene SQL Server instalado como servicio de Windows (instancia `MSSQLSERVER`), ese servicio ocupa el puerto `1433` y responde en lugar del contenedor. El síntoma es `Login failed for user 'sa'` al aplicar la migración, aunque la contraseña sea correcta.

Para usar el contenedor, detener el servicio local desde una terminal de administrador antes de ejecutar `docker compose up -d`:

```powershell
Stop-Service MSSQLSERVER
```

Alternativa: no usar Docker y trabajar con la instancia local, eliminando el User Secret para que se use la cadena de `appsettings.json`.

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

Resultado de `dotnet test`: 16 pruebas, 16 aprobadas, 0 fallidas.

Las nuevas reglas del dominio deben incluir sus respectivas pruebas.

## Pruebas de la API

Los tres endpoints se validaron manualmente con la API en ejecución, contra SQL Server en Docker y la base creada con la migración `InitialCreate`. Cada petición recorre la solución completa: controlador, handler, repositorio, EF Core y SQL Server. Las peticiones están guardadas en `src/LibraryCatalog.Api/LibraryCatalog.Api.http`.

| # | Caso | Petición | Resultado esperado | Resultado |
|---|---|---|---|---|
| 1 | Todos los libros | `GET /api/libros` | `200 OK` con 10 libros ordenados por título, con autor y categoría | Correcto |
| 2 | Libro existente | `GET /api/libros/b0000000-0000-4000-8000-000000000008` | `200 OK` con *Clean Code*, autor Robert C. Martin | Correcto |
| 3 | Libro inexistente | `GET /api/libros/00000000-0000-0000-0000-000000000000` | `404 Not Found` | Correcto |
| 4 | Categoría con libros | `GET /api/libros/categoria/c0000000-0000-4000-8000-000000000001` | `200 OK` con los 3 libros de Novela | Correcto |
| 5 | Categoría sin libros | `GET /api/libros/categoria/00000000-0000-0000-0000-000000000000` | `200 OK` con `[]` | Correcto |

Validaciones complementarias:

- `dotnet build`: la solución compila sin errores.
- `dotnet test`: 16 de 16 pruebas del dominio aprobadas.
- `dotnet ef database update`: la base se crea desde cero con la migración inicial.
- El log de EF Core confirma que cada consulta incluye los `JOIN` con `Autores` y `Categorias`.

## Trabajo pendiente

### Fases completadas

- Configurar la instancia local de SQL Server.
- Crear y aplicar la migración inicial.
- Incluir datos iniciales mediante `HasData`.
- Definir el DTO de salida `LibroDto`.
- Implementar Query 1 y su handler.
- Implementar Query 2 y su handler.
- Implementar Query 3 y su handler.
- Registrar el handler de Query 1 y exponer `GET /api/libros`.
- Registrar los handlers de Query 2 y Query 3 en `AddApplicationServices`.
- Exponer `GET /api/libros/{id}` y `GET /api/libros/categoria/{categoriaId}`.
- Validar los tres endpoints contra la base de datos.

### Mejoras opcionales

Fuera del alcance del Seguimiento 1:

1. Pruebas automáticas para los handlers de Application.
2. Pruebas de integración automáticas para la API.

No implementar commands de escritura salvo que el alcance oficial sea modificado.

## Comandos de migración

Los comandos se ejecutan desde la raíz del repositorio con `dotnet-ef` disponible y una cadena de conexión válida.

Crear una migración nueva:

```powershell
dotnet ef migrations add <NombreMigracion> `
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

En `cmd` los comandos van en una sola línea, sin el acento invertido de continuación.

> No ejecutar estos comandos contra una base compartida sin coordinarlo con el equipo. Las migraciones existentes no se modifican ni se eliminan del repositorio: cualquier cambio de esquema se hace con una migración nueva.

## Handoff para integrantes y asistentes de IA

El siguiente bloque puede copiarse junto con este README al asistente que vaya a continuar el desarrollo:

```text
Trabaja sobre el repositorio LibraryCatalog respetando el README como contexto técnico y fuente de verdad.

El proyecto usa .NET 10, Clean Architecture, DDD, EF Core, SQL Server y CQRS. La primera versión es exclusivamente de consulta.

Las fases de arquitectura y dominio, y de persistencia y base de datos, ya están terminadas. El dominio está verificado con 16 pruebas y la base de datos se crea con la migración InitialCreate, que incluye 6 autores, 5 categorías y 10 libros.

La capa Application ya contiene:
- LibroDto.
- Query 1: GetBooksListQuery y GetBooksListHandler.
- Query 2: GetBookByIdQuery y GetBookByIdHandler.
- Query 3: GetBooksByCategoryQuery y GetBooksByCategoryHandler.

La API contiene LibrosController con GET /api/libros (Query 1), GET /api/libros/{id} (Query 2, 404 si no existe) y GET /api/libros/categoria/{categoriaId} (Query 3). Los tres handlers se registran en AddApplicationServices y se inyectan directamente en el controlador, sin mediador. Los tres endpoints fueron validados contra la base de datos.

Los handlers reutilizan ILibroRepository, aceptan CancellationToken y proyectan las entidades a LibroDto.

Antes de modificar código:
1. Revisa LibraryCatalog.slnx y el estado de Git.
2. Lee las entidades y reglas existentes en LibraryCatalog.Domain.
3. Lee ILibroRepository y LibroRepository antes de crear o modificar casos de uso.
4. Conserva la dirección actual de dependencias entre proyectos.
5. No agregues operaciones Create, Update o Delete.
6. No cambies Libro-Autor a muchos-a-muchos.
7. No incluyas credenciales ni cadenas privadas en archivos versionados.
8. No modifiques ni elimines las migraciones existentes.
9. No sobrescribas el trabajo de otros integrantes.

El alcance del Seguimiento 1 está completo. Mejoras opcionales:
- Añadir pruebas automáticas para los handlers.
- Añadir pruebas de integración automáticas para la API.

Las consultas deben devolver autor y categoría, reutilizar ILibroRepository y aceptar CancellationToken. Las consultas de EF Core ya usan AsNoTracking e incluyen las navegaciones necesarias.

Para probar contra datos reales, la categoría Novela es c0000000-0000-4000-8000-000000000001 y los libros van de b0000000-0000-4000-8000-000000000001 a b0000000-0000-4000-8000-000000000010.

Al terminar cada bloque, ejecuta dotnet build y dotnet test. Mantén los commits pequeños y no mezcles cambios ajenos a la fase asignada.
```

## Criterios para considerar terminada una fase

- La solución compila sin errores ni advertencias.
- Todas las pruebas correspondientes pasan.
- La API inicia sin excepciones.
- La base de datos se puede recrear desde cero con la migración inicial.
- No se versionan secretos ni artefactos generados.
- Las dependencias continúan apuntando hacia Domain.
- El commit contiene un cambio lógico y revisable.
- Los cambios de cada integrante permanecen separados y son integrables.