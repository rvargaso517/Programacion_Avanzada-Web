# Sistema Web Para La Gestión Integral De Gimnasios — Grupo 8

Solución `GymManagement.slnx` con dos proyectos:

| Proyecto | Carpeta | Framework | Qué es |
|---|---|---|---|
| `GymManagement_API` | `GymManagement_API/` | .NET 8 | Web API + Swagger |
| `GymManagement_WEB` | `Tarea1/` | .NET 10 | Aplicación MVC (Razor + Bootstrap) |

---

## 1. Crear la base de datos

Abrir en SSMS, conectado a **su** instancia local, y ejecutar completo (F5):

```
GymManagement_API/Database/00_GymManagementDB_Completo.sql
```

Es el **único** script de base de datos. Crea la base, 14 tablas, índices, llaves
foráneas, 61 procedimientos almacenados y los datos iniciales. Se puede volver a
ejecutar cuantas veces se quiera sin borrar nada: todo está protegido con
`IF NOT EXISTS` y `CREATE OR ALTER`.

Usuario que queda creado:

| Correo | Contraseña | Rol |
|---|---|---|
| `admin@gymmanagement.com` | `Admin123*` | Administrador |

> Cambiar esa contraseña después del primer ingreso.

---

## 2. Configurar la cadena de conexión

Los `appsettings.json` traen `Server=localhost`. **Si su instancia de SQL Server
tiene otro nombre, no edite ese archivo** (se sube al repositorio y le rompe la
configuración a los demás). Use *user secrets*, que se guardan solo en su equipo:

```bash
cd Tarea1
dotnet user-secrets set "ConnectionStrings:GymManagementDB" "Server=SU_INSTANCIA;Database=GymManagementDB;Integrated Security=True;TrustServerCertificate=True;"

cd ../GymManagement_API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=SU_INSTANCIA;Database=GymManagementDB;Integrated Security=True;TrustServerCertificate=True;"
```

Para ver lo que tiene guardado: `dotnet user-secrets list`

Ejemplos de `SU_INSTANCIA`: `localhost`, `localhost\SQLEXPRESS`,
`localhost\MSSQLSERVER01`, `.\SQLEXPRESS`.

---

## 3. Configurar el envío de correos (opcional)

**Las credenciales del correo NO van en `appsettings.json`.** Ese archivo se sube al
repositorio y cualquiera que lo vea puede usar la cuenta.

Si quiere que la aplicación mande correos de verdad, guarde la contraseña de
aplicación como secreto:

```bash
cd Tarea1
dotnet user-secrets set "EmailSettings:SenderEmail" "sucorreo@gmail.com"
dotnet user-secrets set "EmailSettings:SenderPassword" "su contrasena de aplicacion"
```

Lo mismo en `GymManagement_API` si quiere que la API mande los recibos.

**Sin configurar nada la aplicación igual funciona:** en vez de enviar el correo lo
guarda como archivo `.html` en la carpeta `EmailsSimulated` (WEB) o
`ReceiptsSimulated` (API), dentro de `bin/Debug/...`. Sirve perfectamente para probar
la recuperación de contraseña y los recordatorios.

---

## 4. Levantar la aplicación

Hay que correr **los dos** proyectos: el WEB le pide datos a la API.

```bash
# terminal 1
cd GymManagement_API
dotnet run --launch-profile https      # https://localhost:7013  (Swagger)

# terminal 2
cd Tarea1
dotnet run --launch-profile https      # https://localhost:7029
```

En Visual Studio: clic derecho en la solución → *Configurar proyectos de inicio* →
*Varios proyectos de inicio* → poner los dos en **Iniciar**.

---

## Seguridad

- Las contraseñas se guardan con **BCrypt** (workFactor 11), nunca en texto plano.
- El login emite un **JWT** que se guarda en una cookie `HttpOnly` llamada
  `access_token`. Los endpoints `/api/...` marcados con `[Authorize]` lo validan;
  también aceptan el encabezado `Authorization: Bearer <token>` para probar desde
  Swagger o Postman.
- La configuración del token está en la sección `Jwt` de los dos `appsettings.json`
  y **debe ser igual en ambos** para que los tokens sirvan en los dos proyectos.
- Todo error no controlado lo captura
  `Tarea1/Middleware/ManejoExcepcionesMiddleware.cs` y lo guarda en la tabla
  `dbo.LogErrores` con la ruta, el usuario y el stack trace.

> La clave `Jwt:Key` que viene en el repositorio es de desarrollo. En un despliegue
> real debe ir en user secrets o en una variable de entorno.

---

## Acceso a datos

Todo el acceso a datos usa **Dapper + procedimientos almacenados**. No hay SQL
escrito dentro de los controladores. Si necesita una consulta nueva, agregue el
procedimiento en `00_GymManagementDB_Completo.sql` y llámelo con
`commandType: CommandType.StoredProcedure`.

El detalle de todos los procedimientos está en
[Reporte_Procedimientos_Almacenados.md](Reporte_Procedimientos_Almacenados.md).
