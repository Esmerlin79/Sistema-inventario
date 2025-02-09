USE [master]
GO
/****** Object:  Database [Inventario]    Script Date: 19/1/2021 10:48:01 p. m. ******/
CREATE DATABASE [Inventario]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Inventario', FILENAME = N'D:\SQL\MSSQL15.SQLEXPRESS\MSSQL\DATA\Inventario.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Inventario_log', FILENAME = N'D:\SQL\MSSQL15.SQLEXPRESS\MSSQL\DATA\Inventario_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Inventario] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Inventario].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Inventario] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Inventario] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Inventario] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Inventario] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Inventario] SET ARITHABORT OFF 
GO
ALTER DATABASE [Inventario] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Inventario] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Inventario] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Inventario] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Inventario] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Inventario] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Inventario] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Inventario] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Inventario] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Inventario] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Inventario] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Inventario] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Inventario] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Inventario] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Inventario] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Inventario] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Inventario] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Inventario] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Inventario] SET  MULTI_USER 
GO
ALTER DATABASE [Inventario] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Inventario] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Inventario] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Inventario] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Inventario] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Inventario] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Inventario] SET QUERY_STORE = OFF
GO
USE [Inventario]
GO
/****** Object:  Table [dbo].[articulo]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[articulo](
	[idarticulo] [int] IDENTITY(1,1) NOT NULL,
	[idcategoria] [int] NOT NULL,
	[codigo] [varchar](50) NULL,
	[nombre] [varchar](100) NOT NULL,
	[precio_venta] [decimal](11, 2) NOT NULL,
	[stock] [int] NOT NULL,
	[descripcion] [varchar](256) NULL,
	[condicion] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idarticulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[categoria]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categoria](
	[idcategoria] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[descripcion] [varchar](256) NULL,
	[condicion] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idcategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[detalle_ingreso]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[detalle_ingreso](
	[iddetalle_ingreso] [int] IDENTITY(1,1) NOT NULL,
	[idingreso] [int] NOT NULL,
	[idarticulo] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
	[precio] [decimal](11, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[iddetalle_ingreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[detalle_venta]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[detalle_venta](
	[iddetalle_venta] [int] IDENTITY(1,1) NOT NULL,
	[idventa] [int] NOT NULL,
	[idarticulo] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
	[precio] [decimal](11, 2) NOT NULL,
	[descuento] [decimal](11, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[iddetalle_venta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ingreso]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ingreso](
	[idingreso] [int] IDENTITY(1,1) NOT NULL,
	[idproveedor] [int] NOT NULL,
	[idusuario] [int] NOT NULL,
	[tipo_comprobante] [varchar](20) NOT NULL,
	[serie_comprobante] [varchar](7) NULL,
	[num_comprobante] [varchar](10) NOT NULL,
	[fecha_hora] [datetime] NOT NULL,
	[impuesto] [decimal](4, 2) NOT NULL,
	[total] [decimal](11, 2) NOT NULL,
	[estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idingreso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[persona]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[persona](
	[idpersona] [int] IDENTITY(1,1) NOT NULL,
	[tipo_persona] [varchar](20) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[tipo_documento] [varchar](20) NULL,
	[num_documento] [varchar](20) NULL,
	[direccion] [varchar](70) NULL,
	[telefono] [varchar](20) NULL,
	[email] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[idpersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rol]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rol](
	[idrol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](30) NOT NULL,
	[descripcion] [varchar](100) NULL,
	[condicion] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idrol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuario]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuario](
	[idusuario] [int] IDENTITY(1,1) NOT NULL,
	[idrol] [int] NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[tipo_documento] [varchar](20) NULL,
	[num_documento] [varchar](20) NULL,
	[direccion] [varchar](70) NULL,
	[telefono] [varchar](20) NULL,
	[email] [varchar](50) NOT NULL,
	[password_hash] [varbinary](max) NOT NULL,
	[password_salt] [varbinary](max) NOT NULL,
	[condicion] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[idusuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[venta]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[venta](
	[idventa] [int] IDENTITY(1,1) NOT NULL,
	[idcliente] [int] NOT NULL,
	[idusuario] [int] NOT NULL,
	[tipo_comprobante] [varchar](20) NOT NULL,
	[serie_comprobante] [varchar](7) NULL,
	[num_comprobante] [varchar](10) NOT NULL,
	[fecha_hora] [datetime] NOT NULL,
	[impuesto] [decimal](4, 2) NOT NULL,
	[total] [decimal](11, 2) NOT NULL,
	[estado] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idventa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[articulo] ON 

INSERT [dbo].[articulo] ([idarticulo], [idcategoria], [codigo], [nombre], [precio_venta], [stock], [descripcion], [condicion]) VALUES (1, 1, N'3434343', N'Coflakes', CAST(187.00 AS Decimal(11, 2)), 25, N'Coflakes de chocolate', 1)
INSERT [dbo].[articulo] ([idarticulo], [idcategoria], [codigo], [nombre], [precio_venta], [stock], [descripcion], [condicion]) VALUES (2, 1002, N'4353443', N'Manzanas', CAST(40.00 AS Decimal(11, 2)), 100, N'Manzanas verdes', 1)
SET IDENTITY_INSERT [dbo].[articulo] OFF
GO
SET IDENTITY_INSERT [dbo].[categoria] ON 

INSERT [dbo].[categoria] ([idcategoria], [nombre], [descripcion], [condicion]) VALUES (1, N'Cereales', N'Todos las variedades de cereales', 1)
INSERT [dbo].[categoria] ([idcategoria], [nombre], [descripcion], [condicion]) VALUES (2, N'Embutido', N'Variedad de Embutido', 1)
INSERT [dbo].[categoria] ([idcategoria], [nombre], [descripcion], [condicion]) VALUES (1002, N'Frutas', N'Todas las frutas', 1)
INSERT [dbo].[categoria] ([idcategoria], [nombre], [descripcion], [condicion]) VALUES (1003, N'Plasticos', N'Todos Los productos plasticos', 1)
SET IDENTITY_INSERT [dbo].[categoria] OFF
GO
SET IDENTITY_INSERT [dbo].[detalle_ingreso] ON 

INSERT [dbo].[detalle_ingreso] ([iddetalle_ingreso], [idingreso], [idarticulo], [cantidad], [precio]) VALUES (1, 4, 2, 1, CAST(50.00 AS Decimal(11, 2)))
INSERT [dbo].[detalle_ingreso] ([iddetalle_ingreso], [idingreso], [idarticulo], [cantidad], [precio]) VALUES (2, 4, 1, 3, CAST(150.00 AS Decimal(11, 2)))
INSERT [dbo].[detalle_ingreso] ([iddetalle_ingreso], [idingreso], [idarticulo], [cantidad], [precio]) VALUES (3, 5, 2, 5, CAST(20.00 AS Decimal(11, 2)))
INSERT [dbo].[detalle_ingreso] ([iddetalle_ingreso], [idingreso], [idarticulo], [cantidad], [precio]) VALUES (4, 6, 2, 5, CAST(26.00 AS Decimal(11, 2)))
SET IDENTITY_INSERT [dbo].[detalle_ingreso] OFF
GO
SET IDENTITY_INSERT [dbo].[detalle_venta] ON 

INSERT [dbo].[detalle_venta] ([iddetalle_venta], [idventa], [idarticulo], [cantidad], [precio], [descuento]) VALUES (1, 4, 2, 4, CAST(40.00 AS Decimal(11, 2)), CAST(5.00 AS Decimal(11, 2)))
INSERT [dbo].[detalle_venta] ([iddetalle_venta], [idventa], [idarticulo], [cantidad], [precio], [descuento]) VALUES (2, 5, 2, 5, CAST(40.00 AS Decimal(11, 2)), CAST(0.00 AS Decimal(11, 2)))
INSERT [dbo].[detalle_venta] ([iddetalle_venta], [idventa], [idarticulo], [cantidad], [precio], [descuento]) VALUES (3, 6, 1, 5, CAST(187.00 AS Decimal(11, 2)), CAST(0.00 AS Decimal(11, 2)))
SET IDENTITY_INSERT [dbo].[detalle_venta] OFF
GO
SET IDENTITY_INSERT [dbo].[ingreso] ON 

INSERT [dbo].[ingreso] ([idingreso], [idproveedor], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (3, 2, 1002, N'FACTURA', N'001', N'0001', CAST(N'2020-10-10T00:00:00.000' AS DateTime), CAST(20.00 AS Decimal(4, 2)), CAST(100.00 AS Decimal(11, 2)), N'Anulado')
INSERT [dbo].[ingreso] ([idingreso], [idproveedor], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (4, 2, 1002, N'FACTURA', N'004', N'004', CAST(N'2021-01-17T13:14:27.307' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(500.00 AS Decimal(11, 2)), N'Aceptado')
INSERT [dbo].[ingreso] ([idingreso], [idproveedor], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (5, 1002, 1002, N'BOLETA', N'0405', N'1221', CAST(N'2021-01-17T14:03:23.207' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(100.00 AS Decimal(11, 2)), N'Aceptado')
INSERT [dbo].[ingreso] ([idingreso], [idproveedor], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (6, 1002, 1002, N'BOLETA', N'7676', N'8453', CAST(N'2021-01-17T14:04:24.293' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(130.00 AS Decimal(11, 2)), N'Anulado')
SET IDENTITY_INSERT [dbo].[ingreso] OFF
GO
SET IDENTITY_INSERT [dbo].[persona] ON 

INSERT [dbo].[persona] ([idpersona], [tipo_persona], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email]) VALUES (1, N'Cliente', N'Juan Rosario', N'DNI', N'23453434', N'Las Palmas', N'24353434', N'Juan@gmail.com')
INSERT [dbo].[persona] ([idpersona], [tipo_persona], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email]) VALUES (2, N'Proveedor', N'Pedro Amarante Group', N'CEDULA', N'40230434', N'Bayona', N'43565453', N'Pe23@hotmail.com')
INSERT [dbo].[persona] ([idpersona], [tipo_persona], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email]) VALUES (3, N'Cliente', N'Carmen del Leon', N'CEDULA', N'34353434', N'Miami', N'32323', N'carne34@gmail.com')
INSERT [dbo].[persona] ([idpersona], [tipo_persona], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email]) VALUES (4, N'Proveedor', N'Inmuebles Santana SRL', N'RNC', N'43553443', N'Los Angels SRL', N'5454646', N'luci@gmail.com')
INSERT [dbo].[persona] ([idpersona], [tipo_persona], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email]) VALUES (1002, N'Proveedor', N'Fruit SA', N'PASAPORTE', N'483405945', N'Bayona', N'5653434243', N'fjsus@gmail.com')
INSERT [dbo].[persona] ([idpersona], [tipo_persona], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email]) VALUES (1003, N'Cliente', N'Luis Soto', N'PASAPORTE', N'54634366454', N'Las America', N'645343', N'l23soto@gmail.com')
SET IDENTITY_INSERT [dbo].[persona] OFF
GO
SET IDENTITY_INSERT [dbo].[rol] ON 

INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [condicion]) VALUES (1, N'Administrador', N'Administrador del sistema', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [condicion]) VALUES (2, N'Almacenero', N'Almacenero del sistema', 1)
INSERT [dbo].[rol] ([idrol], [nombre], [descripcion], [condicion]) VALUES (1002, N'Vendedor', N'Vendedor del Sistema', 1)
SET IDENTITY_INSERT [dbo].[rol] OFF
GO
SET IDENTITY_INSERT [dbo].[usuario] ON 

INSERT [dbo].[usuario] ([idusuario], [idrol], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email], [password_hash], [password_salt], [condicion]) VALUES (1002, 1, N'Esmerlin Cheriel Elivo', N'Cedula', N'40230757391', N'Santo Domingo Oeste', N'8099093245', N'Esmerlin79@hotmail.com', 0x59E65DEFA8EE49223151D64A40EFF279BE9324231516F336FCDA5139371876C57F310644C3DA6D8297157EC4B745FC0A32F0A1EEB49D2F54D88CCD9CCA405721, 0x69BA57BC2E9F9F1F1DE37760C71CA9B3F1CFA4A62C5BEA2060331405F087AE0400DFC3B7BB445DD7EE8762D9355219A64A6767F8C0F15ABC05E2D631A10B98C227F14A8B8267ED6B2A53D1D37ED3F03F96CDACAFF608EFD81A446DED1994D7B1A39366FA28280ADEDBE9676ECF1091DF6A7247D5B7F5A395427DDA4BF19F038C, 1)
INSERT [dbo].[usuario] ([idusuario], [idrol], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email], [password_hash], [password_salt], [condicion]) VALUES (1003, 1002, N'Daniela Almonte', N'DNI', N'3244353434', N'Las caobas', N'2325435434', N'daniel@gmail.com', 0x691DBCAE43EE618D54708B318E2A2FDD00EA3B7B7EA3C435C382BC8B2730A511D2E84A6D9B61BDBA14A71FA15B3A257721413FBBD8DF854C3E60A00A9DCD5012, 0xAF92E0834F3AA0F6FCDEE231260335908714966F8C38399BFB8BE3D955A4F776C683D9EBD2878F9A4EA7A4EE4EA7CE44A1EA545C237B1AC958E8D473F2133CD382F3938F481BB51372A4593782A8BD99777DD277AEDBCDDA4D620DB56C0E50DEC8D79FEEAF8E73A73DAEDADE17B122894A2DCE1E200366FA668852DCFAFB16F5, 0)
INSERT [dbo].[usuario] ([idusuario], [idrol], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email], [password_hash], [password_salt], [condicion]) VALUES (2002, 1002, N'Luis Alvarez', N'CEDULA', N'4343434545454', N'Miami', N'8089867', N'luis@gmail.com', 0xBD0DBFA9DF7D1CA030DABF309F2494C4958EE88ACC56F4C01DED3C70AAA7ABA0AFE6100B9AAEBE9C2DC85258AF9DCB086AB716CD0654B1A66366F3CB7751B033, 0x8DD6530AD0075A416F8F079004E604692A54CB6C3E3A54A3AED515C7412E3235AAD5CC35E8E809EE33439E4F537EB2F290A322DCE1A896C998C15EBFE9BFC8C65233E57917750C0C51F6EADBDE975B3443FE5E378E9012F59ACF1D922A096B0C85B1DA51C0B8C6CBEE6FD168894B6D78897F4A818EEF7E37440B384312829436, 1)
INSERT [dbo].[usuario] ([idusuario], [idrol], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email], [password_hash], [password_salt], [condicion]) VALUES (2003, 2, N'Carmen del Leon', N'RNC', N'435343', N'Los Angels', N'434343', N'carmen@gmail.com', 0x0F10C4BEF899FF86FDBAB568F1F413FB451F63228977CBDFDCCFBF913B5B14FB9D5FCA4480B039A2E1B4C55B3CC87988EF001C70D0B4BCED70A590F3176ED563, 0x02C1158BBB967DA59D78672FE806E0EA18AC9B78ADA891B25EE555B128BEBD58BF48FD3BB92472F2E0FC6D25C6DEA04D1877CCB88A11D933E11A123BCA8011F06E52F7599833E967943DC9AC482A6C7EA726EB4B5DE74580B506E47ECA70561860BA65D5A8243169EFB355B0481118CC73B27C040FC5196F5F4259BF598EFC5D, 1)
INSERT [dbo].[usuario] ([idusuario], [idrol], [nombre], [tipo_documento], [num_documento], [direccion], [telefono], [email], [password_hash], [password_salt], [condicion]) VALUES (2004, 1002, N'Pedro Vazquez', N'CEDULA', N'402343453434', N'Los coquitos', N'464545', N'pedro45@gmail.com', 0x08D258737D439E2F617C21ADFEEC47132FCA23C0A2491792166F1EB118A6E291C7E3069F4104E3C5789230D71A10219332A3D472458D08C71E14795417A774B4, 0x28B787B5748468D8E442D1496B0286BD6DA9AD286D9E49772416936E5CACBA121E3275410C62D9D117C11551F1CDEFD3E69AFB3048CD097E6297B3E42BB4FE0A8ED1B5FA414D1EDE8AAC9E01F9EC5D99BAC2E221F9E7173DCD76BE96970450C6B671D447841C47AA494DF9CDE0588C0A762AEC2CB518FFE0342E4E83A11C473E, 1)
SET IDENTITY_INSERT [dbo].[usuario] OFF
GO
SET IDENTITY_INSERT [dbo].[venta] ON 

INSERT [dbo].[venta] ([idventa], [idcliente], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (3, 1, 1002, N'FACTURA', N'0342', N'4356534', CAST(N'2020-01-04T00:00:00.000' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(15.00 AS Decimal(11, 2)), N'Anulado')
INSERT [dbo].[venta] ([idventa], [idcliente], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (4, 1003, 1002, N'FACTURA', N'0034', N'0034', CAST(N'2021-01-18T21:49:11.540' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(155.00 AS Decimal(11, 2)), N'Aceptado')
INSERT [dbo].[venta] ([idventa], [idcliente], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (5, 3, 1002, N'BOLETA', N'9090', N'9090', CAST(N'2021-01-18T21:54:58.200' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(200.00 AS Decimal(11, 2)), N'Aceptado')
INSERT [dbo].[venta] ([idventa], [idcliente], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (6, 3, 1002, N'FACTURA', N'3232', N'3232', CAST(N'2021-01-18T21:56:01.243' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(935.00 AS Decimal(11, 2)), N'Anulado')
INSERT [dbo].[venta] ([idventa], [idcliente], [idusuario], [tipo_comprobante], [serie_comprobante], [num_comprobante], [fecha_hora], [impuesto], [total], [estado]) VALUES (7, 3, 1002, N'FACTURA', N'43534', N'435213', CAST(N'2021-02-01T00:00:00.000' AS DateTime), CAST(18.00 AS Decimal(4, 2)), CAST(1000.00 AS Decimal(11, 2)), N'Aceptado')
SET IDENTITY_INSERT [dbo].[venta] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__articulo__72AFBCC6D5A9EC9B]    Script Date: 19/1/2021 10:48:01 p. m. ******/
ALTER TABLE [dbo].[articulo] ADD UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__categori__72AFBCC6DCE5208F]    Script Date: 19/1/2021 10:48:01 p. m. ******/
ALTER TABLE [dbo].[categoria] ADD UNIQUE NONCLUSTERED 
(
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[articulo] ADD  DEFAULT ((1)) FOR [condicion]
GO
ALTER TABLE [dbo].[categoria] ADD  DEFAULT ((1)) FOR [condicion]
GO
ALTER TABLE [dbo].[rol] ADD  DEFAULT ((1)) FOR [condicion]
GO
ALTER TABLE [dbo].[usuario] ADD  DEFAULT ((1)) FOR [condicion]
GO
ALTER TABLE [dbo].[articulo]  WITH CHECK ADD FOREIGN KEY([idcategoria])
REFERENCES [dbo].[categoria] ([idcategoria])
GO
ALTER TABLE [dbo].[detalle_ingreso]  WITH CHECK ADD FOREIGN KEY([idarticulo])
REFERENCES [dbo].[articulo] ([idarticulo])
GO
ALTER TABLE [dbo].[detalle_ingreso]  WITH CHECK ADD FOREIGN KEY([idingreso])
REFERENCES [dbo].[ingreso] ([idingreso])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[detalle_venta]  WITH CHECK ADD FOREIGN KEY([idarticulo])
REFERENCES [dbo].[articulo] ([idarticulo])
GO
ALTER TABLE [dbo].[detalle_venta]  WITH CHECK ADD FOREIGN KEY([idventa])
REFERENCES [dbo].[venta] ([idventa])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ingreso]  WITH CHECK ADD FOREIGN KEY([idproveedor])
REFERENCES [dbo].[persona] ([idpersona])
GO
ALTER TABLE [dbo].[ingreso]  WITH CHECK ADD FOREIGN KEY([idusuario])
REFERENCES [dbo].[usuario] ([idusuario])
GO
ALTER TABLE [dbo].[usuario]  WITH CHECK ADD FOREIGN KEY([idrol])
REFERENCES [dbo].[rol] ([idrol])
GO
ALTER TABLE [dbo].[venta]  WITH CHECK ADD FOREIGN KEY([idcliente])
REFERENCES [dbo].[persona] ([idpersona])
GO
ALTER TABLE [dbo].[venta]  WITH CHECK ADD FOREIGN KEY([idusuario])
REFERENCES [dbo].[usuario] ([idusuario])
GO
/****** Object:  Trigger [dbo].[ActualizarStock_Ingreso]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ActualizarStock_Ingreso]
ON [dbo].[detalle_ingreso]
FOR INSERT 
AS
UPDATE a SET a.stock  = a.stock + d.cantidad
FROM articulo a join 
inserted AS d ON d.idarticulo = a.idarticulo
GO
ALTER TABLE [dbo].[detalle_ingreso] ENABLE TRIGGER [ActualizarStock_Ingreso]
GO
/****** Object:  Trigger [dbo].[ActualizarStock_Venta]    Script Date: 19/1/2021 10:48:01 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ActualizarStock_Venta]
ON [dbo].[detalle_venta]
FOR INSERT
AS
UPDATE a SET a.stock = a.stock - d.cantidad
from articulo as a join inserted AS d on d.idarticulo = a.idarticulo
GO
ALTER TABLE [dbo].[detalle_venta] ENABLE TRIGGER [ActualizarStock_Venta]
GO
USE [master]
GO
ALTER DATABASE [Inventario] SET  READ_WRITE 
GO
