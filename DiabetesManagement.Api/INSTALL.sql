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
	[CREATED] [datetimeoffset](7) NOT NULL,
	[MODIFIED] [datetimeoffset](7) NULL,
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
	[CREATED] [datetimeoffset](7) NOT NULL,
	CONSTRAINT UQ_INVENTORY_HISTORY UNIQUE ([INVENTORYID], [VERSION], [TYPE]),
	INDEX IX_INVENTORY_HISTORY NONCLUSTERED ([INVENTORYID], [VERSION], [TYPE])
) 


CREATE TABLE [dbo].[USER] (
	[USERID] [uniqueidentifier] NOT NULL
		CONSTRAINT PK_USER PRIMARY KEY,
	[EMAILADDRESS] [varchar](255) NULL
		CONSTRAINT UQ_USER UNIQUE,
	[USERNAME] [varchar](255) NULL
		CONSTRAINT UQ_USER_USERNAME UNIQUE,
	[HASH] [nvarchar](2000) NOT NULL,
	[CREATED] [datetimeoffset](7) NOT NULL,
	[MODIFIED] [datetimeoffset](7) NULL
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
    [CREATED] [datetimeoffset](7) NOT NULL
)

CREATE TABLE [dbo].[API_TOKEN_CLAIM] (
    [API_TOKEN_CLAIMID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_API_TOKEN_CLAIM PRIMARY KEY,
    [API_TOKENID] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_API_TOKEN_CLAIM
        REFERENCES [dbo].[API_TOKEN],
    [CLAIM] NVARCHAR(200) NOT NULL,
    [CREATED] [datetimeoffset](7) NOT NULL
)

select * FROM [INVENTORY]


SELECT TOP(1) [I].[INVENTORYID], [I].[KEY], [I].[USERID],
                    [I].[HASH], [I].[CREATED], [I].[MODIFIED], [I].[DEFAULT_TYPE] [DefaultType],
                    [IH].[INVENTORY_HISTORYID] [InventoryHistoryId], [IH].[VERSION],
                    [IH].[ITEMS], [IH].[TYPE], [IH].[HASH] [InventoryHistoryHash], 
                    [IH].[CREATED] [InventoryHistoryCreated]
                FROM [dbo].[INVENTORY_HISTORY] [IH]
                INNER JOIN [dbo].[INVENTORY][I]
                ON [IH].[INVENTORYID] = [I].[INVENTORYID]