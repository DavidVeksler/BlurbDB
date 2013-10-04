USE master 
GO 
EXEC sp_configure 'show advanced options', 1 
GO 
RECONFIGURE WITH OVERRIDE 
GO 
EXEC sp_configure 'xp_cmdshell', 1 
GO 
RECONFIGURE WITH OVERRIDE 
GO 
EXEC sp_configure 'show advanced options', 0 
GO

USE [Juno]
GO

/****** Object:  StoredProcedure [dbo].[SyncBlurbsDevSSIS]    Script Date: 2013/09/23 03:22:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SyncBlurbsDevSSIS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		DavidV
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SyncBlurbsDevSSIS] 
	
AS
BEGIN
	
	declare @ssisstr varchar(8000), @packagename varchar(200),@servername varchar(100),@params varchar(8000)
SET @packagename = ''SyncJunoBlurbs\SyncBlurbstoStagingandDev''
SET @servername = ''cns-e1dw''
SET @params = ''''

-- /set \package.variables[FileName].Value;"\"\\127.0.0.1\Common\SSIS\NewItem.xls\"" /set \package.variables[CreatedBy].Value;"\"Chirag\"" /set \package.variables[ContractDbConnectionString].Value;"\"Data Source=myserver\SQL2K5;User ID=sa;Password=sapass;Initial Catalog=Items;Provider=SQLNCLI.1;Persist Security Info=True;Auto Translate=False;\"" /set \package.variables[BatchID].Value;"\"1\"" /set \package.variables[SupplierID].Value;"\"22334\""

SET @ssisstr = ''dtexec /sq '' + @packagename + '' /ser '' + @servername + '' ''
SET @ssisstr = @ssisstr + @params
--print @ssisstr 

DECLARE @returncode int
EXEC @returncode = xp_cmdshell @ssisstr
SELECT
	@returncode

END
' 
END
GO


USE [Juno]
GO

/****** Object:  StoredProcedure [dbo].[SyncBlurbsLiveSSIS]    Script Date: 2013/09/23 03:22:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SyncBlurbsLiveSSIS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		DavidV
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[SyncBlurbsLiveSSIS] 
	
AS
BEGIN
	
	declare @ssisstr varchar(8000), @packagename varchar(200),@servername varchar(100),@params varchar(8000)
SET @packagename = ''SyncJunoBlurbs\SyncBlurbstoLive''
SET @servername = ''cns-e1dw''
SET @params = ''''

-- /set \package.variables[FileName].Value;"\"\\127.0.0.1\Common\SSIS\NewItem.xls\"" /set \package.variables[CreatedBy].Value;"\"Chirag\"" /set \package.variables[ContractDbConnectionString].Value;"\"Data Source=myserver\SQL2K5;User ID=sa;Password=sapass;Initial Catalog=Items;Provider=SQLNCLI.1;Persist Security Info=True;Auto Translate=False;\"" /set \package.variables[BatchID].Value;"\"1\"" /set \package.variables[SupplierID].Value;"\"22334\""

SET @ssisstr = ''dtexec /sq '' + @packagename + '' /ser '' + @servername + '' ''
SET @ssisstr = @ssisstr + @params
--print @ssisstr 

DECLARE @returncode int
EXEC @returncode = xp_cmdshell @ssisstr
SELECT
	@returncode

END
' 
END
GO


