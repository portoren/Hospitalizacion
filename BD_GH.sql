USE [master]
GO

CREATE DATABASE [clinica] 
--ON  PRIMARY 
--( NAME = N'clinica_data', FILENAME = N'd:\database\clinica.mdf', SIZE = 167872KB , MAXSIZE = UNLIMITED, FILEGROWTH = 16384KB )
--    LOG ON 
--( NAME = N'clinica_log', FILENAME = N'd:\database\clinica.ldf', SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 16384KB )
GO

USE [clinica]
GO

--CREATE TABLE T_RegistroChecklist (
--	nroRegistroChecklist INT NOT NULL,
--	procedimiento VARCHAR ( 255 ) NOT NULL,
--	observaciones VARCHAR ( 255 ) NOT NULL,
--	T_RegistroChecklist_ID INT IDENTITY NOT NULL,
--	T_Doctor_ID INT NOT NULL,
--	T_OrdenInternamiento_ID INT NOT NULL,
--	T_BitacoraInternamiento_ID INT NOT NULL,
--	CONSTRAINT PK_T_RegistroChecklist8 PRIMARY KEY NONCLUSTERED (T_RegistroChecklist_ID)
--	)
--GO
--CREATE TABLE T_Pregunta (
--	numero_pregunta INT NOT NULL,
--	tipo_pregunta VARCHAR ( 255 ) NOT NULL,
--	descripcion VARCHAR ( 255 ) NOT NULL,
--	T_Pregunta_ID INT IDENTITY NOT NULL,
--	T_RegistroChecklist_ID INT NOT NULL,
--	CONSTRAINT PK_T_Pregunta9 PRIMARY KEY NONCLUSTERED (T_Pregunta_ID)
--	)
--GO

CREATE TABLE Habitacion (
	IdHabitacion INT IDENTITY NOT NULL,
	Numero VARCHAR ( 255 ) NOT NULL,
	Tipo VARCHAR ( 255 ) NOT NULL,
	Disponible INT NOT NULL,
	
	--T_OrdenInternamiento_ID INT NOT NULL,
	--T_BitacoraInternamiento_ID INT NOT NULL,
	--CONSTRAINT TC_T_Habitacion27 UNIQUE NONCLUSTERED (T_BitacoraInternamiento_ID),
	--CONSTRAINT TC_T_Habitacion12 UNIQUE NONCLUSTERED (T_OrdenInternamiento_ID),
	--CONSTRAINT PK_T_Habitacion3 PRIMARY KEY NONCLUSTERED (T_Habitacion_ID)
	CONSTRAINT PK_Habitacion PRIMARY KEY NONCLUSTERED (IdHabitacion)
	)
GO

CREATE TABLE Paciente (
	dni varchar(8) NOT NULL,
	nro_historial_clinico INT NOT NULL,
	nombre VARCHAR ( 255 ) NOT NULL,
	apellido_paterno VARCHAR ( 255 ) NOT NULL,
	apellido_materno VARCHAR ( 255 ) NOT NULL,
	estado_actual VARCHAR ( 255 ) NOT NULL,
	--T_BitacoraInternamiento_ID INT NOT NULL,
	--T_PapeletaSalida_ID INT NOT NULL,
	--CONSTRAINT TC_T_Paciente23 UNIQUE NONCLUSTERED (T_BitacoraInternamiento_ID),
	--CONSTRAINT PK_T_Paciente14 PRIMARY KEY NONCLUSTERED (T_PapeletaSalida_ID)
	CONSTRAINT PK_Paciente PRIMARY KEY NONCLUSTERED (dni)
	)
GO

CREATE TABLE Doctor (
	IdDoctor INT IDENTITY NOT NULL,
	dni VARCHAR(8) NOT NULL,
	nombre VARCHAR ( 255 ) NOT NULL,
	apellido_paterno VARCHAR ( 255 ) NOT NULL,
	apellido_materno VARCHAR ( 255 ) NOT NULL,
	--T_Doctor_ID INT IDENTITY NOT NULL,
	CONSTRAINT PK_Doctor PRIMARY KEY NONCLUSTERED (IdDoctor)
	)
GO

