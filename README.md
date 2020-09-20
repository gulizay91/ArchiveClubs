# ArchiveClubs
Book, Movie, TVSeries, Game lists



### IdentityServer Migration
make sure Default Project IdentityServer.API

PackageManagerConsole => Default Project: IdentityServer.Infrastructure

run in order
```sh
Add-Migration init -context AppIdentityDbContext
Update-Database -context AppIdentityDbContext

Add-Migration init -context AppPersistedGrantDbContext
Update-Database -context AppPersistedGrantDbContext

Add-Migration init -context AppConfigurationDbContext
Update-Database -context AppConfigurationDbContext
```
