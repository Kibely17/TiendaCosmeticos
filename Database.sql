-- =====================================================================
-- Sistema de Gestion de Tienda de Cosmeticos
-- Script de creacion de la base de datos (enfoque Database First)
-- =====================================================================

IF DB_ID('TiendaCosmeticos') IS NULL
    CREATE DATABASE TiendaCosmeticos;
GO

USE TiendaCosmeticos;
GO

-- Tabla de Marcas: las marcas de cosmeticos que vende la tienda.
IF OBJECT_ID('dbo.Marcas', 'U') IS NULL
CREATE TABLE dbo.Marcas
(
    Id      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Marcas PRIMARY KEY,
    Nombre  NVARCHAR(100)     NOT NULL,
    Pais    NVARCHAR(100)     NULL,
    Created DATETIME2         NOT NULL CONSTRAINT DF_Marcas_Created DEFAULT (SYSDATETIME()),
    Updated DATETIME2         NULL
);
GO

-- Tabla de Productos: cada producto pertenece a una Marca (uno a muchos).
IF OBJECT_ID('dbo.Productos', 'U') IS NULL
CREATE TABLE dbo.Productos
(
    Id      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Productos PRIMARY KEY,
    Nombre  NVARCHAR(150)     NOT NULL,
    Precio  DECIMAL(18,2)     NOT NULL,
    Stock   INT               NOT NULL CONSTRAINT DF_Productos_Stock DEFAULT (0),
    MarcaId INT               NOT NULL,
    Created DATETIME2         NOT NULL CONSTRAINT DF_Productos_Created DEFAULT (SYSDATETIME()),
    Updated DATETIME2         NULL,
    CONSTRAINT FK_Productos_Marcas FOREIGN KEY (MarcaId) REFERENCES dbo.Marcas(Id)
);
GO

-- Datos iniciales de ejemplo.
IF NOT EXISTS (SELECT 1 FROM dbo.Marcas)
BEGIN
    INSERT INTO dbo.Marcas (Nombre, Pais) VALUES
        (N'Maybelline', N'Estados Unidos'),
        (N'L''Oreal',   N'Francia'),
        (N'Vogue',      N'Republica Dominicana');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Productos)
BEGIN
    INSERT INTO dbo.Productos (Nombre, Precio, Stock, MarcaId) VALUES
        (N'Labial Mate Rojo',        450.00, 25, 1),
        (N'Base Liquida Tono 220',   890.00, 12, 1),
        (N'Rimel Volumen Extremo',   650.00, 30, 2);
END
GO
