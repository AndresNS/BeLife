
SET QUOTED_IDENTIFIER ON

go


/* Create BeLife database.                                                                  */
use master  

go

create database "BeLife"  

go

use "BeLife"  

go


-- Create tables section -------------------------------------------------

-- Table dbo.Cliente

CREATE TABLE [dbo].[Cliente]
(
 [RutCliente] Nvarchar(10) NOT NULL,
 [Nombres] Nvarchar(20) NOT NULL,
 [Apellidos] Nvarchar(20) NOT NULL,
 [FechaNacimiento] Nvarchar(20) NOT NULL,
 [IdSexo] Int NOT NULL,
 [IdEstadoCivil] Int NOT NULL
)
ON [PRIMARY]
go

-- Add keys for table dbo.Cliente

ALTER TABLE [dbo].[Cliente] ADD CONSTRAINT [Cliente_PK] PRIMARY KEY ([RutCliente])
 ON [PRIMARY]
go

-- Table dbo.EstadoCivil

CREATE TABLE [dbo].[EstadoCivil]
(
 [IdEstadoCivil] Int NOT NULL,
 [Descripcion] Nvarchar(15) NOT NULL
)
ON [PRIMARY]
go

-- Add keys for table dbo.EstadoCivil

ALTER TABLE [dbo].[EstadoCivil] ADD CONSTRAINT [EstadoCivil_PK] PRIMARY KEY ([IdEstadoCivil])
 ON [PRIMARY]
go

-- Table dbo.Sexo

CREATE TABLE [dbo].[Sexo]
(
 [IdSexo] Int NOT NULL,
 [Descripcion] Nvarchar(10) NOT NULL
)
ON [PRIMARY]
go

-- Add keys for table dbo.Sexo

ALTER TABLE [dbo].[Sexo] ADD CONSTRAINT [Sexo_PK] PRIMARY KEY ([IdSexo])
 ON [PRIMARY]
go

-- Table Contrato

CREATE TABLE [Contrato]
(
 [Numero] Nvarchar(14) NOT NULL,
 [FechaCreacion] Datetime NOT NULL,
 [RutCliente] Nvarchar(10) NOT NULL,
 [CodigoPlan] Nvarchar(5) NOT NULL,
 [FechaInicioVigencia] Datetime NOT NULL,
 [FechaFinVigencia] Datetime NOT NULL,
 [Vigente] Bit NOT NULL,
 [DeclaracionSalud] Bit NOT NULL,
 [PrimaAnual] Float NOT NULL,
 [PrimaMensual] Float NOT NULL,
 [Observaciones] Nvarchar(max) NOT NULL,
 [FechaTermino] Datetime NOT NULL
)
go

-- Add keys for table Contrato

ALTER TABLE [Contrato] ADD CONSTRAINT [Key1] PRIMARY KEY ([Numero],[RutCliente],[CodigoPlan])
go

-- Table Plan

CREATE TABLE [Plan]
(
 [IdPlan] Nvarchar(5) NOT NULL,
 [Nombre] Nvarchar(20) NOT NULL,
 [PrimaBase] Float NOT NULL,
 [PolizaActual] Nvarchar(15) NOT NULL
)
go

-- Add keys for table Plan

ALTER TABLE [Plan] ADD CONSTRAINT [Key2] PRIMARY KEY ([IdPlan])
go

-- Create foreign keys (relationships) section ------------------------------------------------- 


ALTER TABLE [dbo].[Cliente] ADD CONSTRAINT [Cliente_EstadoCivil_FK1] FOREIGN KEY ([IdEstadoCivil]) REFERENCES [dbo].[EstadoCivil] ([IdEstadoCivil]) ON UPDATE NO ACTION ON DELETE NO ACTION
go


ALTER TABLE [dbo].[Cliente] ADD CONSTRAINT [Cliente_Sexo_FK1] FOREIGN KEY ([IdSexo]) REFERENCES [dbo].[Sexo] ([IdSexo]) ON UPDATE NO ACTION ON DELETE NO ACTION
go


ALTER TABLE [Contrato] ADD CONSTRAINT [Contrato_Cliente_FK1] FOREIGN KEY ([RutCliente]) REFERENCES [dbo].[Cliente] ([RutCliente]) ON UPDATE NO ACTION ON DELETE NO ACTION
go


ALTER TABLE [Contrato] ADD CONSTRAINT [Contrato_Plan_FK1] FOREIGN KEY ([CodigoPlan]) REFERENCES [Plan] ([IdPlan]) ON UPDATE NO ACTION ON DELETE NO ACTION
go


/* Registros de Sexo */

INSERT INTO [BeLife].[dbo].[Sexo] VALUES (1,'Hombre')
INSERT INTO [BeLife].[dbo].[Sexo] VALUES (2,'Mujer')
go

/* Registros de EstadoCivil */

INSERT INTO [BeLife].[dbo].[EstadoCivil] VALUES (1,'Soltero')
INSERT INTO [BeLife].[dbo].[EstadoCivil] VALUES (2,'Casado')
INSERT INTO [BeLife].[dbo].[EstadoCivil] VALUES (3,'Divorciado')
INSERT INTO [BeLife].[dbo].[EstadoCivil] VALUES (4,'Viudo')
go

/* Registros de Planes */

INSERT INTO [BeLife].[dbo].[Plan] VALUES ('VID01','Vida Inicial', 0.5 ,'POL120113229')
INSERT INTO [BeLife].[dbo].[Plan] VALUES ('VID02','Vida Total',3.5,'POL120648575')
INSERT INTO [BeLife].[dbo].[Plan] VALUES ('VID03','Vida Conductor',1.2,'POL125235079')
INSERT INTO [BeLife].[dbo].[Plan] VALUES ('VID04','Vida Senior',2.0,'POL120100054')
INSERT INTO [BeLife].[dbo].[Plan] VALUES ('VID05','Vida Ahorro',3.5,'POL120500489')
go



