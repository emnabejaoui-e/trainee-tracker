FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
RUN useradd -u 5678 -m -s /bin/bash appuser
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Trainee_Tracker.csproj", "./"]
RUN dotnet restore "Trainee_Tracker.csproj"
COPY . .
RUN dotnet publish "Trainee_Tracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
COPY --from=build --chown=appuser:appuser /app/publish .
USER appuser
ENTRYPOINT ["dotnet", "Trainee_Tracker.dll"]