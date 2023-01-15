#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProcessorWorkerService.csproj", "."]
RUN dotnet restore "./ProcessorWorkerService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ProcessorWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessorWorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessorWorkerService.dll"]