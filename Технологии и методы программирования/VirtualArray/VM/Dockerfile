FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VM/VM.csproj", "VM/"]
RUN dotnet restore "VM/VM.csproj"
COPY . .
WORKDIR "/src/VM"
RUN dotnet build "VM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VM.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VM.dll"]
