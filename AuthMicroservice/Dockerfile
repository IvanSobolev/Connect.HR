FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthMicroservice/AuthMicroservice.csproj", "AuthMicroservice/"]
RUN dotnet restore "AuthMicroservice/AuthMicroservice.csproj"
COPY . .
WORKDIR "/src/AuthMicroservice"
RUN dotnet build "AuthMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS migrate
RUN dotnet ef database update

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthMicroservice.dll"]