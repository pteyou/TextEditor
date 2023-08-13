FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Sources/CoreAppClient
COPY CoreAppClient.sln .
ADD Configuration/Configuration.csproj Configuration/
ADD CoreAppClient/CoreAppClient.csproj CoreAppClient/
RUN dotnet restore CoreAppClient.sln
ADD Protofiles/* Protofiles/
ADD Configuration/*.json Configuration/
ADD Configuration/*.cs Configuration/
COPY CoreAppClient/*.cs CoreAppClient/
RUN dotnet build CoreAppClient.sln -c Release -o /app/build

FROM build AS publish
RUN dotnet publish CoreAppClient.sln -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "CoreAppClient.dll" ]