@echo off

set VS2017="C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\devenv.com"
set VS2015="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.com"

if exist %VS2017% goto VS2017
if exist %VS2015% goto VS2015
goto ERROR

:VS2017
echo Build with VS2017
%VS2017% FredWebSolution.sln /Build SpeedDebug
if %errorlevel% equ 0 goto END
goto ERROR

:VS2015
echo Build with VS2015
%VS2015% FredWebSolution.sln /Build SpeedDebug
if %errorlevel% equ 0 goto END
goto ERROR


:ERROR
ECHO Erreur de compil, regarde les logs.
pause
goto END

:END
echo "Termine..."