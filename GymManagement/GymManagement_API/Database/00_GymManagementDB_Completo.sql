/* ============================================================================
   Sistema Web Para La Gestion Integral De Gimnasios - Grupo 8
   Script completo de base de datos: GymManagementDB
   Motor: SQL Server 2019 o superior

   Contenido: 14 tablas, indices, llaves foraneas, 55 procedimientos
              almacenados y los datos iniciales (roles, admin, planes).

   Como usarlo:
     1. Abrir en SSMS conectado a su instancia local.
     2. Ejecutar completo (F5). No hace falta crear la base a mano.
     3. Se puede volver a ejecutar sin borrar nada: todo esta protegido
        con IF NOT EXISTS / CREATE OR ALTER.

   Usuario inicial: admin@gymmanagement.com / Admin123*
   ============================================================================ */
USE [master]
GO
/****** Object:  Database [GymManagementDB]    Script Date: 21/8/2026 14:35:30 ******/
IF DB_ID('GymManagementDB') IS NULL
    CREATE DATABASE [GymManagementDB];
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GymManagementDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GymManagementDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GymManagementDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GymManagementDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GymManagementDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GymManagementDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [GymManagementDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GymManagementDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GymManagementDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GymManagementDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GymManagementDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GymManagementDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GymManagementDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GymManagementDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GymManagementDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GymManagementDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [GymManagementDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GymManagementDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GymManagementDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GymManagementDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GymManagementDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GymManagementDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GymManagementDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GymManagementDB] SET RECOVERY FULL 
GO
ALTER DATABASE [GymManagementDB] SET  MULTI_USER 
GO
ALTER DATABASE [GymManagementDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GymManagementDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GymManagementDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GymManagementDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GymManagementDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GymManagementDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'GymManagementDB', N'ON'
GO
ALTER DATABASE [GymManagementDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [GymManagementDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [GymManagementDB]
GO
/****** Object:  Table [dbo].[Asistencia]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Asistencia]', N'U') IS NULL
CREATE TABLE [dbo].[Asistencia](
	[IdAsistencia] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[FechaHora] [datetime] NOT NULL,
 CONSTRAINT [PK_Asistencia] PRIMARY KEY CLUSTERED 
(
	[IdAsistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Citas]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Citas]', N'U') IS NULL
CREATE TABLE [dbo].[Citas](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Titulo] [varchar](200) NOT NULL,
	[Descripcion] [varchar](max) NULL,
	[Fecha] [date] NOT NULL,
	[HoraInicio] [time](7) NOT NULL,
	[HoraFin] [time](7) NOT NULL,
	[Estado] [varchar](50) NULL,
	[FechaCreacion] [datetime] NULL,
	[FechaActualizacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Clientes]', N'U') IS NULL
CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Apellido] [varchar](100) NOT NULL,
	[Cedula] [varchar](20) NOT NULL,
	[Telefono] [varchar](20) NULL,
	[Correo] [varchar](150) NULL,
	[Direccion] [varchar](250) NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Clientes_Cedula] UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleRutina]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[DetalleRutina]', N'U') IS NULL
CREATE TABLE [dbo].[DetalleRutina](
	[IdDetalle] [int] IDENTITY(1,1) NOT NULL,
	[IdRutina] [int] NOT NULL,
	[DiaSemana] [varchar](20) NOT NULL,
	[Ejercicio] [varchar](100) NOT NULL,
	[Series] [int] NOT NULL,
	[Repeticiones] [varchar](50) NOT NULL,
	[Descanso] [varchar](50) NULL,
	[VideoUrl] [varchar](500) NULL,
 CONSTRAINT [PK_DetalleRutina] PRIMARY KEY CLUSTERED 
(
	[IdDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogErrores]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[LogErrores]', N'U') IS NULL
CREATE TABLE [dbo].[LogErrores](
	[IdError] [int] IDENTITY(1,1) NOT NULL,
	[Mensaje] [varchar](1000) NOT NULL,
	[StackTrace] [varchar](max) NULL,
	[Ruta] [varchar](300) NULL,
	[UsuarioAfectado] [varchar](150) NULL,
	[Fecha] [datetime] NOT NULL,
 CONSTRAINT [PK_LogErrores] PRIMARY KEY CLUSTERED 
(
	[IdError] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MembresiaCliente]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[MembresiaCliente]', N'U') IS NULL
CREATE TABLE [dbo].[MembresiaCliente](
	[IdMembresiaCliente] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdPlan] [int] NOT NULL,
	[FechaInicio] [date] NOT NULL,
	[FechaFin] [date] NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_MembresiaCliente] PRIMARY KEY CLUSTERED 
(
	[IdMembresiaCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Oportunidades]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Oportunidades]', N'U') IS NULL
CREATE TABLE [dbo].[Oportunidades](
	[IdOportunidad] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[Titulo] [varchar](100) NOT NULL,
	[Descripcion] [varchar](500) NULL,
	[MontoEstimado] [decimal](10, 2) NOT NULL,
	[Etapa] [varchar](50) NOT NULL,
	[FechaCierre] [date] NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Oportunidades] PRIMARY KEY CLUSTERED 
(
	[IdOportunidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagos]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Pagos]', N'U') IS NULL
CREATE TABLE [dbo].[Pagos](
	[IdPago] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdMembresiaCliente] [int] NULL,
	[Monto] [decimal](10, 2) NOT NULL,
	[FechaPago] [datetime] NOT NULL,
	[MetodoPago] [varchar](50) NOT NULL,
	[Estado] [varchar](30) NOT NULL,
 CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED 
(
	[IdPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlanesMembresia]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[PlanesMembresia]', N'U') IS NULL
CREATE TABLE [dbo].[PlanesMembresia](
	[IdPlan] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[DuracionDias] [int] NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_PlanesMembresia] PRIMARY KEY CLUSTERED 
(
	[IdPlan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecuperacionPassword]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[RecuperacionPassword]', N'U') IS NULL
CREATE TABLE [dbo].[RecuperacionPassword](
	[IdRecuperacion] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Token] [varchar](255) NOT NULL,
	[FechaExpira] [datetime] NOT NULL,
	[Utilizado] [bit] NOT NULL,
 CONSTRAINT [PK_RecuperacionPassword] PRIMARY KEY CLUSTERED 
(
	[IdRecuperacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReservasEntrenador]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[ReservasEntrenador]', N'U') IS NULL
CREATE TABLE [dbo].[ReservasEntrenador](
	[IdReserva] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdEntrenador] [int] NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Costo] [decimal](10, 2) NOT NULL,
	[Estado] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ReservasEntrenador] PRIMARY KEY CLUSTERED 
(
	[IdReserva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Roles]', N'U') IS NULL
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](200) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Roles_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rutinas]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Rutinas]', N'U') IS NULL
CREATE TABLE [dbo].[Rutinas](
	[IdRutina] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[IdEntrenador] [int] NOT NULL,
	[NombreRutina] [varchar](100) NOT NULL,
	[Descripcion] [varchar](500) NULL,
	[FechaAsignacion] [datetime] NOT NULL,
 CONSTRAINT [PK_Rutinas] PRIMARY KEY CLUSTERED 
(
	[IdRutina] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', N'U') IS NULL
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[IdRol] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Correo] [varchar](150) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Usuarios_Correo] UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Asistencia_FechaHora]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Asistencia_FechaHora' AND object_id = OBJECT_ID(N'[dbo].[Asistencia]'))
CREATE NONCLUSTERED INDEX [IX_Asistencia_FechaHora] ON [dbo].[Asistencia]
(
	[FechaHora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Asistencia_IdCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Asistencia_IdCliente' AND object_id = OBJECT_ID(N'[dbo].[Asistencia]'))
CREATE NONCLUSTERED INDEX [IX_Asistencia_IdCliente] ON [dbo].[Asistencia]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Clientes_Estado]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Clientes_Estado' AND object_id = OBJECT_ID(N'[dbo].[Clientes]'))
CREATE NONCLUSTERED INDEX [IX_Clientes_Estado] ON [dbo].[Clientes]
(
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MembCliente_FechaFin]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MembCliente_FechaFin' AND object_id = OBJECT_ID(N'[dbo].[MembresiaCliente]'))
CREATE NONCLUSTERED INDEX [IX_MembCliente_FechaFin] ON [dbo].[MembresiaCliente]
(
	[FechaFin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MembCliente_IdCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MembCliente_IdCliente' AND object_id = OBJECT_ID(N'[dbo].[MembresiaCliente]'))
CREATE NONCLUSTERED INDEX [IX_MembCliente_IdCliente] ON [dbo].[MembresiaCliente]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MembCliente_IdPlan]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_MembCliente_IdPlan' AND object_id = OBJECT_ID(N'[dbo].[MembresiaCliente]'))
CREATE NONCLUSTERED INDEX [IX_MembCliente_IdPlan] ON [dbo].[MembresiaCliente]
(
	[IdPlan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Oportunidades_Etapa]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Oportunidades_Etapa' AND object_id = OBJECT_ID(N'[dbo].[Oportunidades]'))
CREATE NONCLUSTERED INDEX [IX_Oportunidades_Etapa] ON [dbo].[Oportunidades]
(
	[Etapa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Oportunidades_IdCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Oportunidades_IdCliente' AND object_id = OBJECT_ID(N'[dbo].[Oportunidades]'))
CREATE NONCLUSTERED INDEX [IX_Oportunidades_IdCliente] ON [dbo].[Oportunidades]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pagos_FechaPago]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Pagos_FechaPago' AND object_id = OBJECT_ID(N'[dbo].[Pagos]'))
CREATE NONCLUSTERED INDEX [IX_Pagos_FechaPago] ON [dbo].[Pagos]
(
	[FechaPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pagos_IdCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Pagos_IdCliente' AND object_id = OBJECT_ID(N'[dbo].[Pagos]'))
CREATE NONCLUSTERED INDEX [IX_Pagos_IdCliente] ON [dbo].[Pagos]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pagos_IdMembresiaCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Pagos_IdMembresiaCliente' AND object_id = OBJECT_ID(N'[dbo].[Pagos]'))
CREATE NONCLUSTERED INDEX [IX_Pagos_IdMembresiaCliente] ON [dbo].[Pagos]
(
	[IdMembresiaCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecPass_IdUsuario]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RecPass_IdUsuario' AND object_id = OBJECT_ID(N'[dbo].[RecuperacionPassword]'))
CREATE NONCLUSTERED INDEX [IX_RecPass_IdUsuario] ON [dbo].[RecuperacionPassword]
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RecPass_Token]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RecPass_Token' AND object_id = OBJECT_ID(N'[dbo].[RecuperacionPassword]'))
CREATE NONCLUSTERED INDEX [IX_RecPass_Token] ON [dbo].[RecuperacionPassword]
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Reservas_IdCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Reservas_IdCliente' AND object_id = OBJECT_ID(N'[dbo].[ReservasEntrenador]'))
CREATE NONCLUSTERED INDEX [IX_Reservas_IdCliente] ON [dbo].[ReservasEntrenador]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Rutinas_IdCliente]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Rutinas_IdCliente' AND object_id = OBJECT_ID(N'[dbo].[Rutinas]'))
CREATE NONCLUSTERED INDEX [IX_Rutinas_IdCliente] ON [dbo].[Rutinas]
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Usuarios_IdRol]    Script Date: 21/8/2026 14:35:31 ******/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Usuarios_IdRol' AND object_id = OBJECT_ID(N'[dbo].[Usuarios]'))
CREATE NONCLUSTERED INDEX [IX_Usuarios_IdRol] ON [dbo].[Usuarios]
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF OBJECT_ID(N'[dbo].[DF_Asistencia_FechaHora]') IS NULL
ALTER TABLE [dbo].[Asistencia] ADD  CONSTRAINT [DF_Asistencia_FechaHora]  DEFAULT (getdate()) FOR [FechaHora]
GO
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints d WHERE d.parent_object_id = OBJECT_ID(N'[dbo].[Citas]') AND COL_NAME(d.parent_object_id, d.parent_column_id) = N'Estado')
ALTER TABLE [dbo].[Citas] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
IF NOT EXISTS (SELECT 1 FROM sys.default_constraints d WHERE d.parent_object_id = OBJECT_ID(N'[dbo].[Citas]') AND COL_NAME(d.parent_object_id, d.parent_column_id) = N'FechaCreacion')
ALTER TABLE [dbo].[Citas] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
IF OBJECT_ID(N'[dbo].[DF_Clientes_FechaRegistro]') IS NULL
ALTER TABLE [dbo].[Clientes] ADD  CONSTRAINT [DF_Clientes_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
IF OBJECT_ID(N'[dbo].[DF_Clientes_Estado]') IS NULL
ALTER TABLE [dbo].[Clientes] ADD  CONSTRAINT [DF_Clientes_Estado]  DEFAULT ((1)) FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_LogErrores_Fecha]') IS NULL
ALTER TABLE [dbo].[LogErrores] ADD  CONSTRAINT [DF_LogErrores_Fecha]  DEFAULT (getdate()) FOR [Fecha]
GO
IF OBJECT_ID(N'[dbo].[DF_MembCliente_Estado]') IS NULL
ALTER TABLE [dbo].[MembresiaCliente] ADD  CONSTRAINT [DF_MembCliente_Estado]  DEFAULT ((1)) FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_Oportunidades_FechaRegistro]') IS NULL
ALTER TABLE [dbo].[Oportunidades] ADD  CONSTRAINT [DF_Oportunidades_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
IF OBJECT_ID(N'[dbo].[DF_Oportunidades_Estado]') IS NULL
ALTER TABLE [dbo].[Oportunidades] ADD  CONSTRAINT [DF_Oportunidades_Estado]  DEFAULT ((1)) FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_Pagos_FechaPago]') IS NULL
ALTER TABLE [dbo].[Pagos] ADD  CONSTRAINT [DF_Pagos_FechaPago]  DEFAULT (getdate()) FOR [FechaPago]
GO
IF OBJECT_ID(N'[dbo].[DF_Pagos_Estado]') IS NULL
ALTER TABLE [dbo].[Pagos] ADD  CONSTRAINT [DF_Pagos_Estado]  DEFAULT ('Pagado') FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_Planes_Estado]') IS NULL
ALTER TABLE [dbo].[PlanesMembresia] ADD  CONSTRAINT [DF_Planes_Estado]  DEFAULT ((1)) FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_RecPass_Utilizado]') IS NULL
ALTER TABLE [dbo].[RecuperacionPassword] ADD  CONSTRAINT [DF_RecPass_Utilizado]  DEFAULT ((0)) FOR [Utilizado]
GO
IF OBJECT_ID(N'[dbo].[DF_Reservas_Estado]') IS NULL
ALTER TABLE [dbo].[ReservasEntrenador] ADD  CONSTRAINT [DF_Reservas_Estado]  DEFAULT ('Pendiente') FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_Rutinas_FechaAsignacion]') IS NULL
ALTER TABLE [dbo].[Rutinas] ADD  CONSTRAINT [DF_Rutinas_FechaAsignacion]  DEFAULT (getdate()) FOR [FechaAsignacion]
GO
IF OBJECT_ID(N'[dbo].[DF_Usuarios_Estado]') IS NULL
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Estado]  DEFAULT ((1)) FOR [Estado]
GO
IF OBJECT_ID(N'[dbo].[DF_Usuarios_FechaRegistro]') IS NULL
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
IF OBJECT_ID(N'[dbo].[FK_Asistencia_Clientes]') IS NULL
ALTER TABLE [dbo].[Asistencia]  WITH CHECK ADD  CONSTRAINT [FK_Asistencia_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Asistencia] CHECK CONSTRAINT [FK_Asistencia_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_Citas_Clientes]') IS NULL
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_DetalleRutina_Rutinas]') IS NULL
ALTER TABLE [dbo].[DetalleRutina]  WITH CHECK ADD  CONSTRAINT [FK_DetalleRutina_Rutinas] FOREIGN KEY([IdRutina])
REFERENCES [dbo].[Rutinas] ([IdRutina])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetalleRutina] CHECK CONSTRAINT [FK_DetalleRutina_Rutinas]
GO
IF OBJECT_ID(N'[dbo].[FK_MembCliente_Clientes]') IS NULL
ALTER TABLE [dbo].[MembresiaCliente]  WITH CHECK ADD  CONSTRAINT [FK_MembCliente_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[MembresiaCliente] CHECK CONSTRAINT [FK_MembCliente_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_MembCliente_Planes]') IS NULL
ALTER TABLE [dbo].[MembresiaCliente]  WITH CHECK ADD  CONSTRAINT [FK_MembCliente_Planes] FOREIGN KEY([IdPlan])
REFERENCES [dbo].[PlanesMembresia] ([IdPlan])
GO
ALTER TABLE [dbo].[MembresiaCliente] CHECK CONSTRAINT [FK_MembCliente_Planes]
GO
IF OBJECT_ID(N'[dbo].[FK_Oportunidades_Clientes]') IS NULL
ALTER TABLE [dbo].[Oportunidades]  WITH CHECK ADD  CONSTRAINT [FK_Oportunidades_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Oportunidades] CHECK CONSTRAINT [FK_Oportunidades_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_Pagos_Clientes]') IS NULL
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [FK_Pagos_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [FK_Pagos_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_Pagos_MembresiaCliente]') IS NULL
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [FK_Pagos_MembresiaCliente] FOREIGN KEY([IdMembresiaCliente])
REFERENCES [dbo].[MembresiaCliente] ([IdMembresiaCliente])
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [FK_Pagos_MembresiaCliente]
GO
IF OBJECT_ID(N'[dbo].[FK_RecPass_Usuarios]') IS NULL
ALTER TABLE [dbo].[RecuperacionPassword]  WITH CHECK ADD  CONSTRAINT [FK_RecPass_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[RecuperacionPassword] CHECK CONSTRAINT [FK_RecPass_Usuarios]
GO
IF OBJECT_ID(N'[dbo].[FK_Reservas_Clientes]') IS NULL
ALTER TABLE [dbo].[ReservasEntrenador]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[ReservasEntrenador] CHECK CONSTRAINT [FK_Reservas_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_Reservas_Usuarios]') IS NULL
ALTER TABLE [dbo].[ReservasEntrenador]  WITH CHECK ADD  CONSTRAINT [FK_Reservas_Usuarios] FOREIGN KEY([IdEntrenador])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[ReservasEntrenador] CHECK CONSTRAINT [FK_Reservas_Usuarios]
GO
IF OBJECT_ID(N'[dbo].[FK_Rutinas_Clientes]') IS NULL
ALTER TABLE [dbo].[Rutinas]  WITH CHECK ADD  CONSTRAINT [FK_Rutinas_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Rutinas] CHECK CONSTRAINT [FK_Rutinas_Clientes]
GO
IF OBJECT_ID(N'[dbo].[FK_Rutinas_Usuarios]') IS NULL
ALTER TABLE [dbo].[Rutinas]  WITH CHECK ADD  CONSTRAINT [FK_Rutinas_Usuarios] FOREIGN KEY([IdEntrenador])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Rutinas] CHECK CONSTRAINT [FK_Rutinas_Usuarios]
GO
IF OBJECT_ID(N'[dbo].[FK_Usuarios_Roles]') IS NULL
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Roles] ([IdRol])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Roles]
GO
IF OBJECT_ID(N'[dbo].[CK_MembCliente_Fechas]') IS NULL
ALTER TABLE [dbo].[MembresiaCliente]  WITH CHECK ADD  CONSTRAINT [CK_MembCliente_Fechas] CHECK  (([FechaFin]>=[FechaInicio]))
GO
ALTER TABLE [dbo].[MembresiaCliente] CHECK CONSTRAINT [CK_MembCliente_Fechas]
GO
IF OBJECT_ID(N'[dbo].[CK_Oportunidades_MontoEstimado]') IS NULL
ALTER TABLE [dbo].[Oportunidades]  WITH CHECK ADD  CONSTRAINT [CK_Oportunidades_MontoEstimado] CHECK  (([MontoEstimado]>=(0)))
GO
ALTER TABLE [dbo].[Oportunidades] CHECK CONSTRAINT [CK_Oportunidades_MontoEstimado]
GO
IF OBJECT_ID(N'[dbo].[CK_Pagos_Monto]') IS NULL
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [CK_Pagos_Monto] CHECK  (([Monto]>=(0)))
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [CK_Pagos_Monto]
GO
IF OBJECT_ID(N'[dbo].[CK_Planes_Duracion]') IS NULL
ALTER TABLE [dbo].[PlanesMembresia]  WITH CHECK ADD  CONSTRAINT [CK_Planes_Duracion] CHECK  (([DuracionDias]>(0)))
GO
ALTER TABLE [dbo].[PlanesMembresia] CHECK CONSTRAINT [CK_Planes_Duracion]
GO
IF OBJECT_ID(N'[dbo].[CK_Planes_Precio]') IS NULL
ALTER TABLE [dbo].[PlanesMembresia]  WITH CHECK ADD  CONSTRAINT [CK_Planes_Precio] CHECK  (([Precio]>=(0)))
GO
ALTER TABLE [dbo].[PlanesMembresia] CHECK CONSTRAINT [CK_Planes_Precio]
GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar cita */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cita_Actualizar]
    @IdCita INT,
    @IdCliente INT,
    @Titulo VARCHAR(200),
    @Descripcion VARCHAR(MAX) = NULL,
    @Fecha DATE,
    @HoraInicio TIME,
    @HoraFin TIME,
    @Estado VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Citas
        SET 
            IdCliente = @IdCliente,
            Titulo = @Titulo,
            Descripcion = @Descripcion,
            Fecha = @Fecha,
            HoraInicio = @HoraInicio,
            HoraFin = @HoraFin,
            Estado = @Estado,
            FechaActualizacion = GETDATE()
        WHERE IdCita = @IdCita;

        SELECT @@ROWCOUNT AS Afectados;
    END TRY
    BEGIN CATCH
        SELECT 0 AS Afectados;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear cita -> devuelve el Id generado */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cita_Crear]
    @IdCliente INT,
    @IdUsuario INT,
    @Titulo VARCHAR(200),
    @Descripcion VARCHAR(MAX) = NULL,
    @Fecha DATE,
    @HoraInicio TIME,
    @HoraFin TIME
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.Citas (IdCliente, IdUsuario, Titulo, Descripcion, Fecha, HoraInicio, HoraFin, Estado, FechaCreacion)
        VALUES (@IdCliente, @IdUsuario, @Titulo, @Descripcion, @Fecha, @HoraInicio, @HoraFin, 'Pendiente', GETDATE());

        SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdCita;
    END TRY
    BEGIN CATCH
        SELECT 0 AS IdCita;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Eliminar cita */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cita_Eliminar]
    @IdCita INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.Citas
        WHERE IdCita = @IdCita;

        SELECT @@ROWCOUNT AS Afectados;
    END TRY
    BEGIN CATCH
        SELECT 0 AS Afectados;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Listar todas las citas con información del cliente y usuario */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cita_Listar]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        c.IdCita,
        c.IdCliente,
        uc.Nombre AS Cliente,
        c.IdUsuario,
        uu.Nombre AS Usuario,
        c.Titulo,
        c.Descripcion,
        c.Fecha,
        c.HoraInicio,
        c.HoraFin,
        c.Estado,
        c.FechaCreacion,
        c.FechaActualizacion
    FROM dbo.Citas c
    INNER JOIN dbo.Usuarios uc ON c.IdCliente = uc.IdUsuario
    INNER JOIN dbo.Usuarios uu ON c.IdUsuario = uu.IdUsuario
    ORDER BY c.Fecha DESC, c.HoraInicio;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cita_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener cita por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cita_ObtenerPorId]
    @IdCita INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        c.IdCita,
        c.IdCliente,
        uc.Nombre AS Cliente,
        c.IdUsuario,
        uu.Nombre AS Usuario,
        c.Titulo,
        c.Descripcion,
        c.Fecha,
        c.HoraInicio,
        c.HoraFin,
        c.Estado,
        c.FechaCreacion,
        c.FechaActualizacion
    FROM dbo.Citas c
    INNER JOIN dbo.Usuarios uc ON c.IdCliente = uc.IdUsuario
    INNER JOIN dbo.Usuarios uu ON c.IdUsuario = uu.IdUsuario
    WHERE c.IdCita = @IdCita;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar cliente */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_Actualizar]
	@IdCliente INT,
	@Nombre    VARCHAR(100),
	@Apellido  VARCHAR(100),
	@Cedula    VARCHAR(20),
	@Telefono  VARCHAR(20) = NULL,
	@Correo    VARCHAR(150) = NULL,
	@Direccion VARCHAR(250) = NULL,
	@Estado    BIT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Clientes
	SET Nombre    = @Nombre,
		Apellido  = @Apellido,
		Cedula    = @Cedula,
		Telefono  = @Telefono,
		Correo    = @Correo,
		Direccion = @Direccion,
		Estado    = @Estado
	WHERE IdCliente = @IdCliente;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear cliente -> devuelve el Id generado */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_Crear]
	@Nombre    VARCHAR(100),
	@Apellido  VARCHAR(100),
	@Cedula    VARCHAR(20),
	@Telefono  VARCHAR(20) = NULL,
	@Correo    VARCHAR(150) = NULL,
	@Direccion VARCHAR(250) = NULL,
	@Estado    BIT = 1
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Clientes (Nombre, Apellido, Cedula, Telefono, Correo, Direccion, Estado)
	VALUES (@Nombre, @Apellido, @Cedula, @Telefono, @Correo, @Direccion, @Estado);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdCliente;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_Eliminar]
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	
	-- Eliminar dependencias
	DELETE FROM dbo.Asistencia WHERE IdCliente = @IdCliente;
	DELETE FROM dbo.Citas WHERE IdCliente = @IdCliente;
	DELETE FROM dbo.MembresiaCliente WHERE IdCliente = @IdCliente;
	DELETE FROM dbo.Oportunidades WHERE IdCliente = @IdCliente;
	
	-- Eliminar rutinas asociadas y sus detalles
	DELETE FROM dbo.DetalleRutina WHERE IdRutina IN (SELECT IdRutina FROM dbo.Rutinas WHERE IdCliente = @IdCliente);
	DELETE FROM dbo.Rutinas WHERE IdCliente = @IdCliente;
	
	-- Eliminar de Usuarios si existe
	DECLARE @Correo VARCHAR(150);
	SELECT @Correo = Correo FROM dbo.Clientes WHERE IdCliente = @IdCliente;
	IF @Correo IS NOT NULL
	BEGIN
		DELETE FROM dbo.Usuarios WHERE Correo = @Correo;
	END

	DELETE FROM dbo.Clientes
	WHERE IdCliente = @IdCliente;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ==========================  CLIENTES  ===================================== */

