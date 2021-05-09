USE [BANCODELTIEMPO]
GO
/****** Object:  Table [dbo].[Candidatura]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Candidatura](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idServicio] [int] NULL,
	[idUsuario] [int] NULL,
	[estado] [varchar](50) NULL,
	[fechaInscripcion] [datetime] NULL,
	[horasRequeridas] [int] NULL,
 CONSTRAINT [PK_Candidatura] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Candidatura_Aceptada]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Candidatura_Aceptada](
	[idCandidatura] [int] NOT NULL,
	[fechaAceptacion] [datetime] NULL,
	[fechaAcordadaServicio] [datetime] NULL,
	[horasAcordadas] [datetime] NULL,
 CONSTRAINT [PK_Candidatura_Aceptada] PRIMARY KEY CLUSTERED 
(
	[idCandidatura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Candidatura_Cancelada]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Candidatura_Cancelada](
	[idCandidatura] [int] NOT NULL,
	[fechaCancelacion] [datetime] NULL,
	[comentario] [varchar](50) NULL,
	[motivoCancelacion] [varchar](50) NULL,
 CONSTRAINT [PK_Candidatura_Cancelada] PRIMARY KEY CLUSTERED 
(
	[idCandidatura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Candidatura_Finalizada]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Candidatura_Finalizada](
	[idCandidatura] [int] NOT NULL,
	[valoracion] [int] NULL,
	[comentarios] [varchar](50) NULL,
	[horasGanadas] [int] NULL,
 CONSTRAINT [PK_Candidatura_Finalizada] PRIMARY KEY CLUSTERED 
(
	[idCandidatura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias_Servicios]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias_Servicios](
	[idUsuario] [int] NOT NULL,
	[idServicios] [int] NOT NULL,
 CONSTRAINT [PK_Categorias_Servicios] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC,
	[idServicios] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias_Usuarios]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias_Usuarios](
	[idUsuarios] [int] NOT NULL,
	[idCategoria] [int] NOT NULL,
 CONSTRAINT [PK_Categorias_Usuarios] PRIMARY KEY CLUSTERED 
(
	[idUsuarios] ASC,
	[idCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servicios]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servicios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idCreador] [int] NULL,
	[titulo] [varchar](50) NULL,
	[descripcion] [varchar](50) NULL,
	[fechaCreacion] [datetime] NULL,
	[finalizado] [bit] NULL,
 CONSTRAINT [PK_Servicios] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 09/05/2021 12:52:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[hasPassword] [varbinary](256) NULL,
	[nombre] [varchar](50) NULL,
	[apellidos] [varchar](50) NULL,
	[nombreUsuario] [varchar](50) NULL,
	[tiempoAcumulado] [int] NULL,
	[telefono] [varchar](50) NULL,
	[correo] [varchar](50) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Candidatura]  WITH CHECK ADD  CONSTRAINT [FK_Candidatura_Servicios] FOREIGN KEY([idServicio])
REFERENCES [dbo].[Servicios] ([id])
GO
ALTER TABLE [dbo].[Candidatura] CHECK CONSTRAINT [FK_Candidatura_Servicios]
GO
ALTER TABLE [dbo].[Candidatura_Aceptada]  WITH CHECK ADD  CONSTRAINT [FK_Candidatura_Aceptada_Candidatura] FOREIGN KEY([idCandidatura])
REFERENCES [dbo].[Candidatura] ([id])
GO
ALTER TABLE [dbo].[Candidatura_Aceptada] CHECK CONSTRAINT [FK_Candidatura_Aceptada_Candidatura]
GO
ALTER TABLE [dbo].[Candidatura_Cancelada]  WITH CHECK ADD  CONSTRAINT [FK_Candidatura_Cancelada_Candidatura] FOREIGN KEY([idCandidatura])
REFERENCES [dbo].[Candidatura] ([id])
GO
ALTER TABLE [dbo].[Candidatura_Cancelada] CHECK CONSTRAINT [FK_Candidatura_Cancelada_Candidatura]
GO
ALTER TABLE [dbo].[Candidatura_Finalizada]  WITH CHECK ADD  CONSTRAINT [FK_Candidatura_Finalizada_Candidatura] FOREIGN KEY([idCandidatura])
REFERENCES [dbo].[Candidatura] ([id])
GO
ALTER TABLE [dbo].[Candidatura_Finalizada] CHECK CONSTRAINT [FK_Candidatura_Finalizada_Candidatura]
GO
ALTER TABLE [dbo].[Categorias_Servicios]  WITH CHECK ADD  CONSTRAINT [FK_Categorias_Servicios_Categorias] FOREIGN KEY([idUsuario])
REFERENCES [dbo].[Categorias] ([id])
GO
ALTER TABLE [dbo].[Categorias_Servicios] CHECK CONSTRAINT [FK_Categorias_Servicios_Categorias]
GO
ALTER TABLE [dbo].[Categorias_Servicios]  WITH CHECK ADD  CONSTRAINT [FK_Categorias_Servicios_Servicios] FOREIGN KEY([idServicios])
REFERENCES [dbo].[Servicios] ([id])
GO
ALTER TABLE [dbo].[Categorias_Servicios] CHECK CONSTRAINT [FK_Categorias_Servicios_Servicios]
GO
ALTER TABLE [dbo].[Categorias_Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Categorias_Usuarios_Categorias] FOREIGN KEY([idUsuarios])
REFERENCES [dbo].[Categorias] ([id])
GO
ALTER TABLE [dbo].[Categorias_Usuarios] CHECK CONSTRAINT [FK_Categorias_Usuarios_Categorias]
GO
ALTER TABLE [dbo].[Categorias_Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Categorias_Usuarios_Usuarios] FOREIGN KEY([idCategoria])
REFERENCES [dbo].[Usuarios] ([id])
GO
ALTER TABLE [dbo].[Categorias_Usuarios] CHECK CONSTRAINT [FK_Categorias_Usuarios_Usuarios]
GO
ALTER TABLE [dbo].[Servicios]  WITH CHECK ADD  CONSTRAINT [FK_Servicios_Usuarios] FOREIGN KEY([idCreador])
REFERENCES [dbo].[Usuarios] ([id])
GO
ALTER TABLE [dbo].[Servicios] CHECK CONSTRAINT [FK_Servicios_Usuarios]
GO
