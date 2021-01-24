FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /

COPY . ./
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
#RUN chmod +x /wait
RUN /bin/bash -c 'ls -la /wait; chmod +x /wait; ls -la /wait'

# install the report generator tool
RUN dotnet tool install dotnet-reportgenerator-globaltool --version 4.2.15 --tool-path /tools

CMD /wait && dotnet test --logger trx --results-directory /var/temp /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura && mv /ArdalisSpecification/tests/Ardalis.Specification.UnitTests/coverage.cobertura.xml /var/temp/coverage.unit.cobertura.xml && mv /ArdalisSpecificationEF/tests/Ardalis.Specification.EF.IntegrationTests/coverage.cobertura.xml /var/temp/coverage.ef.integration.cobertura.xml && tools/reportgenerator -reports:/var/temp/coverage.*.cobertura.xml -targetdir:/var/temp/coverage -reporttypes:HtmlInline_AzurePipelines\;HTMLChart\;Cobertura


