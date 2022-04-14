DROP TABLE [SESSION]
DROP TABLE INVENTORY_HISTORY
DROP TABLE INVENTORY
DROP TABLE [USER]


CREATE TABLE [dbo].[User] (
      [UserId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_USER PRIMARY KEY
     ,[DisplayName] VARCHAR(200) NULL
     ,[DisplayName_CS] VARCHAR(200) NULL
     ,[EmailAddress] VARCHAR(200) NOT NULL
     ,[EmailAddress_CS] VARCHAR(200) NOT NULL
     ,[Password] VARCHAR(200) NOT NULL
     ,[Verified] BIT NOT NULL
     ,[Hash] VARCHAR(MAX) NOT NULL
     ,[Created] DATETIMEOFFSET NOT NULL
     ,[Modified] DATETIMEOFFSET NULL
     ,CONSTRAINT UQ_USER UNIQUE ([DisplayName],[EmailAddress])
     ,INDEX IX_USER NONCLUSTERED ([DisplayName],[EmailAddress],[Password])
)

CREATE TABLE [dbo].[Session] (
     [SessionId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_SESSION PRIMARY KEY
    ,[UserId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_Session_User
        REFERENCES [dbo].[User]
    ,[Enabled] BIT NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Expires] DATETIMEOFFSET NULL
)

CREATE TABLE [dbo].[Inventory] (
     [InventoryId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_INVENTORY PRIMARY KEY
    ,[UserId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_Inventory_User
        REFERENCES [dbo].[User]
    ,[Subject] NVARCHAR(200) NOT NULL
    ,[DefaultIntent] NVARCHAR(200) NOT NULL
    ,[Hash] VARCHAR(MAX) NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Modified] DATETIMEOFFSET NULL
    ,CONSTRAINT UQ_INVENTORY UNIQUE ([UserId],[Subject],[DefaultIntent])
    ,INDEX IX_INVENTORY NONCLUSTERED ([UserId],[Subject],[DefaultIntent])
)

CREATE TABLE [dbo].[Inventory_History] (
     [Inventory_HistoryId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_INVENTORY_HISTORY PRIMARY KEY
    ,[InventoryId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_InventoryHistory_Inventory
        REFERENCES [dbo].[Inventory]
    ,[Intent] NVARCHAR(200) NOT NULL
    ,[Value] NVARCHAR(MAX) NOT NULL
    ,[Version] INT NOT NULL
    ,[Hash] VARCHAR(MAX) NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,CONSTRAINT UQ_INVENTORY_HISTORY UNIQUE ([InventoryId],[Intent],[Version])
    ,INDEX IX_INVENTORY_HISTORY NONCLUSTERED ([InventoryId],[Intent],[Version])
)