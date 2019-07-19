FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

COPY . ./
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
#RUN chmod +x /wait
RUN /bin/bash -c 'ls -la /wait; chmod +x /wait; ls -la /wait'

CMD /wait && dotnet test --logger trx --results-directory /var/temp /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura && mv /app/tests/Ardalis.Specification.UnitTests/coverage.cobertura.xml /var/temp/coverage.unit.cobertura.xml && mv /app/tests/Ardalis.Specification.IntegrationTests/coverage.cobertura.xml /var/temp/coverage.integration.cobertura.xml
