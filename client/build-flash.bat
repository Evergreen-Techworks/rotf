@echo off
setlocal enabledelayedexpansion
title Build Ordinary Client SWF -> Downloads

REM ============================================================================
REM  Builds the client SWF and copies it to your Downloads folder, then marks it
REM  trusted so Flash will let it open the game socket. First run downloads the
REM  Apache Flex SDK 4.16.1 (~72 MB) + playerglobal into client\.buildsdk\.
REM  Requires Java (JDK/JRE 8-17) on PATH -> https://adoptium.net
REM ============================================================================

pushd "%~dp0" || ( echo [ERROR] cannot enter script folder. & pause & exit /b 1 )
set "ROOT=%CD%"
set "BUILD=%ROOT%\.buildsdk"
set "FLEX_URL=https://archive.apache.org/dist/flex/4.16.1/binaries/apache-flex-sdk-4.16.1-bin.zip"
set "PG_URL=https://web.archive.org/web/2017id_/https://fpdownload.macromedia.com/get/flashplayer/installers/archive/playerglobal/playerglobal11_2.swc"

where java >nul 2>&1 || ( echo [ERROR] Java not on PATH. Install JDK 8-17: https://adoptium.net & popd & pause & exit /b 1 )

set "SDK_DIR="
if exist "%BUILD%\bin\mxmlc.bat" set "SDK_DIR=%BUILD%"
if exist "%BUILD%\apache-flex-sdk-4.16.1-bin\bin\mxmlc.bat" set "SDK_DIR=%BUILD%\apache-flex-sdk-4.16.1-bin"
if not defined SDK_DIR (
  echo [setup] Downloading Apache Flex SDK 4.16.1 ^(~72 MB, one-time^)...
  if not exist "%BUILD%" mkdir "%BUILD%"
  powershell -NoProfile -Command "[Net.ServicePointManager]::SecurityProtocol='Tls12'; Invoke-WebRequest '%FLEX_URL%' -OutFile '%BUILD%\flex.zip'" || ( echo [ERROR] SDK download failed. & popd & pause & exit /b 1 )
  powershell -NoProfile -Command "Expand-Archive -Force '%BUILD%\flex.zip' '%BUILD%'" || ( echo [ERROR] extract failed. & popd & pause & exit /b 1 )
  del "%BUILD%\flex.zip" >nul 2>&1
  if exist "%BUILD%\bin\mxmlc.bat" set "SDK_DIR=%BUILD%"
  if exist "%BUILD%\apache-flex-sdk-4.16.1-bin\bin\mxmlc.bat" set "SDK_DIR=%BUILD%\apache-flex-sdk-4.16.1-bin"
)
if not defined SDK_DIR ( echo [ERROR] mxmlc not found after extract. & popd & pause & exit /b 1 )

set "PG_DIR=%SDK_DIR%\frameworks\libs\player\11.2"
if not exist "%PG_DIR%\playerglobal.swc" (
  echo [setup] Downloading playerglobal 11.2...
  if not exist "%PG_DIR%" mkdir "%PG_DIR%"
  powershell -NoProfile -Command "[Net.ServicePointManager]::SecurityProtocol='Tls12'; Invoke-WebRequest '%PG_URL%' -OutFile '%PG_DIR%\playerglobal.swc'" || ( echo [ERROR] playerglobal download failed. & popd & pause & exit /b 1 )
)
set "PLAYERGLOBAL_HOME=%SDK_DIR%\frameworks\libs\player"

if not exist "%ROOT%\bin-debug" mkdir "%ROOT%\bin-debug"
echo [build] Compiling WebMain.swf  (first build takes a minute)...
echo(
call "%SDK_DIR%\bin\mxmlc.bat" "%ROOT%\src\WebMain.as" ^
  -source-path+="%ROOT%\src" ^
  -library-path+="%ROOT%\libs" ^
  -locale=en_US ^
  -default-size 800 600 ^
  -default-frame-rate=60 ^
  -default-background-color=0x000000 ^
  -swf-version=15 ^
  -target-player=11.2 ^
  -optimize=true ^
  -use-direct-blit=true ^
  -keep-as3-metadata+=Inject ^
  -keep-as3-metadata+=Embed ^
  -keep-as3-metadata+=PostConstruct ^
  -keep-as3-metadata+=ArrayElementType ^
  -static-link-runtime-shared-libraries=true ^
  -output="%ROOT%\bin-debug\WebMain.swf"

if not exist "%ROOT%\bin-debug\WebMain.swf" ( echo. & echo [FAILED] No SWF produced - scroll up for the first "Error:" line. & popd & pause & exit /b 1 )

REM --- deliver to Downloads + trust it (so Flash allows the game socket) ---
set "DL=%USERPROFILE%\Downloads"
copy /Y "%ROOT%\bin-debug\WebMain.swf" "%DL%\WebMain.swf" >nul
set "TD=%APPDATA%\Macromedia\Flash Player\#Security\FlashPlayerTrust"
if not exist "%TD%" mkdir "%TD%"
> "%TD%\ordinary-client.cfg" echo %DL%\WebMain.swf

echo.
echo ============================================================
echo [OK] Built and copied to:  %DL%\WebMain.swf   (trusted)
echo Open it in the standalone Flash Projector (32.0.0.363)
echo while run-local.sh is running in WSL.
echo ============================================================
popd
pause
