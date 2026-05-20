FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Itransition Course Project.csproj", "."]
RUN dotnet restore "Itransition Course Project.csproj"
COPY . .

RUN dotnet publish "Itransition Course Project.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Itransition Course Project.dll"]
