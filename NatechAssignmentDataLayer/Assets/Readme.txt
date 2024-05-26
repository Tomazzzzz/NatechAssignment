After MSSQL Express installation enable sa account:
.\EnableSAAccount.ps1 -serverInstance ".\SQLEXPRESS" -saPassword "secret123456789@"

Maybe a nuget package is needed:
Install-Module -Name SqlServer -AllowClobber -Force

https://www.getfishtank.com/blog/login-failed-error-18456



Create DB Natech

.\CreateDatabase.ps1 -serverInstance ".\SQLEXPRESS" -databaseName "Natech" -saPassword "secret123456789@"

Create user NatechUser

.\CreateNatechUser.ps1 -serverInstance ".\SQLEXPRESS" -databaseName "Natech" -newUsername "NatechUser" -newUserPassword "NatechSecret@123" -saPassword "secret123456789@"
