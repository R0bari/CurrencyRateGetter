FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Src/WebAPI/*.csproj", "WebAPI/"]
COPY ["Src/DataBase/*.csproj", "DataBase/"]
COPY ["Src/Domain/*.csproj", "Domain/"]
COPY ["Src/DomainServices/*.csproj", "DomainServices/"]
RUN dotnet restore "WebAPI/WebAPI.csproj"
WORKDIR "/src/WebAPI"
COPY ["Src/WebAPI/", "./"]
COPY ["Src/DataBase/", "./"]
COPY ["Src/Domain/", "./"]
COPY ["Src/DomainServices/", "./"]
RUN dotnet build WebAPI.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