/* Listar clientes con búsqueda y filtros */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_Listar]
	@Buscar VARCHAR(100) = NULL,
	@Estado BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdCliente, Nombre, Apellido, Cedula, Telefono, Correo, Direccion, FechaRegistro, Estado
	FROM dbo.Clientes
	WHERE (@Estado IS NULL OR Estado = @Estado)
	  AND (@Buscar IS NULL 
		   OR Nombre LIKE '%' + @Buscar + '%' 
		   OR Apellido LIKE '%' + @Buscar + '%' 
		   OR Cedula LIKE '%' + @Buscar + '%' 
		   OR Correo LIKE '%' + @Buscar + '%')
	ORDER BY Nombre, Apellido;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_ObtenerPorCedula]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener cliente por Cédula (para validaciones de unicidad) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_ObtenerPorCedula]
	@Cedula VARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdCliente, Nombre, Apellido, Cedula, Telefono, Correo, Direccion, FechaRegistro, Estado
	FROM dbo.Clientes
	WHERE Cedula = @Cedula;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener cliente por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_ObtenerPorId]
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdCliente, Nombre, Apellido, Cedula, Telefono, Correo, Direccion, FechaRegistro, Estado
	FROM dbo.Clientes
	WHERE IdCliente = @IdCliente;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DetalleRutina_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear detalle de rutina */
