CREATE DATABASE DiabetesUnitsManager
GO

USE DiabetesUnitsManager

CREATE TABLE [dbo].[INVENTORY] (
	[INVENTORYID] [uniqueidentifier] NOT NULL
		CONSTRAINT PK_INVENTORY PRIMARY KEY,
	[KEY] [nvarchar](64) NOT NULL,
	[DEFAULT_TYPE] [nvarchar](80) NULL,
	[USERID] [uniqueidentifier] NOT NULL,
	[HASH] [nvarchar](200) NOT NULL,
	[CREATED] [datetimeoffset](7) NOT NULL,
	[MODIFIED] [datetimeoffset](7) NULL,
	CONSTRAINT UQ_INVENTORY UNIQUE ([KEY], [USERID]),
	INDEX IX_INVENTORY NONCLUSTERED ([KEY], [USERID], [HASH])
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
	[HASH] [nvarchar](200) NOT NULL,
	[CREATED] [datetimeoffset](7) NOT NULL,
	CONSTRAINT UQ_INVENTORY_HISTORY UNIQUE ([INVENTORYID], [VERSION], [TYPE]),
	INDEX IX_INVENTORY_HISTORY NONCLUSTERED ([INVENTORYID], [VERSION], [TYPE], [HASH])
) 


CREATE TABLE [dbo].[USER] (
	[USERID] [uniqueidentifier] NOT NULL
		CONSTRAINT PK_USER PRIMARY KEY,
	[EMAILADDRESS] [varchar](255) NULL
		CONSTRAINT UQ_USER UNIQUE,
	[USERNAME] [varchar](255) NULL
		CONSTRAINT UQ_USER_USERNAME UNIQUE,
	[HASH] [nvarchar](200) NOT NULL,
	[CREATED] [datetimeoffset](7) NOT NULL,
	[MODIFIED] [datetimeoffset](7) NULL
)

ALTER TABLE [dbo].[INVENTORY]
    ADD CONSTRAINT FK_INVENTORY_USER 
        FOREIGN KEY ([USERID])
        REFERENCES [dbo].[USER]
GO
