@SETLOCAL
@ECHO OFF
SET PROJECT=NServiceBus.Serializers.SystemXml
SET VERSION=1.0.0-SNAPSHOT
IF "%1" NEQ "" SET VERSION=%1
SET R=buildsupport\ripple
%R% clean
msbuild src\%PROJECT%.sln /target:clean
msbuild src\%PROJECT%.sln
%R% create-packages -C -u --version %VERSION%
:END
ENDLOCAL