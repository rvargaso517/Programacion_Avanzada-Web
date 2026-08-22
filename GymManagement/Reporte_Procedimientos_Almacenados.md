# Reporte de Procedimientos Almacenados (SPs) - GymManagement

Este documento contiene la lista de todos los procedimientos almacenados requeridos en el proyecto, extraídos automáticamente de las llamadas en el código C# y de las definiciones en SQL.

Todo vive ahora en un **único** script de base de datos, que crea las tablas, los índices,
las llaves foráneas, los procedimientos y los datos iniciales:

* **Archivo de SQL:** [GymManagement_API/Database/00_GymManagementDB_Completo.sql](GymManagement_API/Database/00_GymManagementDB_Completo.sql)

Los scripts anteriores (Basededatos.sql, 01_CreateStoredProcedures.sql,
Todos_Los_Stored_Procedures_Requeridos.sql, Sps_nuevos.sql y script.sql) fueron
eliminados: su contenido estaba duplicado y ya está incluido en el script único.

---

## 🚨 Stored Procedures que Faltaban en el Script Original (Ya Agregados)

Estos SPs son llamados por el API (HomeController.cs) pero no estaban definidos en 1_CreateStoredProcedures.sql:

1. **dbo.SP_RegistrarUsuario**
   * **Propósito:** Registra un nuevo usuario en la base de datos (con rol de Cliente) y crea su respectiva ficha en la tabla de Clientes de manera transaccional.
   * **Parámetros:** @Nombre, @Apellido, @Cedula, @Telefono, @Correo, @Direccion, @PasswordHash.
   * **Retorna:** Resultado (1 = Éxito, 0 = Error) y Mensaje (detalles del resultado).
   * **Llamado en:** HomeController.cs (método RegistrarClienteAPI).

2. **dbo.SP_InicioSesionUsuario**
   * **Propósito:** Obtiene la información del usuario por correo (incluyendo rol y hash de contraseña) para autenticar la sesión.
   * **Parámetros:** @Correo.
   * **Llamado en:** HomeController.cs (método IniciarSesionAPI).

---

## 📋 Listado Completo de SPs Requeridos por el Proyecto

A continuación se detallan todos los SPs que se referencian en el código C# (GymManagement_API y Tarea1/GymManagement_WEB):

