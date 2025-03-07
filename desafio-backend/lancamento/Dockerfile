FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/lancamento.api/lancamento.api.csproj", "src/lancamento.api/"]
RUN dotnet restore "src/lancamento.api/lancamento.api.csproj"
COPY . .
WORKDIR "/src/src/lancamento.api"
RUN dotnet build "lancamento.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lancamento.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lancamento.api.dll"]