CREATE OR ALTER PROCEDURE [dbo].[sp_DetalleRutina_Crear]
	@IdRutina INT,
	@DiaSemana VARCHAR(20),
	@Ejercicio VARCHAR(100),
	@Series INT,
	@Repeticiones VARCHAR(50),
	@Descanso VARCHAR(50) = NULL,
	@VideoUrl VARCHAR(500) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.DetalleRutina (IdRutina, DiaSemana, Ejercicio, Series, Repeticiones, Descanso, VideoUrl)
	VALUES (@IdRutina, @DiaSemana, @Ejercicio, @Series, @Repeticiones, @Descanso, @VideoUrl);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdDetalle;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DetalleRutina_EliminarPorRutina]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_DetalleRutina_EliminarPorRutina]
	@IdRutina INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.DetalleRutina
	WHERE IdRutina = @IdRutina;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DetalleRutina_ListarPorRutina]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Listar detalle de una rutina */
CREATE OR ALTER PROCEDURE [dbo].[sp_DetalleRutina_ListarPorRutina]
	@IdRutina INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdDetalle, IdRutina, DiaSemana, Ejercicio, Series, Repeticiones, Descanso, VideoUrl
	FROM dbo.DetalleRutina
	WHERE IdRutina = @IdRutina
	ORDER BY 
		CASE DiaSemana 
			WHEN 'Lunes' THEN 1
			WHEN 'Martes' THEN 2
			WHEN 'Miércoles' THEN 3
			WHEN 'Jueves' THEN 4
			WHEN 'Viernes' THEN 5
			WHEN 'Sábado' THEN 6
			WHEN 'Domingo' THEN 7
			ELSE 8 
		END;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InicioSesionUsuario]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