| # | Stored Procedure | Módulo/Controlador/Repositorio de Origen | Estado original |
|---|------------------|-------------------------------------------|-----------------|
| 1 | sp_Rol_Listar | RolRepository.cs | Definió en SQL |
| 2 | sp_Usuario_ObtenerPorCorreo | UsuarioRepository.cs | Definió en SQL |
| 3 | sp_Usuario_ObtenerPorId | UsuarioRepository.cs | Definió en SQL |
| 4 | sp_Usuario_Listar | UsuarioRepository.cs | Definió en SQL |
| 5 | sp_Usuario_ExisteCorreo | UsuarioRepository.cs | Definió en SQL |
| 6 | sp_Usuario_Crear | UsuarioRepository.cs | Definió en SQL |
| 7 | sp_Usuario_Actualizar | UsuarioRepository.cs | Definió en SQL |
| 8 | sp_Usuario_ActualizarPassword | UsuarioRepository.cs | Definió en SQL |
| 9 | sp_Usuario_Eliminar | UsuarioRepository.cs | Definió en SQL |
| 10 | sp_Recuperacion_Crear | RecuperacionRepository.cs | Definió en SQL |
| 11 | sp_Recuperacion_ObtenerPorToken | RecuperacionRepository.cs | Definió en SQL |
| 12 | sp_Recuperacion_MarcarUtilizado | RecuperacionRepository.cs | Definió en SQL |
| 13 | sp_Cliente_Listar | ClientesController.cs | Definió en SQL |
| 14 | sp_Cliente_ObtenerPorId | ClientesController.cs | Definió en SQL |
| 15 | sp_Cliente_ObtenerPorCedula | ClientesController.cs | Definió en SQL |
| 16 | sp_Cliente_Crear | ClientesController.cs | Definió en SQL |
| 17 | sp_Cliente_Actualizar | ClientesController.cs | Definió en SQL |
| 18 | sp_Cliente_Eliminar | ClientesController.cs | Definió en SQL |
| 19 | sp_Oportunidad_Listar | OportunidadesController.cs | Definió en SQL |
| 20 | sp_Oportunidad_ObtenerPorId | OportunidadesController.cs | Definió en SQL |
| 21 | sp_Oportunidad_Crear | OportunidadesController.cs | Definió en SQL |
| 22 | sp_Oportunidad_Actualizar | OportunidadesController.cs | Definió en SQL |
| 23 | sp_Oportunidad_Eliminar | OportunidadesController.cs | Definió en SQL |
| 24 | sp_Cita_Listar | CitasController.cs | Definió en SQL |
| 25 | sp_Cita_ObtenerPorId | CitasController.cs | Definió en SQL |
| 26 | sp_Cita_Crear | CitasController.cs | Definió en SQL |
| 27 | sp_Cita_Actualizar | CitasController.cs | Definió en SQL |
| 28 | sp_Cita_Eliminar | CitasController.cs | Definió en SQL |
| 29 | sp_Plan_Listar | PlanesController.cs | Definió en SQL |
| 30 | sp_Plan_ObtenerPorId | PlanesController.cs | Definió en SQL |
| 31 | sp_Plan_Crear | PlanesController.cs | Definió en SQL |
| 32 | sp_Plan_Actualizar | PlanesController.cs | Definió en SQL |
| 33 | sp_Plan_Eliminar | PlanesController.cs | Definió en SQL |
| 34 | sp_Membresia_Listar | MembresiasController.cs | Definió en SQL |
| 35 | sp_Membresia_ObtenerPorId | MembresiasController.cs | Definió en SQL |
| 36 | sp_Membresia_Crear | MembresiasController.cs | Definió en SQL |
| 37 | sp_Membresia_Actualizar | MembresiasController.cs | Definió en SQL |
| 38 | sp_Membresia_Eliminar | MembresiasController.cs | Definió en SQL |
| 39 | sp_Pago_Listar | PagosController.cs | Definió en SQL |
| 40 | sp_Pago_ObtenerPorId | PagosController.cs | Definió en SQL |
| 41 | sp_Pago_Crear | PagosController.cs | Definió en SQL |
| 42 | sp_Reserva_ListarTodas | ReservaRepository.cs | Definió en SQL |
| 43 | sp_Reserva_ListarPendientesPorCliente | ReservaRepository.cs | Definió en SQL |
| 44 | sp_Reserva_Crear | ReservaRepository.cs | Definió en SQL |
| 45 | sp_Reserva_MarcarComoPagada | ReservaRepository.cs | Definió en SQL |
| 46 | sp_Rutina_ListarPorCliente | RutinaRepository.cs | Definió en SQL |
| 47 | sp_Rutina_ObtenerPorId | RutinaRepository.cs | Definió en SQL |
| 48 | sp_Rutina_Crear | RutinaRepository.cs | Definió en SQL |
| 49 | sp_Rutina_Eliminar | RutinaRepository.cs | Definió en SQL |
| 50 | sp_DetalleRutina_ListarPorRutina | RutinaRepository.cs | Definió en SQL |
| 51 | sp_DetalleRutina_Crear | RutinaRepository.cs | Definió en SQL |
| 52 | SP_RegistrarUsuario | HomeController.cs | **FALTABA (Agregado)** |
| 53 | SP_InicioSesionUsuario | HomeController.cs | **FALTABA (Agregado)** |
| 54 | sp_LogError_Crear | ManejoExcepcionesMiddleware.cs / LogErrorRepository.cs | Agregado (middleware de errores) |
| 55 | sp_LogError_Listar | LogErrorRepository.cs | Agregado (middleware de errores) |
| 56 | sp_Membresia_DatosRecordatorio | MembresiasController.cs | Agregado (reemplaza SQL en línea) |
| 57 | sp_Cliente_ObtenerPorCorreo | MembresiasController.cs / ReservasController.cs / RutinasController.cs | Agregado (reemplaza SQL en línea) |
| 58 | sp_Usuario_ListarPorRol | ReservasController.cs | Agregado (reemplaza SQL en línea) |
| 59 | sp_Reserva_ListarPorCliente | ReservasController.cs | Agregado (reemplaza SQL en línea) |

---

## Procedimientos eliminados del script

Estos 7 SPs venían de la convención antigua de nombres, pero **ningún controlador ni
repositorio del código vivo los llama**, así que se sacaron del script único:

SP_Registrar_Cliente, SP_Consultar_Clientes, SP_Consultar_Cliente_ID,
SP_Actualizar_Cliente, SP_Eliminar_Cliente, SP_Oportunidades_Listar,
SP_Oportunidades_ObtenerId

Las únicas llamadas que quedaban estaban en la carpeta duplicada
GymManagement/GymManagement/, que no forma parte de la solución (GymManagement.slnx).

## Datos iniciales que incluye el script

* Roles: 1 Administrador, 2 Recepcionista, 3 Entrenador, **4 Cliente**
  (el 4 es obligatorio: SP_RegistrarUsuario inserta con IdRol = 4).
* Usuario administrador: admin@gymmanagement.com / Admin123* (hash BCrypt, workFactor 11).
* Tres planes de membresía de ejemplo.