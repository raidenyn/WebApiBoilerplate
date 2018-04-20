CREATE TABLE [dbo].[User]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[Login] NVARCHAR(50) NOT NULL UNIQUE, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [CreatedAt] DATETIME NOT NULL, 
    [RemovedAt] DATETIME NULL, 
    [PasswordHash] NVARCHAR(50) NULL, 
    [PasswordSolt] NVARCHAR(50) NULL
)
