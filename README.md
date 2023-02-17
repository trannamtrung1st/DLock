# DLock

## Prerequisites
Have knowledge of:
+ C#/.NET
+ ASP.NET
+ ReactJS
+ Docker

## System requirements
+ .NET 6 SDK
+ NodeJS (for React client app)
+ Docker

## Run instruction
### Local
1. Install .NET 6 SDK
2. Install NodeJS
3. Configure required appsettings.json values
4. Setup SQL Server and Redis:
   + You can use your existing SQL Server and Redis instances
   + Run `docker-compose up -d database redis` for running using Docker
5. Run 4 projects: `Booking, Field, Identity, WebProxy`
6. Run `dlock-client` React client app
### Docker
1. Install Docker desktop
2. Open command line at the repository root folder
3. Run `docker-compose up -d`
4. Test the app