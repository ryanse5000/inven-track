# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first so package downloads are cached between builds
COPY InvenTrack.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---- Run ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# The app runs as a non-root user, so give it a folder it can write the database to
RUN mkdir -p /app/data && chown $APP_UID /app/data
USER $APP_UID

# Render forwards traffic to port 10000 and handles HTTPS in front of the app
ENV ASPNETCORE_HTTP_PORTS=10000 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true \
    ConnectionStrings__Inventory="Data Source=/app/data/inventory.db"
EXPOSE 10000

ENTRYPOINT ["dotnet", "InvenTrack.dll"]
