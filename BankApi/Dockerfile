FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .

RUN dotnet test --logger html --results-directory tests

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
COPY --from=build /app/tests ./tests

VOLUME /app/tests

EXPOSE 8080
ENTRYPOINT ["dotnet", "BankApi.dll"]
