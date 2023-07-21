FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /

COPY . ./
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.8.0/wait /wait
#RUN chmod +x /wait
RUN /bin/bash -c 'ls -la /wait; chmod +x /wait; ls -la /wait'

# install the report generator tool
RUN dotnet tool install dotnet-reportgenerator-globaltool --version 5.1.23 --tool-path /tools

CMD /wait && dotnet test -f net7.0 Specification/tests/Ardalis.Specification.UnitTests/Ardalis.Specification.UnitTests.csproj --collect:"XPlat Code Coverage" && dotnet test -f net7.0 Specification.EntityFrameworkCore/tests/Ardalis.Specification.EntityFrameworkCore.IntegrationTests/Ardalis.Specification.EntityFrameworkCore.IntegrationTests.csproj --collect:"XPlat Code Coverage" && tools/reportgenerator -reports:Specification*/**/coverage.cobertura.xml -targetdir:/var/temp/coverage -reporttypes:HtmlInline_AzurePipelines\;HTMLChart\;Cobertura
