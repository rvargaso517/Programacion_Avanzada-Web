# GymManagement - Sistema de Gestión de Gimnasio

Este es el proyecto de gestión de gimnasio que incluye un portal web orientado al cliente (MVC) y un panel de administración para el personal, conectado a una API REST en .NET 8.

---

## 📋 Resumen de Cambios Recientes

1. **GitHub Desktop Limpio:** El archivo `.gitignore` ha sido optimizado para ignorar todas las carpetas temporales de compilación (`bin` y `obj`).
2. **Layout de Inicio Corregido:** La página principal pública (`Home/Index`) se renderiza de forma óptima e independiente del rol de la sesión.
3. **Mapeo Seguro de Citas:** El calendario de FullCalendar cuenta con control de errores (`try-catch`) y recarga forzada de caché (`asp-append-version`).
4. **Permisos y Accesos Dinámicos:** Nuevo panel en "Contenido Web > Roles y Accesos" para habilitar/deshabilitar accesos del menú lateral por rol con un switch en tiempo real.
5. **Agenda para Entrenador:** Filtrada para mostrar únicamente sus citas asignadas y sus reservas personales (`dbo.ReservasEntrenador`).
6. **Vídeos de Ejercicios:** Columna de vídeos corregida en rutinas mediante ajuste en procedimiento de base de datos.
7. **Edición Dinámica de Entrenadores:** Nueva pestaña en el panel para modificar nombres, especialidades y cargar fotos físicas del equipo.
8. **Visor de Logs de Errores:** Pestaña administrativa para monitorear errores del sistema y StackTraces en tiempo real.

---

## 🚀 Guía de Configuración e Inicio

### 1. Base de Datos (SSMS)
Para crear o restablecer la base de datos completa:
1. Abre **SQL Server Management Studio (SSMS)**.
2. Abre y ejecuta todo el archivo SQL ubicado en:
   `GymManagement_API/Database/00_GymManagementDB_Completo.sql`

### 2. Cadena de Conexión
Asegúrate de que la cadena de conexión en los archivos `appsettings.json` de **Tarea1** y **GymManagement_API** apunte a tu servidor de base de datos local:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=;Database=GymManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Configuración del Correo (Gmail)
Para habilitar el envío real de correos (recuperación de acceso, recibos, etc.) sin exponer contraseñas en el repositorio, ejecuta los siguientes comandos en la terminal de cada proyecto:

**En la carpeta `Tarea1`:**
```bash
dotnet user-secrets set "EmailSettings:SenderEmail" "correo"
dotnet user-secrets set "EmailSettings:SenderPassword" "contraseña"
```

**En la carpeta `GymManagement_API`:**
```bash
dotnet user-secrets set "EmailSettings:SenderEmail" "correo"
dotnet user-secrets set "EmailSettings:SenderPassword" "contraseña"
```

*Nota: Si no se configuran las credenciales, la aplicación simulará el envío guardando los archivos `.html` en las carpetas `EmailsSimulated` y `ReceiptsSimulated` dentro de la ruta de depuración (`bin/Debug/`).*

---

## 👥 Permisos de Roles por Defecto
El sistema cuenta con un archivo de permisos iniciales (`wwwroot/data/permisos.json`):
* **Administrador (Rol 1):** Acceso total a todos los módulos y gestión de accesos.
* **Recepcionista (Rol 2):** Clientes, Oportunidades, Agenda, Reservas y Pagos.
* **Entrenador (Rol 3):** Dashboard, Agenda, Rutinas y Reservas.
* **Cliente (Rol 4):** Agenda y Reservas (Vista de usuario).