-- Crear el procedimiento almacenado
CREATE OR ALTER PROCEDURE [dbo].[SP_InicioSesionUsuario]
	@Correo VARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		u.IdUsuario,
		u.IdRol,
		u.Nombre,
		u.Correo,
		u.PasswordHash,
		r.Nombre AS Rol,
		'' AS Token
	FROM dbo.Usuarios u
	INNER JOIN dbo.Roles r ON r.IdRol = u.IdRol
	WHERE u.Correo = @Correo
	  AND u.Estado = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Membresia_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar membresía */
CREATE OR ALTER PROCEDURE [dbo].[sp_Membresia_Actualizar]
	@IdMembresiaCliente INT,
	@IdCliente INT,
	@IdPlan INT,
	@FechaInicio DATE,
	@FechaFin DATE,
	@Estado BIT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.MembresiaCliente
	SET IdCliente = @IdCliente,
		IdPlan = @IdPlan,
		FechaInicio = @FechaInicio,
		FechaFin = @FechaFin,
		Estado = @Estado
	WHERE IdMembresiaCliente = @IdMembresiaCliente;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Membresia_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear membresía */
CREATE OR ALTER PROCEDURE [dbo].[sp_Membresia_Crear]
	@IdCliente INT,
	@IdPlan INT,
	@FechaInicio DATE,
	@FechaFin DATE,
	@Estado BIT = 1
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.MembresiaCliente (IdCliente, IdPlan, FechaInicio, FechaFin, Estado)
	VALUES (@IdCliente, @IdPlan, @FechaInicio, @FechaFin, @Estado);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdMembresiaCliente;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Membresia_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Eliminar membresía */
