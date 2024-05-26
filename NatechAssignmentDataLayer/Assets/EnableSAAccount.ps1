param (
    [string]$serverInstance = ".\SQLEXPRESS",
    [string]$saPassword = "secret"
)

try {
    # Import the SQLServer module for SQL Server commands
    Import-Module SqlServer -ErrorAction Stop

    # Creating the connection string
    $connectionString = "Server=$serverInstance;Database=master;Integrated Security=True;"

    # SQL query to enable the 'sa' login and set the password
    $query = @"
ALTER LOGIN [sa] WITH PASSWORD = N'$saPassword';
ALTER LOGIN [sa] ENABLE;
"@

    # Executing the SQL query
    Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -ErrorAction Stop

    Write-Output "The 'sa' user has been enabled and configured with the provided password on server '$serverInstance'."
} catch {
    Write-Error "An error occurred: $_"
}

# Verify if 'sa' account is enabled and password is set
try {
    $verifyQuery = "SELECT name, is_disabled FROM sys.sql_logins WHERE name = 'sa';"
    $result = Invoke-Sqlcmd -ConnectionString $connectionString -Query $verifyQuery -ErrorAction Stop

    if ($result.is_disabled -eq 0) {
        Write-Output "The 'sa' account is enabled."
    } else {
        Write-Error "The 'sa' account is still disabled."
    }
} catch {
    Write-Error "An error occurred during verification: $_"
}
