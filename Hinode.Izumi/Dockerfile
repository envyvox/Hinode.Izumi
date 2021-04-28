FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Copy everything else and build
COPY Hinode.Izumi ./Hinode.Izumi
COPY Hinode.Izumi.Commands ./Hinode.Izumi.Commands
COPY Hinode.Izumi.Data ./Hinode.Izumi.Data
COPY Hinode.Izumi.Framework ./Hinode.Izumi.Framework
COPY Hinode.Izumi.Services ./Hinode.Izumi.Services

COPY nuget.config ./
RUN dotnet restore Hinode.Izumi/Hinode.Izumi.csproj --configfile ./nuget.config
RUN dotnet publish Hinode.Izumi/*.csproj -c Release -o out

FROM node:12.2.0 AS ng
WORKDIR /ClientApp
COPY --from=build-env /Hinode.Izumi/ClientApp .
RUN npm i
RUN npm i -g @angular/cli@11.2.5
RUN ng build --prod

# Build runtime image
WORKDIR /
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build-env ./out .
COPY --from=ng /ClientApp/dist /ClientApp/dist
ENTRYPOINT ["dotnet", "Hinode.Izumi.dll"]
