FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

#COPY /tests/Ardalis.Specification.IntegrationTests/Ardalis.Specification.IntegrationTests.csproj ./
#RUN dotnet restore
#
COPY . ./
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
RUN chmod +x /wait

CMD /wait && dotnet test --logger trx --results-directory /var/temp
