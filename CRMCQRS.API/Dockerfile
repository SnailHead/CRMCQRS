FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 10000
EXPOSE 10001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CRMCQRS.API/CRMCQRS.API.csproj", "CRMCQRS.API/"]
RUN dotnet restore "CRMCQRS.API/CRMCQRS.API.csproj"
COPY . .
WORKDIR "/src/CRMCQRS.API"
RUN dotnet build "CRMCQRS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRMCQRS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRMCQRS.API.dll"]
