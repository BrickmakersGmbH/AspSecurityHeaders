FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AspSecurityHeaders.Tool/AspSecurityHeaders.Tool.csproj", "AspSecurityHeaders.Tool/"]
RUN dotnet restore "AspSecurityHeaders.Tool/AspSecurityHeaders.Tool.csproj"
COPY . .
WORKDIR "/src/AspSecurityHeaders.Tool"
RUN dotnet build "AspSecurityHeaders.Tool.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspSecurityHeaders.Tool.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspSecurityHeaders.Tool.dll"]
