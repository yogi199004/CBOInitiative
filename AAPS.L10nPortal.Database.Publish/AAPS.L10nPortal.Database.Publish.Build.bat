@echo off
SET BUILDLOGDIR=bin\Release\
SET BUILDLOG=Build.Log.txt
IF EXIST %BUILDLOGDIR% SET BUILDLOG=%BUILDLOGDIR%Build.Log.txt
SET MODE="STANDLONE"
IF NOT %CD%\==%~dp0 SET MODE="DEPENDENT"

rem Sql project publish options
SET TARGETCONNECTIONSTRING="Data Source=localhost;Initial Catalog=L10nPortal;Integrated Security=SSPI;"
IF NOT "%~1"=="" SET TARGETCONNECTIONSTRING="%~1"

REM ----------------
FOR /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -property installationPath`) do (
  SET VSPATH=%%i
)
IF ERRORLEVEL 1 GOTO MissingMSBuild
IF NOT EXIST "%VSPATH%" GOTO MissingMSBuild

SET MSBUILDPATH=%VSPATH%\MSBuild\Current\Bin\MSBuild.exe
IF NOT EXIST "%MSBUILDPATH%" SET MSBUILDPATH=%VSPATH%\MSBuild\15.0\Bin\MSBuild.exe
IF NOT EXIST "%MSBUILDPATH%" GOTO MissingMSBuild

SET SQLPACKAGEPATH=%VSPATH%\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\130\sqlpackage.exe
IF NOT EXIST "%SQLPACKAGEPATH%" GOTO MissingSqlPackage
REM ----------------

echo Start building AAPS.L10nPortal.Database.Publish.sqlproj

IF %MODE%=="DEPENDENT" echo Start building AAPS.L10nPortal.Database.Publish.sqlproj >> %BUILDLOG%
IF %MODE%=="STANDLONE" echo Start building AAPS.L10nPortal.Database.Publish.sqlproj > %BUILDLOG%	

"%MSBUILDPATH%" /t:Build "%~dp0AAPS.L10nPortal.Database.Publish.sqlproj" /P:Configuration=Release /nologo /clp:Summary /fileLogger /flp:logfile=%BUILDLOG%;append=true
SET BUILD_STATUS=%ERRORLEVEL%
IF NOT %BUILD_STATUS%==0 goto SqlProjectBuildError

echo Start publishing AAPS.L10nPortal.Database.Publish.sqlproj
echo Start publishing AAPS.L10nPortal.Database.Publish.sqlproj >> %BUILDLOG%

"%SQLPACKAGEPATH%"^
 /a:Publish^
 /p:BlockOnPossibleDataLoss=False^
 /p:DropObjectsNotInSource=False^
 /p:ExcludeObjectTypes="Logins;Routes;ServerRoleMembership;ServerRoles;Users"^
 /p:IgnorePermissions=True^
 /p:IgnoreRoleMembership=True^
 /p:GenerateSmartDefaults=False^
 /p:CreateNewDatabase=False^
 /sf:"%~dp0bin\Release\AAPS.L10nPortal.Database.Publish.dacpac"^
 /tcs:%TARGETCONNECTIONSTRING%^
 2>> %BUILDLOG%

SET BUILD_STATUS=%ERRORLEVEL%
IF NOT %BUILD_STATUS%==0 GOTO SqlProjectPublishError

GOTO EOF

:MissingMSBuild
ECHO The MSBuild tools could not be found at '%MSBUILDPATH%' >> %BUILDLOG%
GOTO ERROR
:MissingSqlPackage
ECHO The sqlpackage tools could not be found at '%SQLPACKAGEPATH%' >> %BUILDLOG%
GOTO ERROR
:SqlProjectBuildError
ECHO Sql project build failed >> %BUILDLOG%
GOTO ERROR
:SqlProjectPublishError
ECHO Sql project publish failed >> %BUILDLOG%
GOTO ERROR

:ERROR
IF %MODE%=="STANDLONE" PAUSE
exit 1
:EOF
IF %MODE%=="STANDLONE" PAUSE