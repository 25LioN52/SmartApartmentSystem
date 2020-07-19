#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
#RUN apt-get update -yq \
    #&& apt-get install curl gnupg -yq \
    #&& curl -sL https://deb.nodesource.com/setup_10.x | bash \
    #&& apt-get install nodejs -yq
#WORKDIR /app
#EXPOSE 80
#
ARG NODE_IMAGE=node:latest

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
#COPY *.sln .
#COPY ./src/ ./src/
#RUN dotnet restore
COPY . .
WORKDIR "/src/."
RUN dotnet build "src/SmartApartmentSystem.WebUI/SmartApartmentSystem.API.csproj" -c Release -o /app/build -r linux-arm

FROM build AS publish
RUN dotnet publish "src/SmartApartmentSystem.WebUI/SmartApartmentSystem.API.csproj" -c Release -o /app/publish  -r linux-arm

FROM ${NODE_IMAGE} as node-build
WORKDIR /src
COPY src/SmartApartmentSystem.WebUI/ClientApp .
RUN npm install
RUN npm rebuild node-sass
RUN npm run build -- --prod

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim-arm32v7 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=node-build /src/www ./ClientApp/dist
ENTRYPOINT ["dotnet", "SmartApartmentSystem.API.dll"]

#docker build "C:\Repos\SmartApartmentSystem" -t 25lion52/sas:latest
#docker push 25lion52/sas
#docker run -v /home/pi/sas:/local-db --privileged --restart unless-stopped -p 8700:80 -e TZ=Europe/Kiev 25lion52/sas
