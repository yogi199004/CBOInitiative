@echo off
rem Sql project publish options
SET TARGETCONNECTIONSTRINGTEMPLATE="Data Source=localhost;Initial Catalog={DatabaseName};Integrated Security=SSPI;"
IF NOT "%~1"=="" SET TARGETCONNECTIONSTRINGTEMPLATE="%~1"
SET CREATENEWDATABASE="False"
IF NOT "%~2"=="" SET CREATENEWDATABASE="%~2"
SET DATABASEPREFIX=""
IF NOT "%~3"=="" SET DATABASEPREFIX="%~3"

SETLOCAL ENABLEDELAYEDEXPANSION

SET BUILDLOGDIR=bin\Release\
SET BUILDLOG=Build.Log.txt
IF EXIST %BUILDLOGDIR% SET BUILDLOG=%BUILDLOGDIR%Build.Log.txt

echo Start AAPS.L10nPortal.Database.OPM_Cache.Schema enviroment deploying
@echo Start AAPS.L10nPortal.Database.OPM_Cache.Schema enviroment deploying > %BUILDLOG%
SET TARGETCACHEDBNAME="%DATABASEPREFIX:"=%L10n_OPM_Cache"
SET TARGETCONNECTIONSTRING=!TARGETCONNECTIONSTRINGTEMPLATE:{DatabaseName}=%TARGETCACHEDBNAME:"=%!
call "AAPS.L10nPortal.Database.OPM_Cache\AAPS.L10nPortal.Database.OPM_Cache.Schema.Build.bat" %TARGETCONNECTIONSTRING% %CREATENEWDATABASE%

echo AAPS.L10nPortal.Database.OPM_Cache.Publish enviroment deploying
@echo Start AAPS.L10nPortal.Database.OPM_Cache.Publish enviroment deploying > %BUILDLOG%
SET TARGETCACHEDBNAME="%DATABASEPREFIX:"=%L10n_OPM_Cache"
SET TARGETCONNECTIONSTRING=!TARGETCONNECTIONSTRINGTEMPLATE:{DatabaseName}=%TARGETCACHEDBNAME:"=%!
call "AAPS.L10nPortal.Database.OPM_Cache.Publish\AAPS.L10nPortal.Database.OPM_Cache.Publish.Build.bat" %TARGETCONNECTIONSTRING%

echo Start AAPS.L10nPortal.Database.OPM_Cache.Schema enviroment deploying
@echo Start AAPS.L10nPortal.Database.OPM_Cache.Schema enviroment deploying > %BUILDLOG%
SET TARGETL10NDBNAME="%DATABASEPREFIX:"=%L10nPortal"
SET TARGETCONNECTIONSTRING=!TARGETCONNECTIONSTRINGTEMPLATE:{DatabaseName}=%TARGETL10NDBNAME:"=%!
call "AAPS.L10nPortal.Database.Schema\AAPS.L10nPortal.Database.Schema.Build.bat" %TARGETCONNECTIONSTRING% %CREATENEWDATABASE%

echo AAPS.L10nPortal.Database.OPM_Cache.Publish enviroment deploying
@echo Start AAPS.L10nPortal.Database.OPM_Cache.Publish enviroment deploying > %BUILDLOG%
SET TARGETL10NDBNAME="%DATABASEPREFIX:"=%L10nPortal"
SET TARGETCONNECTIONSTRING=!TARGETCONNECTIONSTRINGTEMPLATE:{DatabaseName}=%TARGETL10NDBNAME:"=%!
call "AAPS.L10nPortal.Database.Publish\AAPS.L10nPortal.Database.Publish.Build.bat" %TARGETCONNECTIONSTRING%

ENDLOCAL

pause