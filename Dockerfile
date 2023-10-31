FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
ARG VERSION=1.0.0.0
ARG ARTIFACTORY_USERNAME
ARG ARTIFACTORY_PASSWORD
ENV ENV_ARTIFACTORY_USERNAME=$ARTIFACTORY_USERNAME
ENV ENV_ARTIFACTORY_PASSWORD=$ARTIFACTORY_PASSWORD

# Copy everything
COPY . ./
RUN dotnet clean
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out -p:Version=$VERSION

# Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "micm-resea-initial-1-on-1-event-job.dll"]

EXPOSE 8080
EXPOSE 9090