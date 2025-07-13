FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/FSIT.PayLink.Infrastructure/FSIT.PayLink.Infrastructure.csproj    FSIT.PayLink.Infrastructure/
COPY src/FSIT.PayLink.Application/FSIT.PayLink.Application.csproj          FSIT.PayLink.Application/
COPY src/FSIT.PayLink.Contracts/FSIT.PayLink.Contracts.csproj              FSIT.PayLink.Contracts/
COPY src/FSIT.PayLink.Domain/FSIT.PayLink.Domain.csproj                    FSIT.PayLink.Domain/
COPY src/FSIT.PayLink.Api/FSIT.PayLink.Api.csproj                          FSIT.PayLink.Api/
RUN dotnet restore FSIT.PayLink.Api/FSIT.PayLink.Api.csproj

COPY src/ FSIT.PayLink.Api/
WORKDIR /src/FSIT.PayLink.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "FSIT.PayLink.Api.dll"]
