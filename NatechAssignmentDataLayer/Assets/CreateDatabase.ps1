param (
    [string]$serverInstance = ".\SQLEXPRESS",
    [string]$databaseName = "Natech",
    [string]$saPassword = "secret123456789@"
)

# Creating the connection string
$connectionString = "Server=$serverInstance;Database=master;User Id=sa;Password=$saPassword;"

# SQL query to create a new database
$query = @"
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'$databaseName')
BEGIN
    CREATE DATABASE [$databaseName]
    PRINT 'Database [$databaseName] created successfully on server [$serverInstance].'
END
ELSE
BEGIN
    PRINT 'Database [$databaseName] already exists on server [$serverInstance].'
END
"@

# Executing the SQL query
try {
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -ErrorAction Stop
} catch {
    Write-Error "An error occurred: $_"
}
