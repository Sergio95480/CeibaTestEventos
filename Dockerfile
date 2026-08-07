FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "src/CeibaTestEventos.Api/CeibaTestEventos.Api.csproj"

RUN dotnet publish "src/CeibaTestEventos.Api/CeibaTestEventos.Api.csproj" \
    -c Release \
    -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CeibaTestEventos.Api.dll"]