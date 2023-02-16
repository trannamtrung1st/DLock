FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DLock.Identity.WebApi/DLock.Identity.WebApi.csproj", "DLock.Identity.WebApi/"]
RUN dotnet restore "DLock.Identity.WebApi/DLock.Identity.WebApi.csproj"
COPY . .
WORKDIR "/src/DLock.Identity.WebApi"
RUN dotnet build "DLock.Identity.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DLock.Identity.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD dotnet DLock.Identity.WebApi.dll