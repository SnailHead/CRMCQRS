FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CRMCQRS.Identity/CRMCQRS.Identity.csproj", "CRMCQRS.Identity/"]
RUN dotnet restore "CRMCQRS.Identity/CRMCQRS.Identity.csproj"
COPY . .
WORKDIR "/src/CRMCQRS.Identity"
RUN dotnet build "CRMCQRS.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRMCQRS.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRMCQRS.Identity.dll"]
