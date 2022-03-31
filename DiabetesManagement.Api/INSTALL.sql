USE master

ALTER DATABASE DiabetesUnitsManager
SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

DROP DATABASE DiabetesUnitsManager
GO

CREATE DATABASE DiabetesUnitsManager
GO

USE DiabetesUnitsManager

CREATE TABLE [dbo].[INVENTORY] (
	[INVENTORYID] [uniqueidentifier] NOT NULL
		CONSTRAINT PK_INVENTORY PRIMARY KEY,
	[KEY] [nvarchar](64) NOT NULL,
	[DEFAULT_TYPE] [nvarchar](80) NULL,
	[USERID] [uniqueidentifier] NOT NULL,
	[HASH] [nvarchar](2000) NOT NULL,
	[CREATED] [DATETIMEOFFSET](7) NOT NULL,
	[MODIFIED] [DATETIMEOFFSET](7) NULL,
	CONSTRAINT UQ_INVENTORY UNIQUE ([KEY], [DEFAULT_TYPE], [USERID]),
	INDEX IX_INVENTORY NONCLUSTERED ([KEY], [USERID])
)


CREATE TABLE [dbo].[INVENTORY_HISTORY] (
	[INVENTORY_HISTORYID] [uniqueidentifier] NOT NULL
		CONSTRAINT PK_INVENTORY_HISTORY PRIMARY KEY,
	[INVENTORYID] [uniqueidentifier] NOT NULL
		CONSTRAINT FK_INVENTORY_HISTORY_INVENTORY
		REFERENCES [dbo].[INVENTORY],
	[VERSION] [int] NOT NULL,
	[TYPE] [nvarchar](80) NOT NULL,
	[ITEMS] [nvarchar](max) NOT NULL,
	[HASH] [nvarchar](2000) NOT NULL,
	[CREATED] [DATETIMEOFFSET](7) NOT NULL,
	CONSTRAINT UQ_INVENTORY_HISTORY UNIQUE ([INVENTORYID], [VERSION], [TYPE]),
	INDEX IX_INVENTORY_HISTORY NONCLUSTERED ([INVENTORYID], [VERSION], [TYPE])
) 


CREATE TABLE [dbo].[USER] (
	[USERID] [uniqueidentifier] NOT NULL
		CONSTRAINT PK_USER PRIMARY KEY,
	[EMAILADDRESS] [varchar](255) NULL
		CONSTRAINT UQ_USER UNIQUE,
	[DISPLAYNAME] [varchar](255) NULL
		CONSTRAINT UQ_USER_USERNAME UNIQUE,
	[PASSWORD] [varchar](255) NULL,
	[HASH] [nvarchar](2000) NOT NULL,
	[CREATED] [DATETIMEOFFSET](7) NOT NULL,
	[MODIFIED] [DATETIMEOFFSET](7) NULL
)

ALTER TABLE [dbo].[INVENTORY]
    ADD CONSTRAINT FK_INVENTORY_USER 
        FOREIGN KEY ([USERID])
        REFERENCES [dbo].[USER]
GO

CREATE TABLE [dbo].[API_TOKEN] (
    [API_TOKENID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_API_TOKEN PRIMARY KEY,
    [KEY] NVARCHAR(200) NOT NULL
        CONSTRAINT UQ_API_TOKEN UNIQUE,
    [SECRET] NVARCHAR(MAX) NOT NULL,
    [CREATED] [DATETIMEOFFSET](7) NOT NULL
)

CREATE TABLE [dbo].[API_TOKEN_REQUEST] (
	[API_TOKEN_REQUESTID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_API_TOKEN_REQUEST PRIMARY KEY,
	[API_TOKENID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_API_TOKEN_REQUEST
        REFERENCES [dbo].[API_TOKEN],
	[TOKEN] NVARCHAR(200) NOT NULL,
    [EXPIRES] [DATETIMEOFFSET](7) NOT NULL,
	[CREATED] [DATETIMEOFFSET](7) NOT NULL
	CONSTRAINT UQ_API_TOKEN_REQUEST UNIQUE ([TOKEN])
)

CREATE TABLE [dbo].[API_TOKEN_CLAIM] (
    [API_TOKEN_CLAIMID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_API_TOKEN_CLAIM PRIMARY KEY,
    [API_TOKENID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_API_TOKEN_CLAIM
        REFERENCES [dbo].[API_TOKEN],
    [CLAIM] NVARCHAR(200) NOT NULL,
    [CREATED] [DATETIMEOFFSET](7) NOT NULL
)

DELETE FROM [USER] WHERE [EMailADDRESS] = 'uLqgPyYCALsuOHXE7Zwm1N4m3mi8RwAlN2ZkDvrkPSo='