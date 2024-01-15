FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ParkingLotManager/ParkingLotManager.csproj", "ParkingLotManager/"]
RUN dotnet restore "ParkingLotManager/ParkingLotManager.csproj"
COPY . .
WORKDIR "/src/ParkingLotManager"
RUN dotnet build "ParkingLotManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParkingLotManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParkingLotManager.dll"]
