FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG TARGETARCH
WORKDIR /app

RUN apt-get update && apt-get install -y wget fontconfig libfreetype6 libx11-6 libxext6 libxrender1 xfonts-75dpi xfonts-base libjpeg62-turbo \
    && rm -rf /var/lib/apt/lists/*
    # libpng16-16 \
    # libssl3 \

RUN wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6.1-3/wkhtmltox_0.12.6.1-3.bookworm_${TARGETARCH}.deb \
    && dpkg -i wkhtmltox_0.12.6.1-3.bookworm_${TARGETARCH}.deb \
    && rm wkhtmltox_0.12.6.1-3.bookworm_${TARGETARCH}.deb

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["pdf-poc.csproj", "."]
RUN dotnet restore "./pdf-poc.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./pdf-poc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./pdf-poc.csproj"1231312 -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pdf-poc.dll"]
123123123213