--CREATE TABLE T_Suceso (
--	nro_suceso INT NOT NULL,
--	fecha_hora DATETIME NOT NULL,
--	descripcion VARCHAR ( 255 ) NOT NULL,
--	T_Suceso_ID INT IDENTITY NOT NULL,
--	T_BitacoraInternamiento_ID INT NOT NULL,
--	T_Paciente_T_BitacoraInternamiento_ID INT NOT NULL,
--	CONSTRAINT PK_T_Suceso6 PRIMARY KEY NONCLUSTERED (T_Suceso_ID)
--	)
--GO
--CREATE TABLE T_BitacoraInternamiento (
--	nro_bitacora_internamiento INT NOT NULL,
--	T_BitacoraInternamiento_ID INT IDENTITY NOT NULL,
--	T_Doctor_ID INT NOT NULL,
--	T_OrdenInternamiento_ID INT NOT NULL,
--	T_Paciente_T_BitacoraInternamiento_ID INT NOT NULL,
--	CONSTRAINT PK_T_BitacoraInternamiento5 PRIMARY KEY NONCLUSTERED (T_BitacoraInternamiento_ID),
--	CONSTRAINT TC_T_BitacoraInternamiento10 UNIQUE NONCLUSTERED (T_OrdenInternamiento_ID)
--	)
--GO
--CREATE TABLE T_AltaMedica (
--	nroAlltaMedica INT NOT NULL,
--	descripcion VARCHAR ( 255 ) NOT NULL,
--	generacCita VARCHAR ( 255 ) NOT NULL,
--	T_AltaMedica_ID INT IDENTITY NOT NULL,
--	T_Epicrisis_ID INT NOT NULL,
--	CONSTRAINT TC_T_AltaMedica31 UNIQUE NONCLUSTERED (T_Epicrisis_ID),
--	CONSTRAINT PK_T_AltaMedica10 PRIMARY KEY NONCLUSTERED (T_AltaMedica_ID)
--	)
--GO
--CREATE TABLE T_Epicrisis (
--	nroEpicrisis INT NOT NULL,
--	diagIngreso VARCHAR ( 255 ) NOT NULL,
--	diagEgreso VARCHAR ( 255 ) NOT NULL,
--	tratamiento VARCHAR ( 255 ) NOT NULL,
--	evolucion VARCHAR ( 255 ) NOT NULL,
--	fechaIngreso DATETIME NOT NULL,
--	fechaEgreso DATETIME NOT NULL,
--	totalDas INT NOT NULL,
--	estado VARCHAR ( 255 ) NOT NULL,
--	descripcion VARCHAR ( 255 ) NOT NULL,
--	T_Epicrisis_ID INT IDENTITY NOT NULL,
--	T_OrdenInternamiento_ID INT NOT NULL,
--	T_BitacoraInternamiento_ID INT NOT NULL,
--	CONSTRAINT PK_T_Epicrisis7 PRIMARY KEY NONCLUSTERED (T_Epicrisis_ID),
--	CONSTRAINT TC_T_Epicrisis16 UNIQUE NONCLUSTERED (T_OrdenInternamiento_ID)
--	)
--GO
--CREATE TABLE T_PapeletaSalida (
--	nroPapeletaSalida INT NOT NULL,
--	T_PapeletaSalida_ID INT IDENTITY NOT NULL,
--	T_AltaMedica_ID INT NOT NULL,
--	CONSTRAINT PK_T_PapeletaSalida11 PRIMARY KEY NONCLUSTERED (T_PapeletaSalida_ID),
--	CONSTRAINT TC_T_PapeletaSalida34 UNIQUE NONCLUSTERED (T_AltaMedica_ID)
--	)
--GO

CREATE TABLE Dominio (
	IdDominio VARCHAR ( 3 ) NOT NULL,
	Nombre VARCHAR ( 50 ) NOT NULL,
	CONSTRAINT PK_Dominio PRIMARY KEY NONCLUSTERED (IdDominio)
)
GO

CREATE TABLE Parametro (
	IdParametro VARCHAR ( 3 ) NOT NULL,
	IdDominio VARCHAR ( 3 ) NOT NULL,
	Nombre VARCHAR ( 200 ) NOT NULL,
	CONSTRAINT PK_Parametro PRIMARY KEY NONCLUSTERED (IdDominio, IdParametro),
	CONSTRAINT FK_Dominio_Parametro FOREIGN KEY (IdDominio) REFERENCES Dominio (IdDominio) 
)
GO

CREATE TABLE AccesorioCama (
	IdAccesorioCama INT IDENTITY NOT NULL,
	Nombre VARCHAR ( 50 ) NOT NULL,
	CONSTRAINT PK_AccesorioCama PRIMARY KEY NONCLUSTERED (IdAccesorioCama)
)
GO