CREATE OR ALTER PROCEDURE [dbo].[sp_Membresia_Eliminar]
	@IdMembresiaCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.MembresiaCliente
	WHERE IdMembresiaCliente = @IdMembresiaCliente;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Membresia_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ==========================  MEMBRESÍAS DE CLIENTES  ======================== */

/* Listar todas las membresías */
CREATE OR ALTER PROCEDURE [dbo].[sp_Membresia_Listar]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT mc.IdMembresiaCliente, mc.IdCliente, c.Nombre + ' ' + c.Apellido AS ClienteNombre,
		   mc.IdPlan, p.Nombre AS PlanNombre, mc.FechaInicio, mc.FechaFin, mc.Estado
	FROM dbo.MembresiaCliente mc
	INNER JOIN dbo.Clientes c ON c.IdCliente = mc.IdCliente
	INNER JOIN dbo.PlanesMembresia p ON p.IdPlan = mc.IdPlan
	ORDER BY mc.FechaInicio DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Membresia_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener membresía por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Membresia_ObtenerPorId]
	@IdMembresiaCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT mc.IdMembresiaCliente, mc.IdCliente, c.Nombre + ' ' + c.Apellido AS ClienteNombre,
		   mc.IdPlan, p.Nombre AS PlanNombre, mc.FechaInicio, mc.FechaFin, mc.Estado
	FROM dbo.MembresiaCliente mc
	INNER JOIN dbo.Clientes c ON c.IdCliente = mc.IdCliente
	INNER JOIN dbo.PlanesMembresia p ON p.IdPlan = mc.IdPlan
	WHERE mc.IdMembresiaCliente = @IdMembresiaCliente;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Oportunidad_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar oportunidad */
CREATE OR ALTER PROCEDURE [dbo].[SP_Oportunidad_Actualizar]
	@IdOportunidad INT,
	@IdCliente     INT,
	@Titulo        VARCHAR(100),
	@Descripcion   VARCHAR(500) = NULL,
	@MontoEstimado DECIMAL(10,2),
	@Etapa         VARCHAR(50),
	@FechaCierre   DATE = NULL,
	@Estado        BIT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Oportunidades
	SET IdCliente     = @IdCliente,
		Titulo        = @Titulo,
		Descripcion   = @Descripcion,
		MontoEstimado = @MontoEstimado,
		Etapa         = @Etapa,
		FechaCierre   = @FechaCierre,
		Estado        = @Estado
	WHERE IdOportunidad = @IdOportunidad;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Oportunidad_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear oportunidad -> devuelve el Id generado */
CREATE OR ALTER PROCEDURE [dbo].[SP_Oportunidad_Crear]
	@IdCliente     INT,
	@Titulo        VARCHAR(100),
	@Descripcion   VARCHAR(500) = NULL,
	@MontoEstimado DECIMAL(10,2),
	@Etapa         VARCHAR(50),
	@FechaCierre   DATE = NULL,
	@Estado        BIT = 1
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Oportunidades (IdCliente, Titulo, Descripcion, MontoEstimado, Etapa, FechaCierre, Estado)
	VALUES (@IdCliente, @Titulo, @Descripcion, @MontoEstimado, @Etapa, @FechaCierre, @Estado);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdOportunidad;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Oportunidad_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Eliminar oportunidad */
CREATE OR ALTER PROCEDURE [dbo].[SP_Oportunidad_Eliminar]
	@IdOportunidad INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.Oportunidades
	WHERE IdOportunidad = @IdOportunidad;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Oportunidad_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


/* ==========================  OPORTUNIDADES  ================================ */

/* Listar oportunidades con búsqueda y filtros */
CREATE OR ALTER PROCEDURE [dbo].[sp_Oportunidad_Listar]
	@Buscar    VARCHAR(100) = NULL,
	@Etapa     VARCHAR(50) = NULL,
	@IdCliente INT = NULL,
	@Estado    BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT o.IdOportunidad, o.IdCliente, o.Titulo, o.Descripcion, o.MontoEstimado, 
		   o.Etapa, o.FechaCierre, o.FechaRegistro, o.Estado,
		   (c.Nombre + ' ' + c.Apellido) AS ClienteNombreCompleto
	FROM dbo.Oportunidades o
	INNER JOIN dbo.Clientes c ON o.IdCliente = c.IdCliente
	WHERE (@Estado IS NULL OR o.Estado = @Estado)
	  AND (@IdCliente IS NULL OR o.IdCliente = @IdCliente)
	  AND (@Etapa IS NULL OR o.Etapa = @Etapa)
	  AND (@Buscar IS NULL 
		   OR o.Titulo LIKE '%' + @Buscar + '%' 
		   OR o.Descripcion LIKE '%' + @Buscar + '%')
	ORDER BY o.FechaRegistro DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Oportunidad_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener oportunidad por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Oportunidad_ObtenerPorId]
	@IdOportunidad INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT o.IdOportunidad, o.IdCliente, o.Titulo, o.Descripcion, o.MontoEstimado, 
		   o.Etapa, o.FechaCierre, o.FechaRegistro, o.Estado,
		   (c.Nombre + ' ' + c.Apellido) AS ClienteNombreCompleto
	FROM dbo.Oportunidades o
	INNER JOIN dbo.Clientes c ON o.IdCliente = c.IdCliente
	WHERE o.IdOportunidad = @IdOportunidad;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Pago_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear pago */
