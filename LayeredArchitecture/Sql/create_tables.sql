--
-- DATABASE
--
IF DB_ID('$(dbname)') IS NULL
BEGIN
    CREATE DATABASE $(dbname)
END
GO

USE $(dbname)
GO 

-- Create History Schema:
IF NOT EXISTS ( SELECT  *
                FROM    sys.schemas
                WHERE   name = N'history' )
    EXEC('CREATE SCHEMA [history]');
GO

--
-- TABLES
--

IF  NOT EXISTS 
	(SELECT * FROM sys.objects 
	 WHERE object_id = OBJECT_ID(N'[history].[AddressHistory]') AND type in (N'U'))
	 
BEGIN

	CREATE TABLE [history].[AddressHistory](
        [AddressID] INT NOT NULL
        , [Name1] [NVARCHAR](255)
        , [Name2] [NVARCHAR](255)
        , [Street] [NVARCHAR](2000)
        , [ZipCode] [NVARCHAR](255)
        , [City] [NVARCHAR](255)
        , [Country] [NVARCHAR](255)
        , [AuditUser] [NVARCHAR](255) NOT NULL
        , [AuditOperation] [TINYINT] NOT NULL
        , [EntityVersion] [BIGINT]
        , [RowVersion] [ROWVERSION]
        , [SysStartTime] DATETIME2 NOT NULL
        , [SysEndTime] DATETIME2 NOT NULL
    );

END
GO

IF  NOT EXISTS 
	(SELECT * FROM sys.objects 
	 WHERE object_id = OBJECT_ID(N'[dbo].[Address]') AND type in (N'U'))
	 
BEGIN

	CREATE TABLE [dbo].[Address](
        [AddressID] INT NOT NULL IDENTITY PRIMARY KEY
        , [Name1] [NVARCHAR](255)
        , [Name2] [NVARCHAR](255)
        , [Street] [NVARCHAR](2000)
        , [ZipCode] [NVARCHAR](255)
        , [City] [NVARCHAR](255)
        , [Country] [NVARCHAR](255)
        , [AuditUser] [NVARCHAR](255) NOT NULL
        , [AuditOperation] [TINYINT] NOT NULL
        , [EntityVersion] [BIGINT]
        , [RowVersion] [ROWVERSION]
        , [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL
        , [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL
        , PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime)
        , CONSTRAINT CHK_Address_AuditOperation CHECK (AuditOperation in (1, 2, 3))
    ) 
    WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [history].[AddressHistory]));

END
GO

IF  NOT EXISTS 
	(SELECT * FROM sys.objects 
	 WHERE object_id = OBJECT_ID(N'[history].[PersonHistory]') AND type in (N'U'))
	 
BEGIN

	CREATE TABLE [history].[PersonHistory](
        [PersonID] INT NOT NULL
        , [FirstName] [NVARCHAR](255)
        , [LastName] [NVARCHAR](255)
        , [BirthDate] [DATETIME2](7)        
        , [AuditUser] [NVARCHAR](255) NOT NULL
        , [AuditOperation] [TINYINT] NOT NULL
        , [EntityVersion] [BIGINT]
        , [RowVersion] [ROWVERSION]
        , [SysStartTime] DATETIME2 NOT NULL
        , [SysEndTime] DATETIME2 NOT NULL
    );

END
GO


IF  NOT EXISTS 
	(SELECT * FROM sys.objects 
	 WHERE object_id = OBJECT_ID(N'[dbo].[Person]') AND type in (N'U'))
	 
BEGIN

	CREATE TABLE [dbo].[Person](
        [PersonID] INT NOT NULL IDENTITY PRIMARY KEY
        , [FirstName] [NVARCHAR](255)
        , [LastName] [NVARCHAR](255)
        , [BirthDate] [DATETIME2](7)        
        , [AuditUser] [NVARCHAR](255) NOT NULL
        , [AuditOperation] [TINYINT] NOT NULL
        , [EntityVersion] [BIGINT] DEFAULT 0
        , [RowVersion] [ROWVERSION]
        , [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL
        , [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL
        , PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime)
        , CONSTRAINT CHK_Person_AuditOperation CHECK (AuditOperation in (1, 2, 3))
    ) 
    WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [history].[PersonHistory]));

END
GO

IF  NOT EXISTS 
	(SELECT * FROM sys.objects 
	 WHERE object_id = OBJECT_ID(N'[history].[PersonAddressHistory]') AND type in (N'U'))
	 
BEGIN

	CREATE TABLE [history].[PersonAddressHistory](
        [PersonAddressID] INT NOT NULL
        , [PersonID] INT NOT NULL
        , [AddressID] INT NOT NULL
        , [ValidFrom] [DATETIME2](7)
        , [ValidUntil] [DATETIME2](7)     
        , [AuditUser] [NVARCHAR](255) NOT NULL
        , [AuditOperation] [TINYINT] NOT NULL
        , [EntityVersion] [BIGINT]
        , [RowVersion] [ROWVERSION]
        , [SysStartTime] DATETIME2 NOT NULL
        , [SysEndTime] DATETIME2 NOT NULL
    );

END
GO

IF  NOT EXISTS 
	(SELECT * FROM sys.objects 
	 WHERE object_id = OBJECT_ID(N'[dbo].[PersonAddress]') AND type in (N'U'))
	 
BEGIN

	CREATE TABLE [dbo].[PersonAddress](
        [PersonAddressID] INT NOT NULL IDENTITY PRIMARY KEY  
        , [PersonID] INT NOT NULL
        , [AddressID] INT NOT NULL
        , [ValidFrom] [DATETIME2](7)
        , [ValidUntil] [DATETIME2](7)
        , [AuditUser] [NVARCHAR](255) NOT NULL
        , [AuditOperation] [TINYINT] NOT NULL
        , [EntityVersion] [BIGINT] DEFAULT 0
        , [RowVersion] [ROWVERSION]
        , [SysStartTime] DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL
        , [SysEndTime] DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL
        , PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime)
        , CONSTRAINT FK_PersonAddress_Address FOREIGN KEY (AddressID)
          REFERENCES dbo.Address (AddressID)
        , CONSTRAINT FK_PersonAddress_Person FOREIGN KEY (PersonID)
          REFERENCES dbo.Person (PersonID)
        , CONSTRAINT CHK_PersonAddress_AuditOperation CHECK (AuditOperation in (1, 2, 3))
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [history].[PersonAddressHistory]))

END
GO
