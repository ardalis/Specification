#!/bin/bash
# Fati Iseni

WorkingDir="$(pwd)"

########## Make sure you're not on a root path :)
safetyCheck()
{
  declare -a arr=("" "/" "/c" "/d" "c:\\" "d:\\" "C:\\" "D:\\")
  for i in "${arr[@]}"
  do
    if [ "$WorkingDir" = "$i" ]; then
      echo "";
      echo "You are on a root path. Please run the script from a given directory.";
      exit 1;
    fi
  done
}

deleteBinObj()
{
  echo "Deleting bin and obj directories...";
  find "$WorkingDir/" -type d -name "bin" -exec rm -rf {} \; > /dev/null 2>&1;
  find "$WorkingDir/" -type d -name "obj" -exec rm -rf {} \; > /dev/null 2>&1;
}

deleteVSDir()
{
  echo "Deleting .vs directories...";
  find "$WorkingDir/" -type d -name ".vs" -exec rm -rf {} \; > /dev/null 2>&1;
}

deleteLogs()
{
  echo "Deleting Logs directories...";
  find "$WorkingDir/" -type d -name "Logs" -exec rm -rf {} \; > /dev/null 2>&1;
}

deleteUserCsprojFiles()
{
  echo "Deleting *.csproj.user files...";
  find "$WorkingDir/" -type f -name "*.csproj.user" -exec rm -rf {} \; > /dev/null 2>&1;
}

deleteTestResults()
{
  echo "Deleting test and coverage artifacts...";
  find "$WorkingDir/" -type d -name "TestResults" -exec rm -rf {} \; > /dev/null 2>&1;
}

deleteLocalGitBranches()
{
  echo "Deleting local unused git branches (e.g. no corresponding remote branch)...";
  git fetch -p && git branch -vv | awk '/: gone\]/{print $1}' | xargs -I {} git branch -D {}
}

safetyCheck;
echo "";

if [ "$1" = "help" ]; then
  echo "Usage:";
  echo "";
  echo -e "clean.sh [obj | vs | logs | user | coverages | branches | all]";
  echo "";
  echo -e "obj (Default)\t-\tDeletes bin and obj directories.";
  echo -e "vs\t\t-\tDeletes .vs directories.";
  echo -e "logs\t\t-\tDeletes Logs directories.";
  echo -e "user\t\t-\tDeletes *.csproj.user files.";
  echo -e "coverages\t-\tDeletes test and coverage artifacts.";
  echo -e "branches\t-\tDeletes local unused git branches (e.g. no corresponding remote branch).";
  echo -e "all\t\t-\tApply all options";

elif [ "$1" = "obj" ]; then
  deleteBinObj;
elif [ "$1" = "vs" ]; then
  deleteVSDir;
elif [ "$1" = "logs" ]; then
  deleteLogs;
elif [ "$1" = "user" ]; then
  deleteUserCsprojFiles;
elif [ "$1" = "coverages" ]; then
  deleteTestResults;
elif [ "$1" = "branches" ]; then
  deleteLocalGitBranches;
elif [ "$1" = "all" ]; then
  deleteBinObj;
  deleteVSDir;
  deleteLogs;
  deleteUserCsprojFiles;
  deleteTestResults;
  deleteLocalGitBranches;
else
  deleteBinObj;
fi