CREATE OR ALTER PROCEDURE [dbo].[sp_Pago_Crear]
	@IdCliente INT,
	@IdMembresiaCliente INT = NULL,
	@Monto DECIMAL(10,2),
	@MetodoPago VARCHAR(50),
	@Estado VARCHAR(30) = 'Pagado'
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Pagos (IdCliente, IdMembresiaCliente, Monto, MetodoPago, Estado)
	VALUES (@IdCliente, @IdMembresiaCliente, @Monto, @MetodoPago, @Estado);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdPago;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Pago_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ==========================  PAGOS  ========================================= */

/* Listar todos los pagos */
CREATE OR ALTER PROCEDURE [dbo].[sp_Pago_Listar]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT pg.IdPago, pg.IdCliente, c.Nombre + ' ' + c.Apellido AS ClienteNombre,
		   pg.IdMembresiaCliente, p.Nombre AS PlanNombre, pg.Monto, pg.FechaPago, pg.MetodoPago, pg.Estado
	FROM dbo.Pagos pg
	INNER JOIN dbo.Clientes c ON c.IdCliente = pg.IdCliente
	LEFT JOIN dbo.MembresiaCliente mc ON mc.IdMembresiaCliente = pg.IdMembresiaCliente
	LEFT JOIN dbo.PlanesMembresia p ON p.IdPlan = mc.IdPlan
	ORDER BY pg.FechaPago DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Pago_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener pago por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Pago_ObtenerPorId]
	@IdPago INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT pg.IdPago, pg.IdCliente, c.Nombre + ' ' + c.Apellido AS ClienteNombre,
		   pg.IdMembresiaCliente, p.Nombre AS PlanNombre, pg.Monto, pg.FechaPago, pg.MetodoPago, pg.Estado
	FROM dbo.Pagos pg
	INNER JOIN dbo.Clientes c ON c.IdCliente = pg.IdCliente
	LEFT JOIN dbo.MembresiaCliente mc ON mc.IdMembresiaCliente = pg.IdMembresiaCliente
	LEFT JOIN dbo.PlanesMembresia p ON p.IdPlan = mc.IdPlan
	WHERE pg.IdPago = @IdPago;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Plan_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar plan */
CREATE OR ALTER PROCEDURE [dbo].[sp_Plan_Actualizar]
	@IdPlan INT,
	@Nombre VARCHAR(100),
	@Descripcion VARCHAR(250) = NULL,
	@DuracionDias INT,
	@Precio DECIMAL(10,2),
	@Estado BIT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.PlanesMembresia
	SET Nombre = @Nombre,
		Descripcion = @Descripcion,
		DuracionDias = @DuracionDias,
		Precio = @Precio,
		Estado = @Estado
	WHERE IdPlan = @IdPlan;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Plan_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear plan */
CREATE OR ALTER PROCEDURE [dbo].[sp_Plan_Crear]
	@Nombre VARCHAR(100),
	@Descripcion VARCHAR(250) = NULL,
	@DuracionDias INT,
	@Precio DECIMAL(10,2),
	@Estado BIT = 1
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.PlanesMembresia (Nombre, Descripcion, DuracionDias, Precio, Estado)
	VALUES (@Nombre, @Descripcion, @DuracionDias, @Precio, @Estado);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdPlan;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Plan_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Eliminar plan */
CREATE OR ALTER PROCEDURE [dbo].[sp_Plan_Eliminar]
	@IdPlan INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.PlanesMembresia
	WHERE IdPlan = @IdPlan;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Plan_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



/* ==========================  PLANES DE MEMBRESIA  ========================== */

/* Listar todos los planes */
CREATE OR ALTER PROCEDURE [dbo].[sp_Plan_Listar]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdPlan, Nombre, Descripcion, DuracionDias, Precio, Estado
	FROM dbo.PlanesMembresia
	ORDER BY Nombre;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Plan_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener plan por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Plan_ObtenerPorId]
	@IdPlan INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdPlan, Nombre, Descripcion, DuracionDias, Precio, Estado
	FROM dbo.PlanesMembresia
	WHERE IdPlan = @IdPlan;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Recuperacion_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ====================  RECUPERACIÓN DE CONTRASEÑA  ======================== */

/* Crear token de recuperación */
CREATE OR ALTER PROCEDURE [dbo].[sp_Recuperacion_Crear]
	@IdUsuario   INT,
	@Token       VARCHAR(255),
	@FechaExpira DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.RecuperacionPassword (IdUsuario, Token, FechaExpira, Utilizado)
	VALUES (@IdUsuario, @Token, @FechaExpira, 0);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdRecuperacion;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Recuperacion_MarcarUtilizado]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Marcar token como utilizado */
CREATE OR ALTER PROCEDURE [dbo].[sp_Recuperacion_MarcarUtilizado]
	@IdRecuperacion INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.RecuperacionPassword
	SET Utilizado = 1
	WHERE IdRecuperacion = @IdRecuperacion;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Recuperacion_ObtenerPorToken]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener token válido (no usado y no expirado) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Recuperacion_ObtenerPorToken]
	@Token VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdRecuperacion, IdUsuario, Token, FechaExpira, Utilizado
	FROM dbo.RecuperacionPassword
	WHERE Token = @Token;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RegistrarUsuario]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_RegistrarUsuario]
	@Nombre VARCHAR(100),
	@Apellido VARCHAR(100),
	@Cedula VARCHAR(20),
	@Telefono VARCHAR(20),
	@Correo VARCHAR(150),
	@Direccion VARCHAR(255),
	@PasswordHash VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		-- Verificar si el correo ya existe
		IF EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Correo = @Correo)
		BEGIN
			SELECT 0 AS Resultado, 'El correo ya está registrado' AS Mensaje;
			RETURN;
		END

		-- Verificar si la cédula ya existe en Clientes
		IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Cedula = @Cedula)
		BEGIN
			SELECT 0 AS Resultado, 'La cédula ya está registrada' AS Mensaje;
			RETURN;
		END

		-- Insertar nuevo cliente
		INSERT INTO dbo.Clientes (Nombre, Apellido, Cedula, Telefono, Correo, Direccion, Estado)
		VALUES (@Nombre, @Apellido, @Cedula, @Telefono, @Correo, @Direccion, 1);

		-- Insertar nuevo usuario con rol de cliente (IdRol = 4)
		INSERT INTO dbo.Usuarios (IdRol, Nombre, Correo, PasswordHash, Estado)
		VALUES (4, @Nombre + ' ' + @Apellido, @Correo, @PasswordHash, 1);

		SELECT 1 AS Resultado, 'Usuario registrado exitosamente' AS Mensaje;
	END TRY
	BEGIN CATCH
		SELECT 0 AS Resultado, ERROR_MESSAGE() AS Mensaje;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Reserva_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


/* ====================  RESERVAS DE ENTRENADORES (CLIENTES)  ================ */

