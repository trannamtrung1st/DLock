FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DLock.Field.WebApi/DLock.Field.WebApi.csproj", "DLock.Field.WebApi/"]
RUN dotnet restore "DLock.Field.WebApi/DLock.Field.WebApi.csproj"
COPY . .
WORKDIR "/src/DLock.Field.WebApi"
RUN dotnet build "DLock.Field.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DLock.Field.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD dotnet DLock.Field.WebApi.dll