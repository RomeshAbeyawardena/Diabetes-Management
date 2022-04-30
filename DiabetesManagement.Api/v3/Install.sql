ALTER DATABASE InventoryDb
SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE master
DROP DATABASE InventoryDb
GO

CREATE DATABASE InventoryDb
GO

USE InventoryDb

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

CREATE TABLE [dbo].[Application] (
     [ApplicationId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_APPLICATION PRIMARY KEY
    ,[UserId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_Application_User
        REFERENCES [dbo].[User]
    ,[Name] VARCHAR(200) NOT NULL
    ,[Name_CS] VARCHAR(200) NOT NULL
    ,[DisplayName] VARCHAR(200) NOT NULL
    ,[DisplayName_CS] VARCHAR(200) NOT NULL
    ,[Intent] VARCHAR(200) NOT NULL
    ,[Enabled] BIT NOT NULL
    ,[Hash] VARCHAR(MAX) NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Modified] DATETIMEOFFSET NULL
    ,[Expires] DATETIMEOFFSET NULL
    ,CONSTRAINT UQ_Application UNIQUE ([UserId],[Name],[Intent])
    ,INDEX IX_Application NONCLUSTERED ([UserId],[Name],[Intent])
)

CREATE TABLE [dbo].[ApplicationInstance] (
    [ApplicationInstanceId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_APPLICATIONINSTANCE PRIMARY KEY
    ,[ApplicationId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_ApplicationInstance_Application
        REFERENCES [dbo].[Application]
    ,[AccessToken] VARCHAR(MAX) NOT NULL
    ,[Enabled] BIT NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Expires] DATETIMEOFFSET NULL
)

CREATE TABLE [dbo].[AccessToken] (
     [AccessTokenId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_ACCESSTOKEN PRIMARY KEY
    ,[ApplicationId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_AccessToken_Application
        REFERENCES [dbo].[Application]
    ,[Key] VARCHAR(200) NOT NULL
    ,[Key_CS] VARCHAR(200) NOT NULL
    ,[Value] VARCHAR(200) NOT NULL
    ,[Enabled] BIT NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Expires] DATETIMEOFFSET NULL
    ,CONSTRAINT UQ_AccessToken UNIQUE ([ApplicationId],[Key],[Value])
    ,INDEX IX_AccessToken NONCLUSTERED ([ApplicationId],[Key],[Value])
)

CREATE TABLE [dbo].[AccessToken_Claim] (
     [AccessToken_ClaimId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_AccessToken_Claim PRIMARY KEY
    ,[AccessTokenId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_AccessToken_Claim_AccessToken
        REFERENCES [dbo].[AccessToken]
    ,[Claim] VARCHAR(200) NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Expires] DATETIMEOFFSET NULL
    ,CONSTRAINT UQ_AccessToken_Claim UNIQUE ([AccessTokenId],[Claim])
    ,INDEX IX_AccessToken_Claim NONCLUSTERED ([AccessTokenId],[Claim])
)

CREATE TABLE [dbo].[Session] (
     [SessionId]  UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_SESSION PRIMARY KEY
    ,[UserId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT FK_Session_User
        REFERENCES [dbo].[User]
    ,[AccessToken] VARCHAR(400) NOT NULL
    ,[Enabled] BIT NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Expires] DATETIMEOFFSET NULL
    ,CONSTRAINT UQ_Session UNIQUE ([UserId], [AccessToken])
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

CREATE TABLE [dbo].[Function] (
     [FunctionId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Function PRIMARY KEY
    ,[Name] VARCHAR(200) NOT NULL
    ,[Name_CS] VARCHAR(200) NOT NULL
    ,[Path] VARCHAR(512) NOT NULL
    ,[Path_CS] VARCHAR(512) NOT NULL
    ,[Complexity] DECIMAL NULL
    ,[AccessToken] VARCHAR(MAX) NOT NULL
    ,[Enabled] BIT NOT NULL
    ,[Created] DATETIMEOFFSET NOT NULL
    ,[Modified] DATETIMEOFFSET NULL
    ,CONSTRAINT UQ_Function UNIQUE ([Name], [Path])
    ,INDEX IX_Function_Name NONCLUSTERED ([Name], [Path])
    ,INDEX IX_Function_Name_Path NONCLUSTERED ([Name], [Path])
)

select * FROM [user]
select * FROM [session]
select * FROM [application]
select * FROM [applicationinstance]
select * FROM [accesstoken]
select * FROM [function]