CREATE TABLE Cama (
	IdCama INT IDENTITY NOT NULL,
	Marca VARCHAR ( 3 ) NOT NULL,
	Modelo VARCHAR ( 3 ) NOT NULL,
	TipoCama VARCHAR ( 3 ) NOT NULL,
	ModoOperacion VARCHAR ( 3 ) NOT NULL,
	TipoColchon VARCHAR ( 3 ) NOT NULL,
	Estado VARCHAR ( 3 ) NOT NULL,
	
	--T_OrdenInternamiento_ID INT NOT NULL,
	--T_Habitacion_ID INT NOT NULL,
	--T_BitacoraInternamiento_ID INT NOT NULL,

	--CONSTRAINT TC_T_Cama29 UNIQUE NONCLUSTERED (T_BitacoraInternamiento_ID),
	--CONSTRAINT TC_T_Cama14 UNIQUE NONCLUSTERED (T_OrdenInternamiento_ID),
	CONSTRAINT PK_Cama PRIMARY KEY NONCLUSTERED (IdCama)
)
GO

CREATE TABLE Cama_AccesorioCama (
	IdCamaAccesorioCama INT IDENTITY NOT NULL,
	IdAccesorioCama INT NOT NULL,
	IdCama INT NOT NULL,
	CONSTRAINT PK_CamaAccesorioCama PRIMARY KEY NONCLUSTERED (IdCamaAccesorioCama),
	CONSTRAINT FK_AccesorioCama_CamaAccesorioCama FOREIGN KEY (IdAccesorioCama) REFERENCES AccesorioCama (IdAccesorioCama),
	CONSTRAINT FK_Cama_CamaAccesorioCama FOREIGN KEY (IdCama) REFERENCES Cama (IdCama) 
 )
GO

CREATE TABLE OrdenInternamiento (
	IdOrdenInternamiento INT IDENTITY NOT NULL,
	Numero VARCHAR(15) NOT NULL,
	IdDoctor INT NOT NULL,
	IdPaciente varchar(8) NOT NULL,
	IdHabitacion INT NULL,
	IdCama INT NULL,
	Estado VARCHAR(3) NOT NULL,
	--T_BitacoraInternamiento_ID INT NOT NULL,
	--T_Paciente_T_BitacoraInternamiento_ID INT NOT NULL,
	CONSTRAINT PK_OrdenInternamiento PRIMARY KEY NONCLUSTERED (IdOrdenInternamiento),
	CONSTRAINT FK_Doctor_OrdenInternamiento FOREIGN KEY (IdDoctor) REFERENCES Doctor (IdDoctor),
	CONSTRAINT FK_Paciente_OrdenInternamiento FOREIGN KEY (IdPaciente) REFERENCES Paciente (dni),
	CONSTRAINT FK_Habitacion_OrdenInternamiento FOREIGN KEY (IdHabitacion) REFERENCES Habitacion (IdHabitacion)
	--CONSTRAINT PK_T_OrdenInternamiento2 PRIMARY KEY NONCLUSTERED (T_OrdenInternamiento_ID),
	--CONSTRAINT TC_T_OrdenInternamiento25 UNIQUE NONCLUSTERED (T_BitacoraInternamiento_ID)
	)
GO

CREATE TABLE OrdenInternamiento_Recurso (
	IdOrdenInternamientoRecurso INT IDENTITY NOT NULL,
	IdOrdenInternamiento INT NOT NULL,
	Recurso VARCHAR(3) NOT NULL,
	Cantidad INT NOT NULL,
	Observacion VARCHAR ( 255 ) NOT NULL,	
	CONSTRAINT PK_OrdenInternamientoRecurso PRIMARY KEY NONCLUSTERED (IdOrdenInternamientoRecurso),
	CONSTRAINT FK_OrdenInternamiento_OrdenInternamientoRecurso FOREIGN KEY (IdOrdenInternamiento) REFERENCES OrdenInternamiento (IdOrdenInternamiento),
	)
GO

