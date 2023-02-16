FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DLock.Booking.WebApi/DLock.Booking.WebApi.csproj", "DLock.Booking.WebApi/"]
RUN dotnet restore "DLock.Booking.WebApi/DLock.Booking.WebApi.csproj"
COPY . .
WORKDIR "/src/DLock.Booking.WebApi"
RUN dotnet build "DLock.Booking.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DLock.Booking.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD dotnet DLock.Booking.WebApi.dll