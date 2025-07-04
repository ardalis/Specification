#!/bin/bash

dotnet tool list -g dotnet-reportgenerator-globaltool > /dev/null 2>&1
exists=$(echo $?)
if [ $exists -ne 0 ]; then
  echo "Installing ReportGenerator..."
dotnet tool install -g dotnet-reportgenerator-globaltool
  echo "ReportGenerator installed"
fi

find . -type d -name TestResults -exec rm -rf {} \; > /dev/null 2>&1

testtarget="$1"

if [ "$testtarget" = "" ]; then
testtarget="ci.slnf"
fi

dotnet build $testtarget --configuration Release
dotnet test $testtarget --configuration Release --no-build --no-restore --framework net9.0 --collect:"XPlat Code Coverage;Format=opencover"

reportgenerator \
    -reports:tests/**/coverage.opencover.xml \
    -targetdir:TestResults \
    -reporttypes:"Html;Badges;MarkdownSummaryGithub" \
    -assemblyfilters:-*Tests*
