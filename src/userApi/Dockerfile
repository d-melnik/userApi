#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/userApi/userApi.csproj", "src/userApi/"]
RUN dotnet restore "src/userApi/userApi.csproj"
COPY . .
WORKDIR "/src/src/userApi"
RUN dotnet build "userApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "userApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "userApi.dll"]