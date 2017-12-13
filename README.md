# bjk-net-core-api-boilerplate
This is a simple ASP.NET Core 2 web api boilerplate project. It has Entity Framework Core, NLog, and JWT token authentication 
already implemented, so you can get up and running fast on a new project.

## Quickstart

1. Clone or download the project and open it in Visual Studio.
2. Edit `appsettings.json` to set the connection string to match your database information.
```
...
  "Data": {
    "ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestApplication;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
  },
...
```
3. Open the Visual Studio Package Manager Console, and execute `Add-Migration` and then `Update-Database`. Your database will be updated with the tables needed for ASP.NET Core 2 identity, and you're ready to get off and running.
 
## Test It's Working

1. Run the application in Visual Studio.
2. An unmodified request to `/api/values` should return a `401` status code, as the user is currently unauthorized.
3. Register a new user with the following request, and verify you get a `200 OK` response.
```
POST /api/auth/register HTTP/1.1
Host: {baseUrl}
Content-Type: application/json

{
  "Email": "example@example.com",
  "Password": "my-secure-password"
}
```
4. Log the user in with the following request:
```
POST /api/auth/token HTTP/1.1
Host: {baseUrl}
Content-Type: application/json

{
  "Username": "example@example.com",
  "Password": "my-secure-password"
}
```
The response should look similar to the following:
```
HTTP/1.1 200 OK
Content-Type: application/json

{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicm9zYWxpbmQubS53aWxsc0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFkMDQ5Yjg3LTY1OWUtNDQwNi04NzllLWE4MTAyN2I3ZDc2YyIsInN1YiI6InJvc2FsaW5kLm0ud2lsbHNAZ21haWwuY29tIiwianRpIjoiMThmOTAyNWMtZDBjNC00Mzc5LThiMDMtYjBmMjgyYzZiZjJiIiwiZW1haWwiOiJyb3NhbGluZC5tLndpbGxzQGdtYWlsLmNvbSIsImV4cCI6MTUxMzIwNTQ5NiwiaXNzIjoiaHR0cDovL3d3dy5leGFtcGxlLmNvbSIsImF1ZCI6Imh0dHA6Ly93d3cuZXhhbXBsZS5jb20ifQ.JMkAIf_0Zl7KA97Ya-wItDom6L2iBk_STYcjMadiIhM",
    "expiration": "2017-12-13T22:51:36Z"
}
```
5. Use the token from your login response as a bearer token in a new request to `ValuesController`.
```
GET /api/values HTTP/1.1
Host: localhost:53665
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicm9zYWxpbmQubS53aWxsc0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFkMDQ5Yjg3LTY1OWUtNDQwNi04NzllLWE4MTAyN2I3ZDc2YyIsInN1YiI6InJvc2FsaW5kLm0ud2lsbHNAZ21haWwuY29tIiwianRpIjoiMThmOTAyNWMtZDBjNC00Mzc5LThiMDMtYjBmMjgyYzZiZjJiIiwiZW1haWwiOiJyb3NhbGluZC5tLndpbGxzQGdtYWlsLmNvbSIsImV4cCI6MTUxMzIwNTQ5NiwiaXNzIjoiaHR0cDovL3d3dy5leGFtcGxlLmNvbSIsImF1ZCI6Imh0dHA6Ly93d3cuZXhhbXBsZS5jb20ifQ.JMkAIf_0Zl7KA97Ya-wItDom6L2iBk_STYcjMadiIhM
Content-Type: application/json
```
6. Verify that logs are being recorded to `dbo.Log` in your database.
 
 ## Configurable Settings
 
 1. By default, this project's NLog configuration stores data in a table in your database called `dbo.Log` and stores the application's name as `TestApplication`. NLog configuration can be updated in the `nlog.config` file in the root of the project.
 2. The `appsettings.json` file contains default placeholder values for allowed CORS urls and JWT token key/issuer/audience. These can be updated easily at any time.
 3. When the app builds its hosting environment at runtime, it runs a `SeedDatabase()` method in `Program.cs`. This method is a home for any database seeding that must be run when the app starts. By default, this project includes one seeder, `RoleInitializer::Seed()`, which 
 checks for the existence of the identity roles "Admin" and "User", and creates them if they don't exist.
 3. All controllers derived from `BaseController` get a `GlobalExceptionHandler` filter for free, which contains an `OnException` method for any logic that should run on unhandled exceptions. By default, this project uses NLog to log data about the exception to the database.
