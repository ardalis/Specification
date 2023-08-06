FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /

COPY . ./

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.8.0/wait /wait
RUN /bin/bash -c 'ls -la /wait; chmod +x /wait; ls -la /wait'

# install the report generator tool
RUN dotnet tool install dotnet-reportgenerator-globaltool --version 5.1.23 --tool-path /tools

CMD /wait && \
dotnet build ci.slnf --configuration Release && \
dotnet test -f net7.0 ci.slnf --configuration Release --no-build --no-restore --collect:"xplat code coverage" && \
tools/reportgenerator -reports:Specification*/**/coverage.cobertura.xml -targetdir:/var/temp/TestResults -assemblyfilters:"-*Tests*;"
