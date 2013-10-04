USE [master]
GO
/****** Object:  Database [BlurbDB]    Script Date: 2013/09/30 07:15:12 PM ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'BlurbDB')
BEGIN
CREATE DATABASE [BlurbDB] ON  PRIMARY 
( NAME = N'BlurbDB', FILENAME = N'D:\Databases\BlurbDB.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BlurbDB_log', FILENAME = N'D:\Databases\BlurbDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END

GO
ALTER DATABASE [BlurbDB] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BlurbDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BlurbDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BlurbDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BlurbDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BlurbDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BlurbDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BlurbDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BlurbDB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [BlurbDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BlurbDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BlurbDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BlurbDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BlurbDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BlurbDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BlurbDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BlurbDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BlurbDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BlurbDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BlurbDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BlurbDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BlurbDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BlurbDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BlurbDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BlurbDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BlurbDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BlurbDB] SET  MULTI_USER 
GO
ALTER DATABASE [BlurbDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BlurbDB] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BlurbDB', N'ON'
GO
USE [BlurbDB]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NOT NULL,
	[ParentCategoryId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[SaveDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK__Area_lkp__300F11AC] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Culture]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Culture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Culture](
	[CultureCode] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[CultureCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NULL,
	[InsertDate] [datetime] NOT NULL,
	[SaveDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Product_lkp] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductCulture_lnk]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductCulture_lnk]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProductCulture_lnk](
	[ProductCultureId] [int] NOT NULL,
	[CultureCode] [char](10) NOT NULL,
	[InsertDate] [datetime] NOT NULL,
	[SaveDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductCulture_lnk] PRIMARY KEY CLUSTERED 
(
	[ProductCultureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TextResource]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextResource]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TextResource](
	[TextResourceId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NULL,
	[Description] [varchar](max) NULL,
	[DefaultText] [varchar](max) NULL,
	[InsertDate] [datetime] NULL,
 CONSTRAINT [PK_TextResource] PRIMARY KEY CLUSTERED 
(
	[TextResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TextResourceLocalization]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TextResourceLocalization]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TextResourceLocalization](
	[TextResourceLocalizationId] [int] IDENTITY(1,1) NOT NULL,
	[TextResourceId] [int] NOT NULL,
	[LanguageCode] [varchar](10) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[InsertDate] [datetime] NULL,
 CONSTRAINT [PK_TextResourceLocalization] PRIMARY KEY CLUSTERED 
(
	[TextResourceLocalizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_Area_lkp_productCode]    Script Date: 2013/09/30 07:15:12 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND name = N'IX_Area_lkp_productCode')
CREATE NONCLUSTERED INDEX [IX_Area_lkp_productCode] ON [dbo].[Category]
(
	[CategoryId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ProductCulture_lnk_CultureCode]    Script Date: 2013/09/30 07:15:12 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProductCulture_lnk]') AND name = N'IX_ProductCulture_lnk_CultureCode')
CREATE NONCLUSTERED INDEX [IX_ProductCulture_lnk_CultureCode] ON [dbo].[ProductCulture_lnk]
(
	[CultureCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Area_lkp_Product_id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Area_lkp_Product_id]  DEFAULT ((1)) FOR [ProductId]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Area_lkp_DisplayOrder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Area_lkp_DisplayOrder]  DEFAULT ((100)) FOR [DisplayOrder]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Area_InsertDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Area_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Area_SaveDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Area_SaveDate]  DEFAULT (getdate()) FOR [SaveDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Area_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Area_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Product_lkp_InsertDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_lkp_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Product_lkp_SaveDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_lkp_SaveDate]  DEFAULT (getdate()) FOR [SaveDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Product_lkp_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_lkp_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_ProductCulture_lnk_InsertDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ProductCulture_lnk] ADD  CONSTRAINT [DF_ProductCulture_lnk_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_ProductCulture_lnk_SaveDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ProductCulture_lnk] ADD  CONSTRAINT [DF_ProductCulture_lnk_SaveDate]  DEFAULT (getdate()) FOR [SaveDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_ProductCulture_lnk_UpdateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ProductCulture_lnk] ADD  CONSTRAINT [DF_ProductCulture_lnk_UpdateDate]  DEFAULT (((1)/(1))/(1800)) FOR [UpdateDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TextResource_InsertDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TextResource] ADD  CONSTRAINT [DF_TextResource_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_TextResourceLocalization_InsertDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[TextResourceLocalization] ADD  CONSTRAINT [DF_TextResourceLocalization_InsertDate]  DEFAULT (getdate()) FOR [InsertDate]
END

GO
/****** Object:  Trigger [dbo].[Area_lkp_UpdateDate_Trigger]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Area_lkp_UpdateDate_Trigger]'))
EXEC dbo.sp_executesql @statement = N'create TRIGGER [dbo].[Area_lkp_UpdateDate_Trigger] ON [dbo].[Category]

--Created:  4/21/2006 3:47:44 PM		(auto generated)
--Changes:  -

FOR INSERT, UPDATE

AS

UPDATE  Category SET UpdateDate = GetDate()
	FROM Category
	INNER JOIN inserted
		ON inserted.Area_id = Category.Area_id
' 
GO
/****** Object:  Trigger [dbo].[Product_lkp_UpdateDate_Trigger]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Product_lkp_UpdateDate_Trigger]'))
EXEC dbo.sp_executesql @statement = N'/********************************************************************************
Trigger: Product_lkp_UpdateDate_Trigger
Created: 12/30/2007 5:25:58 PM					Spewer v2.0
Changes: -

********************************************************************************/

create Trigger [dbo].[Product_lkp_UpdateDate_Trigger] On [dbo].[Product]

For Insert, Update

As

Update Product_lkp Set UpdateDate = GetDate()
    From Product_lkp
    Inner Join inserted
        On  inserted.Code = Product_lkp.Code
' 
GO
/****** Object:  Trigger [dbo].[ProductCulture_lnk_UpdateDate_Trigger]    Script Date: 2013/09/30 07:15:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ProductCulture_lnk_UpdateDate_Trigger]'))
EXEC dbo.sp_executesql @statement = N'
/********************************************************************************
Trigger: ProductCulture_lnk_UpdateDate_Trigger
Created: 12/30/2007 5:18:11 PM					Spewer v2.0
Changes: -

********************************************************************************/

create Trigger [dbo].[ProductCulture_lnk_UpdateDate_Trigger] On [dbo].[ProductCulture_lnk]

For Insert, Update

As

Update ProductCulture_lnk Set UpdateDate = GetDate()
    From ProductCulture_lnk
    Inner Join inserted
        On  inserted.ProductCulture_Id = ProductCulture_lnk.ProductCulture_Id
' 
GO
USE [master]
GO
ALTER DATABASE [BlurbDB] SET  READ_WRITE 
GO
