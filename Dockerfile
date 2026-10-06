# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia o projeto
COPY ["API.csproj", "./"]

# Restaura dependências
RUN dotnet restore "API.csproj"

# Copia o restante do código
COPY . .

# Compila
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publica
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Porta utilizada pelo Render
ENV ASPNETCORE_HTTP_PORTS=10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]