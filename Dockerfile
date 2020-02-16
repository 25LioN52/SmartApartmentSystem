FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["SmartApartmentSystem.csproj", ""]
RUN dotnet restore "./SmartApartmentSystem.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SmartApartmentSystem.csproj" -c Release -o /app/build  -r linux-arm

FROM build AS publish
RUN dotnet publish "SmartApartmentSystem.csproj" -c Release -o /app/publish  -r linux-arm

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim-arm32v7 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartApartmentSystem.dll"]

#docker build "E:\Repos\SmartApartmentSystem" -t 25lion52/sas:latest
#docker push 25lion52/sas
#docker run --privileged -p 8700:80 25lion52/sas
