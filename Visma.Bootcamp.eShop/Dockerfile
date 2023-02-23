#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Visma.Bootcamp.eShop/Visma.Bootcamp.eShop.csproj", "Visma.Bootcamp.eShop/"]
COPY ["Visma.Bootcamp.eShop.ApplicationCore/Visma.Bootcamp.eShop.ApplicationCore.csproj", "Visma.Bootcamp.eShop.ApplicationCore/"]
RUN dotnet restore "Visma.Bootcamp.eShop/Visma.Bootcamp.eShop.csproj"
COPY . .
WORKDIR "/src/Visma.Bootcamp.eShop"
RUN dotnet build "Visma.Bootcamp.eShop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Visma.Bootcamp.eShop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Visma.Bootcamp.eShop.dll"]