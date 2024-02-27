FROM mcr.microsoft.com/dotnet/aspnet:8.0-cbl-mariner AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV TZ="America/Santo_Domingo"
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GestionCasos.Api/gestion_casos.Api.csproj", "GestionCasos.Api/"]
RUN dotnet restore --force "GestionCasos.Api/gestion_casos.Api.csproj"
COPY . .
WORKDIR "/src/GestionCasos.Api"
RUN dotnet build "gestion_casos.Api.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "gestion_casos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
ARG CONNECTION_STRING
ENV CONNECTION_STRING = CONNECTION_STRING
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gestion_casos.Api.dll"]