--ALTER TABLE T_OrdenInternamiento ADD CONSTRAINT FK_T_OrdenInternamiento18 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_BitacoraInternamiento (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_OrdenInternamiento ADD CONSTRAINT FK_T_OrdenInternamiento5 FOREIGN KEY (T_Doctor_ID) REFERENCES T_Doctor (T_Doctor_ID) 
--GO
--ALTER TABLE T_OrdenInternamiento ADD CONSTRAINT FK_T_OrdenInternamiento24 FOREIGN KEY (T_Paciente_T_BitacoraInternamiento_ID) REFERENCES T_Paciente (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Cama ADD CONSTRAINT FK_T_Cama10 FOREIGN KEY (T_OrdenInternamiento_ID) REFERENCES T_OrdenInternamiento (T_OrdenInternamiento_ID) 
--GO
--ALTER TABLE T_Cama ADD CONSTRAINT FK_T_Cama20 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_BitacoraInternamiento (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Cama ADD CONSTRAINT FK_T_Cama13 FOREIGN KEY (T_Habitacion_ID) REFERENCES T_Habitacion (T_Habitacion_ID) 
--GO
--ALTER TABLE T_AccesorioCama ADD CONSTRAINT FK_T_AccesorioCama15 FOREIGN KEY (T_Cama_ID) REFERENCES T_Cama (T_Cama_ID) 
--GO
--ALTER TABLE T_Habitacion ADD CONSTRAINT FK_T_Habitacion9 FOREIGN KEY (T_OrdenInternamiento_ID) REFERENCES T_OrdenInternamiento (T_OrdenInternamiento_ID) 
--GO
--ALTER TABLE T_Habitacion ADD CONSTRAINT FK_T_Habitacion19 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_BitacoraInternamiento (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Epicrisis ADD CONSTRAINT FK_T_Epicrisis25 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_Paciente (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Epicrisis ADD CONSTRAINT FK_T_Epicrisis11 FOREIGN KEY (T_OrdenInternamiento_ID) REFERENCES T_OrdenInternamiento (T_OrdenInternamiento_ID) 
--GO
--ALTER TABLE T_RegistroChecklist ADD CONSTRAINT FK_T_RegistroChecklist12 FOREIGN KEY (T_OrdenInternamiento_ID) REFERENCES T_OrdenInternamiento (T_OrdenInternamiento_ID) 
--GO
--ALTER TABLE T_RegistroChecklist ADD CONSTRAINT FK_T_RegistroChecklist7 FOREIGN KEY (T_Doctor_ID) REFERENCES T_Doctor (T_Doctor_ID) 
--GO
--ALTER TABLE T_RegistroChecklist ADD CONSTRAINT FK_T_RegistroChecklist28 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_Paciente (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Suceso ADD CONSTRAINT FK_T_Suceso16 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_BitacoraInternamiento (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Suceso ADD CONSTRAINT FK_T_Suceso27 FOREIGN KEY (T_Paciente_T_BitacoraInternamiento_ID) REFERENCES T_Paciente (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_AltaMedica ADD CONSTRAINT FK_T_AltaMedica21 FOREIGN KEY (T_Epicrisis_ID) REFERENCES T_Epicrisis (T_Epicrisis_ID) 
--GO
--ALTER TABLE T_Pregunta ADD CONSTRAINT FK_T_Pregunta22 FOREIGN KEY (T_RegistroChecklist_ID) REFERENCES T_RegistroChecklist (T_RegistroChecklist_ID) 
--GO
--ALTER TABLE T_BitacoraInternamiento ADD CONSTRAINT FK_T_BitacoraInternamiento8 FOREIGN KEY (T_OrdenInternamiento_ID) REFERENCES T_OrdenInternamiento (T_OrdenInternamiento_ID) 
--GO
--ALTER TABLE T_BitacoraInternamiento ADD CONSTRAINT FK_T_BitacoraInternamiento26 FOREIGN KEY (T_Paciente_T_BitacoraInternamiento_ID) REFERENCES T_Paciente (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_BitacoraInternamiento ADD CONSTRAINT FK_T_BitacoraInternamiento6 FOREIGN KEY (T_Doctor_ID) REFERENCES T_Doctor (T_Doctor_ID) 
--GO
--ALTER TABLE T_PapeletaSalida ADD CONSTRAINT FK_T_PapeletaSalida23 FOREIGN KEY (T_AltaMedica_ID) REFERENCES T_AltaMedica (T_AltaMedica_ID) 
--GO
--ALTER TABLE T_Paciente ADD CONSTRAINT FK_T_Paciente17 FOREIGN KEY (T_BitacoraInternamiento_ID) REFERENCES T_BitacoraInternamiento (T_BitacoraInternamiento_ID) 
--GO
--ALTER TABLE T_Paciente ADD CONSTRAINT FK_T_Paciente29 FOREIGN KEY (T_PapeletaSalida_ID) REFERENCES T_PapeletaSalida (T_PapeletaSalida_ID) 
--GO
--ALTER TABLE T_Recurso ADD CONSTRAINT FK_T_Recurso14 FOREIGN KEY (T_Habitacion_ID) REFERENCES T_Habitacion (T_Habitacion_ID) 
--GO

CREATE TABLE [dbo].[OrdenInternamiento_Bitacora](
	[IdOrdenInternamientoBitacora] [int] IDENTITY(1,1) NOT NULL,
	[IdOrdenInternamiento] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[EstadoPaciente] [varchar](3) NOT NULL,
	[Descripcion] [varchar](255) NOT NULL,
	[Estado] [varchar](3) NOT NULL,
 CONSTRAINT [PK_OrdenInternamientoBitacora] PRIMARY KEY NONCLUSTERED 
(
	[IdOrdenInternamientoBitacora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[OrdenInternamiento_Bitacora]  WITH CHECK ADD  CONSTRAINT [FK_OrdenInternamiento_OrdenInternamientoBitacora] FOREIGN KEY([IdOrdenInternamiento])
REFERENCES [dbo].[OrdenInternamiento] ([IdOrdenInternamiento])
GO

ALTER TABLE [dbo].[OrdenInternamiento_Bitacora] CHECK CONSTRAINT [FK_OrdenInternamiento_OrdenInternamientoBitacora]
GO

INSERT INTO Dominio VALUES ('001','Marca'),('002','Tipo de Cama'),('003','Modo de Operación'),('004','Tipo Colchón'),('005','Estado de la Cama'),('006','Tipo de la Habitación'),
('007','Estado de Orden de Internamiento'),('008','Recurso'),('009','Estado del Paciente'),('010','Procedimiento'),('011','Entrada'),('012','Pausa Quirurgica'),('013','Salida')
go 
INSERT INTO Parametro VALUES 
('001','001','Marca 001'),('002','001','Marca 002'),('003','001','Marca 003'),('004','001','Marca 004'),('005','001','Marca 005'),('006','001','Marca 006'),
('001','002','Tipo de Cama 001'),('002','002','Tipo de Cama 002'),('003','002','Tipo de Cama 003'),('004','002','Tipo de Cama 004'),('005','002','Tipo de Cama 005'),('006','002','Tipo de Cama 006'),
('001','003','Modo de Operación 001'),('002','003','Modo de Operación 002'),('003','003','Modo de Operación 003'),('004','003','Modo de Operación 004'),('005','003','Modo de Operación 005'),('006','003','Modo de Operación 006'),
('001','004','Tipo Colchón 001'),('002','004','Tipo Colchón 002'),('003','004','Tipo Colchón 003'),('004','004','Tipo Colchón 004'),('005','004','Tipo Colchón 005'),('006','004','Tipo Colchón 006'),
('001','005','Abierta'),('002','005','Cerrada'),('003','005','En uso'),('004','005','Anulada'),
('001','006','Simple'),('002','006','Doble'),('003','006','Triple'),
('001','007','No Asignado Habitación'),('002','007','Habitación Asignado'),('003','007','Dado de Alta'),
('002','008','Sillon'),('003','008','Televisor'),('004','008','Mesa'),('005','008','Frigobar'),--('001','008','Cama Ortopedica'),
('001','009','Estable'),('002','009','En Recuperacion'),('001','010','Procedimiento A'),('002','010','Procedimiento B'),
('001','011','El paciente ha confirmado su identidad'),
('002','011','El paciente ha confirmado su consentimiento'),
('003','011','El paciente ha confirmado el procedimiento'),
('004','011','Se ha completado el control de seguridad de la anastesia'),
('005','011','Pulsioximetro colocado y en funcionamiento'),
('006','011','Tiene el paciente alergias conocidas'),
('007','011','Via aerea dificil/riesgo de aspiracion'),
('008','011','Riesgo de Hemorragia > 500ML (7ml/kg niños) y se ha previsto la disponibilidad de acceso intravenoso y liquidos adecuados'),
('001','012','El paciente ha confirmado su identidad'),
('002','012','Confirmar si todos los miembros del equipo se hayan presentado por su nombre y funcion'),
('003','012','El cirujano revisa los pasos...'),
('004','012','El equipo de anastesia revisa..'),
('005','012','El equipo de enfermeria revisa...'),
('006','012','El equipo de enfermeria revisa si existen...'),
('007','012','se ha administrado...'),
('001','013','La enfermera confirma...'),
('002','013','La enfermera confirma verbal...'),
('003','013','El cirujano y anastesista...')
go
INSERT INTO AccesorioCama VALUES ('Almohada'),('Colcha'),('Reja de Seguridad'),('Sábanas')
go
INSERT INTO Habitacion (Numero, Tipo, Disponible) VALUES
('101','001',1),('102','001',1),('202','001',1),('302','001',0),('402','001',0)
go
INSERT INTO Paciente VALUES
('00000001',1,'JUAN','PERES','','001'),('00000002',2,'PEDRO','IBARRA','','001'),('00000003',3,'RONALDO','MESSI','','001'),
('00000004',4,'JUAN','RIQUELME','','001')
go
INSERT INTO Doctor (dni, nombre, apellido_paterno, apellido_materno) values
('00000005','ROGELIO','CANCHES',''),('00000006','JULIO','CONTRERAS',''),('00000007','ALEJANDRO','GARCIA',''),
('00000008','DIONISIO','ELESPURU','')
go
INSERT INTO OrdenInternamiento (Numero,IdDoctor,IdPaciente,IdHabitacion,Estado,IdCama) VALUES 
('09003939',1,'00000001',null,'002',null),('02004040',2,'00000002',null,'002',null),('41099494',3,'00000003',null,'002',null),
('41099495',4,'00000004',null,'001',null)
go

create procedure pa_cama_set_insert
@n_id int output,
@v_marca varchar(3),
@v_modelo varchar(3),
@v_tipocama varchar(3),
@v_modooperacion varchar(3),
@v_tipocolchon varchar(3),
@v_estado varchar(3)
as
begin
insert into Cama (Marca, Modelo, TipoCama, ModoOperacion, TipoColchon, Estado) values (@v_marca, @v_modelo, @v_tipocama, @v_modooperacion, @v_tipocolchon, @v_estado)
set @n_id = (select @@IDENTITY)
end
go

/****** Object:  StoredProcedure [dbo].[pa_bitacora_set_insert]    Script Date: 19/01/2014 01:23:02 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[pa_bitacora_set_insert]
@n_id int output,
@n_ordeninternamiento int,
@f_fecha datetime,
@v_estadopaciente varchar(3),
@v_descripcion varchar(255),
@v_estado varchar(3)
as
begin
insert into [dbo].[OrdenInternamiento_Bitacora] 
([IdOrdenInternamiento], [Fecha], [EstadoPaciente], [Descripcion], [Estado]) 
values (@n_ordeninternamiento, @f_fecha, @v_estadopaciente, @v_descripcion, @v_estado)
set @n_id = (select @@IDENTITY)
end

GO

CREATE TABLE Procedimiento (
	IdProcedimiento INT IDENTITY NOT NULL,
	Tipo VARCHAR ( 3 ) NOT NULL,
	IdOrdenInternamiento [int] NOT NULL,
	Estado VARCHAR ( 3 ) NOT NULL,

	CONSTRAINT PK_Procedimiento PRIMARY KEY NONCLUSTERED (IdProcedimiento),
	CONSTRAINT FK_OrdenInternamiento_Procedimiento FOREIGN KEY (IdOrdenInternamiento) REFERENCES OrdenInternamiento (IdOrdenInternamiento),
)
GO

CREATE TABLE Procedimiento_Pregunta (
	IdProcedimientoPregunta INT IDENTITY NOT NULL,
	IdProcedimiento INT NOT NULL,
	Tipo VARCHAR ( 3 ) NOT NULL,
	Pregunta VARCHAR ( 3 ) NOT NULL,
	Respuesta VARCHAR ( 2 ) NOT NULL,
	Descripcion VARCHAR ( 500 ) NULL,
	Estado VARCHAR ( 3 ) NOT NULL,
	CONSTRAINT PK_ProcedimientoPregunta PRIMARY KEY NONCLUSTERED (IdProcedimientoPregunta),
	CONSTRAINT FK_Procedimiento_ProcedimientoPregunta FOREIGN KEY (IdProcedimiento) REFERENCES Procedimiento (IdProcedimiento)
 )
GO

create procedure pa_procedimiento_set_insert
@n_id int output,
@n_ordeninternamiento int,
@v_tipo varchar(3),
@v_estado varchar(3)
as
begin
insert into Procedimiento (IdOrdenInternamiento, Tipo, Estado) values (@n_ordeninternamiento, @v_tipo, @v_estado)
set @n_id = (select @@IDENTITY)
end
go



