FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BackendLi/BackendLi.csproj", "BackendLi/"]
RUN dotnet restore "BackendLi/BackendLi.csproj"
COPY . .
WORKDIR "/src/BackendLi"
RUN dotnet build "BackendLi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BackendLi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 7290
EXPOSE 5236
CMD ["dotnet", "BackendLi.dll"]