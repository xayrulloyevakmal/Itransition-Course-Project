FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Itransition Course Project.csproj", "."]
RUN dotnet restore
COPY . .

# Build va Migratsiya toolini ichkarida o'rnatamiz
RUN dotnet tool install --local dotnet-ef --version 8.0.11
RUN dotnet ef migrations add InitialPostgres --context ApplicationDbContext

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Itransition Course Project.dll"]
