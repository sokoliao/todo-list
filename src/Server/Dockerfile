FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs
WORKDIR /src
COPY . .
RUN dotnet restore "./BxTestTask.csproj"
RUN dotnet publish "./BxTestTask.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "BxTestTask.dll"]