/* Crear reserva de entrenador */
CREATE OR ALTER PROCEDURE [dbo].[sp_Reserva_Crear]
	@IdCliente INT,
	@IdEntrenador INT,
	@FechaHora DATETIME,
	@Costo DECIMAL(10,2)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.ReservasEntrenador (IdCliente, IdEntrenador, FechaHora, Costo, Estado)
	VALUES (@IdCliente, @IdEntrenador, @FechaHora, @Costo, 'Pendiente');

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdReserva;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Reserva_ListarPendientesPorCliente]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Listar reservas pendientes de un cliente */
CREATE OR ALTER PROCEDURE [dbo].[sp_Reserva_ListarPendientesPorCliente]
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.IdReserva, r.IdCliente, r.IdEntrenador, e.Nombre AS EntrenadorNombre, r.FechaHora, r.Costo, r.Estado
	FROM dbo.ReservasEntrenador r
	INNER JOIN dbo.Usuarios e ON e.IdUsuario = r.IdEntrenador
	WHERE r.IdCliente = @IdCliente AND r.Estado = 'Pendiente'
	ORDER BY r.FechaHora ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Reserva_ListarTodas]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Listar todas las reservas (con nombres) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Reserva_ListarTodas]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.IdReserva, r.IdCliente, c.Nombre + ' ' + c.Apellido AS ClienteNombre, r.IdEntrenador, e.Nombre AS EntrenadorNombre, r.FechaHora, r.Costo, r.Estado
	FROM dbo.ReservasEntrenador r
	INNER JOIN dbo.Clientes c ON c.IdCliente = r.IdCliente
	INNER JOIN dbo.Usuarios e ON e.IdUsuario = r.IdEntrenador
	ORDER BY r.FechaHora DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Reserva_MarcarComoPagada]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Marcar reserva como pagada */
CREATE OR ALTER PROCEDURE [dbo].[sp_Reserva_MarcarComoPagada]
	@IdReserva INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.ReservasEntrenador
	SET Estado = 'Pagado'
	WHERE IdReserva = @IdReserva;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rol_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ==========================  ROLES  ======================================= */
CREATE OR ALTER PROCEDURE [dbo].[sp_Rol_Listar]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdRol, Nombre, Descripcion
	FROM dbo.Roles
	ORDER BY Nombre;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rutina_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_Rutina_Actualizar]
	@IdRutina INT,
	@NombreRutina VARCHAR(100),
	@Descripcion VARCHAR(500) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Rutinas
	SET NombreRutina = @NombreRutina,
		Descripcion = @Descripcion
	WHERE IdRutina = @IdRutina;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rutina_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ====================  RUTINAS (ENTRENADORES Y CLIENTES)  ================== */

/* Crear una nueva rutina */
CREATE OR ALTER PROCEDURE [dbo].[sp_Rutina_Crear]
	@IdCliente INT,
	@IdEntrenador INT,
	@NombreRutina VARCHAR(100),
	@Descripcion VARCHAR(500) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Rutinas (IdCliente, IdEntrenador, NombreRutina, Descripcion)
	VALUES (@IdCliente, @IdEntrenador, @NombreRutina, @Descripcion);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdRutina;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rutina_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Eliminar rutina completa (detalles se eliminan en cascada por FK) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Rutina_Eliminar]
	@IdRutina INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM dbo.Rutinas WHERE IdRutina = @IdRutina;
	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rutina_ListarPorCliente]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Listar rutinas de un cliente */
CREATE OR ALTER PROCEDURE [dbo].[sp_Rutina_ListarPorCliente]
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.IdRutina, r.IdCliente, r.IdEntrenador, e.Nombre AS EntrenadorNombre, r.NombreRutina, r.Descripcion, r.FechaAsignacion
	FROM dbo.Rutinas r
	INNER JOIN dbo.Usuarios e ON e.IdUsuario = r.IdEntrenador
	WHERE r.IdCliente = @IdCliente
	ORDER BY r.FechaAsignacion DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Rutina_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener rutina por Id */
CREATE OR ALTER PROCEDURE [dbo].[sp_Rutina_ObtenerPorId]
	@IdRutina INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.IdRutina, r.IdCliente, c.Nombre + ' ' + c.Apellido AS ClienteNombre, r.IdEntrenador, e.Nombre AS EntrenadorNombre, r.NombreRutina, r.Descripcion, r.FechaAsignacion
	FROM dbo.Rutinas r
	INNER JOIN dbo.Clientes c ON c.IdCliente = r.IdCliente
	INNER JOIN dbo.Usuarios e ON e.IdUsuario = r.IdEntrenador
	WHERE r.IdRutina = @IdRutina;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Actualizar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar datos del usuario (sin contraseña) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_Actualizar]
	@IdUsuario INT,
	@IdRol     INT,
	@Nombre    VARCHAR(100),
	@Correo    VARCHAR(150),
	@Estado    BIT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Usuarios
	SET IdRol  = @IdRol,
		Nombre = @Nombre,
		Correo = @Correo,
		Estado = @Estado
	WHERE IdUsuario = @IdUsuario;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ActualizarPassword]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Actualizar contraseña (para reseteo / cambio) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_ActualizarPassword]
	@IdUsuario    INT,
	@PasswordHash VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE dbo.Usuarios
	SET PasswordHash = @PasswordHash
	WHERE IdUsuario = @IdUsuario;

	SELECT @@ROWCOUNT AS Afectados;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Crear]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Crear usuario -> devuelve el Id generado */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_Crear]
	@IdRol        INT,
	@Nombre       VARCHAR(100),
	@Correo       VARCHAR(150),
	@PasswordHash VARCHAR(255),
	@Estado       BIT = 1
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Usuarios (IdRol, Nombre, Correo, PasswordHash, Estado)
	VALUES (@IdRol, @Nombre, @Correo, @PasswordHash, @Estado);

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdUsuario;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Eliminar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Eliminar usuario (elimina también sus tokens de recuperación) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_Eliminar]
	@IdUsuario INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRAN;
			DELETE FROM dbo.RecuperacionPassword WHERE IdUsuario = @IdUsuario;
			DELETE FROM dbo.Usuarios WHERE IdUsuario = @IdUsuario;
		COMMIT TRAN;
		SELECT @@ROWCOUNT AS Afectados;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRAN;
		THROW;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ExisteCorreo]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Verificar si un correo ya existe (para validaciones). Excluye un Id opcional. */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_ExisteCorreo]
	@Correo    VARCHAR(150),
	@IdExcluir INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT CASE WHEN EXISTS (
		SELECT 1 FROM dbo.Usuarios
		WHERE Correo = @Correo
		  AND (@IdExcluir IS NULL OR IdUsuario <> @IdExcluir)
	) THEN 1 ELSE 0 END AS Existe;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_Listar]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Listar usuarios (sin hash) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_Listar]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT u.IdUsuario, u.IdRol, u.Nombre, u.Correo,
		   u.Estado, u.FechaRegistro, r.Nombre AS RolNombre
	FROM dbo.Usuarios u
	INNER JOIN dbo.Roles r ON r.IdRol = u.IdRol
	ORDER BY u.Nombre;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ObtenerPorCorreo]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* ==========================  USUARIOS  ==================================== */

