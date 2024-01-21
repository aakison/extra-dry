-- Install an Identity database on a newly minted SQL Server instance.
-- Creates the database, schema, roles, logins, users, and grants.
-- This script is idempotent, so it can be run multiple times without harm.
-- The user password is expected to be set in the SQL variable 'IdentityAppPassword'.
-- Usage: sqlcmd -S localhost -i setup.sql -v IdentityAppPassword="MyPassword"

USE [Master]
GO

-- Create Database
IF DB_ID('SampleIdentity') IS NULL
BEGIN
    CREATE DATABASE [SampleIdentity]
END
GO

-- Create User
IF NOT EXISTS (select [Name] from sys.sql_logins where [Name] = 'IdentityApp') 
BEGIN
    CREATE LOGIN [IdentityApp] WITH PASSWORD = N'$(IdentityAppPassword)'
END
ELSE
BEGIN
    ALTER LOGIN [IdentityApp] WITH PASSWORD = N'$(IdentityAppPassword)'
END
GO

USE [SampleIdentity]
GO

-- Create Roles on Instance.
IF DATABASE_PRINCIPAL_ID('db_executor') IS NULL
BEGIN
    CREATE ROLE [db_executor]
    GRANT EXECUTE TO [db_executor]
END
GO

-- Add login to database
IF NOT EXISTS (select [Name] from sys.sysusers where [Name] = 'IdentityApp') BEGIN
    CREATE USER [IdentityApp] FROM LOGIN [IdentityApp]
END
GO

-- Add roles to login
ALTER ROLE [db_datareader] ADD MEMBER [IdentityApp]
ALTER ROLE [db_datawriter] ADD MEMBER [IdentityApp]
ALTER ROLE [db_executor] ADD MEMBER [IdentityApp]
GO
