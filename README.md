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

### Ocelot API Gateway
http://localhost:5050/swagger/index.html

http://localhost:5050/book-api/book

http://localhost:5050/movie-api/movie


#### BookArchive API 
http://localhost:7000/swagger/index.html


#### MovieArchive API 
http://localhost:8000/swagger/index.html
