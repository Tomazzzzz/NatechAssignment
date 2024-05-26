param (
    [string]$serverInstance = ".\SQLEXPRESS",
    [string]$databaseName = "Natech",
    [string]$newUsername = "NatechUser",
    [string]$newUserPassword = "YourNatechUserPassword@123",
    [string]$saPassword = "YourSaPassword"
)

# Creating the connection string
$connectionString = "Server=$serverInstance;Database=master;User Id=sa;Password=$saPassword;"

# SQL query to create a new login and user, and grant access to the specified database
$query = @"
IF NOT EXISTS (SELECT name FROM master.sys.sql_logins WHERE name = N'$newUsername')
BEGIN
    CREATE LOGIN [$newUsername] WITH PASSWORD = N'$newUserPassword';
    PRINT 'Login [$newUsername] created successfully.'
END
ELSE
BEGIN
    PRINT 'Login [$newUsername] already exists.'
END

USE [$databaseName];

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = N'$newUsername')
BEGIN
    CREATE USER [$newUsername] FOR LOGIN [$newUsername];
    ALTER ROLE db_owner ADD MEMBER [$newUsername];
    PRINT 'User [$newUsername] created successfully and added to db_owner role in database [$databaseName].'
END
ELSE
BEGIN
    PRINT 'User [$newUsername] already exists in database [$databaseName].'
END
"@

# Executing the SQL query
try {
    Write-Output "Connecting to SQL Server using connection string: $connectionString"
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -ErrorAction Stop
    Write-Output "User '$newUsername' created and granted access to database '$databaseName' on server '$serverInstance'."
} catch {
    Write-Error "An error occurred: $_"
}
