FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/consolidado.api/consolidado.api.csproj", "src/consolidado.api/"]
RUN dotnet restore "src/consolidado.api/consolidado.api.csproj"
COPY . .
WORKDIR "/src/src/consolidado.api"
RUN dotnet build "consolidado.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "consolidado.api.csproj" -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "consolidado.api.dll"]