/* Obtener usuario por correo (incluye hash y nombre de rol) -> para LOGIN */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_ObtenerPorCorreo]
	@Correo VARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT u.IdUsuario, u.IdRol, u.Nombre, u.Correo, u.PasswordHash,
		   u.Estado, u.FechaRegistro, r.Nombre AS RolNombre
	FROM dbo.Usuarios u
	INNER JOIN dbo.Roles r ON r.IdRol = u.IdRol
	WHERE u.Correo = @Correo;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Usuario_ObtenerPorId]    Script Date: 21/8/2026 14:35:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* Obtener usuario por Id (sin hash) */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_ObtenerPorId]
	@IdUsuario INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT u.IdUsuario, u.IdRol, u.Nombre, u.Correo,
		   u.Estado, u.FechaRegistro, r.Nombre AS RolNombre
	FROM dbo.Usuarios u
	INNER JOIN dbo.Roles r ON r.IdRol = u.IdRol
	WHERE u.IdUsuario = @IdUsuario;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Cliente_ObtenerPorCorreo] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Ficha de cliente a partir del correo. La usan los controladores para saber
   qué cliente es el usuario que tiene la sesión abierta. */
CREATE OR ALTER PROCEDURE [dbo].[sp_Cliente_ObtenerPorCorreo]
	@Correo VARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT IdCliente, Nombre, Apellido, Cedula, Telefono, Correo, Direccion, FechaRegistro, Estado
	FROM dbo.Clientes
	WHERE Correo = @Correo;
END
GO

/****** Object:  StoredProcedure [dbo].[sp_Usuario_ListarPorRol] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Usuarios activos de un rol. Se usa, por ejemplo, para llenar el combo de
   entrenadores (IdRol = 3) en las reservas. */
CREATE OR ALTER PROCEDURE [dbo].[sp_Usuario_ListarPorRol]
	@IdRol INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT u.IdUsuario, u.IdRol, u.Nombre, u.Correo, u.Estado, u.FechaRegistro,
	       r.Nombre AS RolNombre
	FROM dbo.Usuarios u
	INNER JOIN dbo.Roles r ON r.IdRol = u.IdRol
	WHERE u.IdRol = @IdRol
	  AND u.Estado = 1
	ORDER BY u.Nombre;
END
GO

/****** Object:  StoredProcedure [dbo].[sp_Reserva_ListarPorCliente] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Todas las reservas de un cliente (pasadas y pendientes), de la más reciente
   a la más antigua. Distinto de sp_Reserva_ListarPendientesPorCliente. */
CREATE OR ALTER PROCEDURE [dbo].[sp_Reserva_ListarPorCliente]
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT r.IdReserva,
	       r.IdCliente,
	       e.Nombre AS EntrenadorNombre,
	       r.IdEntrenador,
	       r.FechaHora,
	       r.Costo,
	       r.Estado
	FROM dbo.ReservasEntrenador r
	INNER JOIN dbo.Usuarios e ON e.IdUsuario = r.IdEntrenador
	WHERE r.IdCliente = @IdCliente
	ORDER BY r.FechaHora DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[sp_Membresia_DatosRecordatorio] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Datos necesarios para enviarle a un cliente el recordatorio de vencimiento
   de su membresía. Lo llama MembresiasController.EnviarRecordatorio. */
CREATE OR ALTER PROCEDURE [dbo].[sp_Membresia_DatosRecordatorio]
	@IdMembresiaCliente INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mc.IdMembresiaCliente,
	       mc.FechaFin,
	       c.Nombre,
	       c.Apellido,
	       c.Correo,
	       p.Nombre AS PlanNombre
	FROM dbo.MembresiaCliente mc
	INNER JOIN dbo.Clientes c ON c.IdCliente = mc.IdCliente
	INNER JOIN dbo.PlanesMembresia p ON p.IdPlan = mc.IdPlan
	WHERE mc.IdMembresiaCliente = @IdMembresiaCliente;
END
GO

/****** Object:  StoredProcedure [dbo].[sp_LogError_Crear] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Registra un error no controlado. Lo llama el middleware de excepciones
   de la capa web (Tarea1/Middleware/ManejoExcepcionesMiddleware.cs). */
CREATE OR ALTER PROCEDURE [dbo].[sp_LogError_Crear]
	@Mensaje VARCHAR(1000),
	@StackTrace VARCHAR(MAX) = NULL,
	@Ruta VARCHAR(300) = NULL,
	@UsuarioAfectado VARCHAR(150) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.LogErrores (Mensaje, StackTrace, Ruta, UsuarioAfectado, Fecha)
	VALUES (@Mensaje, @StackTrace, @Ruta, @UsuarioAfectado, GETDATE());

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdError;
END
GO

/* Consultar los errores registrados (para la pantalla de administracion). */
CREATE OR ALTER PROCEDURE [dbo].[sp_LogError_Listar]
	@Top INT = 200
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP (@Top) IdError, Mensaje, StackTrace, Ruta, UsuarioAfectado, Fecha
	FROM dbo.LogErrores
	ORDER BY Fecha DESC;
END
GO

/* ============================================================================
   DATOS INICIALES (semilla)
   Sin estos registros la aplicacion no funciona: SP_RegistrarUsuario inserta
   los usuarios que se auto-registran con IdRol = 4 (Cliente).
   ============================================================================ */
USE [GymManagementDB]
GO

SET IDENTITY_INSERT [dbo].[Roles] ON;
GO
MERGE [dbo].[Roles] AS destino
USING (VALUES
    (1, 'Administrador', 'Acceso total al sistema'),
    (2, 'Recepcionista', 'Gestion de clientes, membresias, pagos y citas'),
    (3, 'Entrenador',    'Gestion de rutinas, reservas y asistencia'),
    (4, 'Cliente',       'Consulta su perfil, rutinas, membresia y pagos')
) AS origen (IdRol, Nombre, Descripcion)
    ON destino.IdRol = origen.IdRol
WHEN MATCHED THEN
    UPDATE SET Nombre = origen.Nombre, Descripcion = origen.Descripcion
WHEN NOT MATCHED THEN
    INSERT (IdRol, Nombre, Descripcion) VALUES (origen.IdRol, origen.Nombre, origen.Descripcion);
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF;
GO

/* Usuario administrador inicial.
   Correo:     admin@gymmanagement.com
   Contrasena: Admin123*
   El hash es BCrypt (workFactor 11). CAMBIAR ESTA CONTRASENA DESPUES DEL PRIMER INGRESO. */
IF NOT EXISTS (SELECT 1 FROM [dbo].[Usuarios] WHERE Correo = 'admin@gymmanagement.com')
BEGIN
    INSERT INTO [dbo].[Usuarios] (IdRol, Nombre, Correo, PasswordHash, Estado)
    VALUES (1, 'Administrador', 'admin@gymmanagement.com',
            '$2a$11$K0jpGblKh62vN02B0gTz.eNvReHrlpap4G/7T5GBX13Fr.WmWde.2', 1);
END
GO

/* Planes de membresia de ejemplo (opcional: la portada los muestra) */
IF NOT EXISTS (SELECT 1 FROM [dbo].[PlanesMembresia])
BEGIN
    INSERT INTO [dbo].[PlanesMembresia] (Nombre, Descripcion, DuracionDias, Precio, Estado)
    VALUES ('Mensual',    'Acceso ilimitado durante 30 dias',   30,  25000.00, 1),
           ('Trimestral', 'Acceso ilimitado durante 90 dias',   90,  67500.00, 1),
           ('Anual',      'Acceso ilimitado durante 365 dias', 365, 240000.00, 1);
END
GO
USE [master]
GO
ALTER DATABASE [GymManagementDB] SET  READ_WRITE 
GO
