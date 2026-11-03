@echo off
setlocal EnableDelayedExpansion
chcp 65001 >nul
set "PROJ=F:\CAPSTONEWEBISTE\PROJECT\API\NVCMS.API.ReadGoogleSheet\NVCMS.API.ReadGoogleSheet.csproj"
set "TESTS=F:\CAPSTONEWEBISTE\PROJECT\API\NVCMS.API.ReadGoogleSheet.Tests"
set "LOG=F:\CAPSTONEWEBISTE\Claude outputs\build-api.log"
echo Dang build NVCMS.API.ReadGoogleSheet (Release)... log: %LOG%
(
  echo === START %date% %time%
  where dotnet
  dotnet --list-sdks
  echo.
  echo === BUILD
  dotnet build "%PROJ%" -c Release -nologo
  echo BUILD_EXIT=!ERRORLEVEL!
  echo.
  echo === TEST
  for %%F in ("%TESTS%\*.csproj") do dotnet test "%%F" -c Release -nologo
  echo TEST_EXIT=!ERRORLEVEL!
  echo === END %date% %time%
) > "%LOG%" 2>&1
echo Xong. Ket qua trong %LOG%
timeout /t 5 >nul
