FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["SmartApartmentSystem/SmartApartmentSystem.csproj", "SmartApartmentSystem/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Queries/Queries.csproj", "Queries/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Services/Commands.csproj", "Services/"]
RUN dotnet restore "SmartApartmentSystem/SmartApartmentSystem.csproj"
COPY . .
WORKDIR "/src/SmartApartmentSystem"
RUN dotnet build "SmartApartmentSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartApartmentSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartApartmentSystem.dll"]