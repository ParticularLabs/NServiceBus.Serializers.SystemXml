SETLOCAL
SET PROJECT=NServiceBus.Serializers.SystemXml
SET VERSION=1.0.0
SET R=buildsupport\ripple
%R% clean
msbuild src\%PROJECT%.sln /target:clean
msbuild src\%PROJECT%.sln
%R% create-packages -v %VERSION%
:END
ENDLOCAL