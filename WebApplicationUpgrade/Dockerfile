FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["WebApplicationUpgrade.csproj", "./"]

RUN dotnet restore "WebApplicationUpgrade.csproj"

COPY . .

RUN dotnet publish "WebApplicationUpgrade.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "WebApplicationUpgrade.dll"]