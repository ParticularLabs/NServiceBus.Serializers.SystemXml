SETLOCAL
SET SRCDIR=C:\apps\dotnet\svn\NServiceBus\binaries
SET DESTDIR=.
COPY %SRCDIR%\NServiceBus.dll %DESTDIR%
COPY %SRCDIR%\NServiceBus.pdb %DESTDIR%
COPY %SRCDIR%\NServiceBus.xml %DESTDIR%
COPY %SRCDIR%\NServiceBus.Core.* %DESTDIR%
ENDLOCAL
