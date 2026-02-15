FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PD.UI/PD.UI.csproj", "PD.UI/"]
COPY ["PD.BL/PD.BL.csproj", "PD.BL/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Dtos/Dtos.csproj", "Dtos/"]
COPY ["PD.DAL/PD.DAL.csproj", "PD.DAL/"]
RUN dotnet restore "PD.UI/PD.UI.csproj"
COPY . .
WORKDIR "/src/PD.UI"
RUN dotnet build "./PD.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PD.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PD.UI.dll"]
