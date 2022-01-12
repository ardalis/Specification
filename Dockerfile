FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /

COPY . ./
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.8.0/wait /wait
#RUN chmod +x /wait
RUN /bin/bash -c 'ls -la /wait; chmod +x /wait; ls -la /wait'

# install the report generator tool
RUN dotnet tool install dotnet-reportgenerator-globaltool --version 4.8.7 --tool-path /tools

CMD /wait && dotnet test -f net5.0 Specification/tests/Ardalis.Specification.UnitTests/Ardalis.Specification.UnitTests.csproj --logger trx --results-directory /var/temp /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura && mv /Specification/tests/Ardalis.Specification.UnitTests/coverage.net5.0.cobertura.xml /var/temp/coverage.unit.cobertura.xml && dotnet test Specification.EntityFrameworkCore/tests/Ardalis.Specification.EntityFrameworkCore.IntegrationTests/Ardalis.Specification.EntityFrameworkCore.IntegrationTests.csproj --logger trx --results-directory /var/temp /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura && mv /Specification.EntityFrameworkCore/tests/Ardalis.Specification.EntityFrameworkCore.IntegrationTests/coverage.cobertura.xml /var/temp/coverage.ef.integration.cobertura.xml && tools/reportgenerator -reports:/var/temp/coverage.*.cobertura.xml -targetdir:/var/temp/coverage -reporttypes:HtmlInline_AzurePipelines\;HTMLChart\;Cobertura