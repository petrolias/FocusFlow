# Use the official .NET SDK image for building the Web API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files and restore dependencies
COPY FocusFlow.sln .
COPY FocusFlow.WebApi/FocusFlow.WebApi.csproj ./FocusFlow.WebApi/
COPY FocusFlow.Core/FocusFlow.Core.csproj ./FocusFlow.Core/
COPY FocusFlow.Abstractions/FocusFlow.Abstractions.csproj ./FocusFlow.Abstractions/
# Copy full source
COPY ../ .

# Restore and publish
#RUN dotnet restore ./FocusFlow.WebApi/FocusFlow.WebApi.csproj
RUN dotnet publish ./FocusFlow.WebApi/FocusFlow.WebApi.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
EXPOSE 443
#ENV ASPNETCORE_URLS=https://+:443
ENV ASPNETCORE_URLS=http://+:5000;
ENTRYPOINT ["dotnet", "FocusFlow.WebApi.dll"]