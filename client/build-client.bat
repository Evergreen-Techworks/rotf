@echo off
setlocal enabledelayedexpansion
title Ordinary Client / ROTF - build Flash client

REM ============================================================================
REM  Builds the Ordinary Client (realm-cli AS3) into bin-debug\WebMain.swf.
REM  First run downloads the Apache Flex SDK 4.16.1 (~72 MB) + playerglobal 11.2
REM  into a local .buildsdk\ folder; later runs reuse them.
REM
REM  Requirements: Java (JDK/JRE 8-17) on PATH  ->  https://adoptium.net
REM  Recipe validated on Linux against this exact client (0 errors).
REM
REM  Can be run directly from a \\wsl$ path (pushd maps it to a drive letter).
REM ============================================================================

pushd "%~dp0" || ( echo [ERROR] Cannot enter script folder. & pause & exit /b 1 )
set "ROOT=%CD%"
set "BUILD=%ROOT%\.buildsdk"
set "FLEX_URL=https://archive.apache.org/dist/flex/4.16.1/binaries/apache-flex-sdk-4.16.1-bin.zip"
set "PG_URL=https://web.archive.org/web/2017id_/https://fpdownload.macromedia.com/get/flashplayer/installers/archive/playerglobal/playerglobal11_2.swc"

echo(
echo === Ordinary Client / ROTF : building WebMain.swf ===
echo(

REM --- Java present? ---
where java >nul 2>&1 || ( echo [ERROR] Java not found on PATH. Install Temurin JDK 8-17 from https://adoptium.net and re-run. & popd & pause & exit /b 1 )

REM --- locate the Flex SDK (handles both flat and nested zip layouts) ---
set "SDK_DIR="
if exist "%BUILD%\bin\mxmlc.bat" set "SDK_DIR=%BUILD%"
if exist "%BUILD%\apache-flex-sdk-4.16.1-bin\bin\mxmlc.bat" set "SDK_DIR=%BUILD%\apache-flex-sdk-4.16.1-bin"

if not defined SDK_DIR (
  echo [setup] Downloading Apache Flex SDK 4.16.1 ^(~72 MB, one-time^)...
  if not exist "%BUILD%" mkdir "%BUILD%"
  powershell -NoProfile -Command "[Net.ServicePointManager]::SecurityProtocol='Tls12'; Invoke-WebRequest '%FLEX_URL%' -OutFile '%BUILD%\flex.zip'" || ( echo [ERROR] SDK download failed. & popd & pause & exit /b 1 )
  echo [setup] Extracting SDK...
  powershell -NoProfile -Command "Expand-Archive -Force '%BUILD%\flex.zip' '%BUILD%'" || ( echo [ERROR] SDK extract failed. & popd & pause & exit /b 1 )
  del "%BUILD%\flex.zip" >nul 2>&1
  if exist "%BUILD%\bin\mxmlc.bat" set "SDK_DIR=%BUILD%"
  if exist "%BUILD%\apache-flex-sdk-4.16.1-bin\bin\mxmlc.bat" set "SDK_DIR=%BUILD%\apache-flex-sdk-4.16.1-bin"
)
if not defined SDK_DIR ( echo [ERROR] Could not find mxmlc after extraction. Check %BUILD%. & popd & pause & exit /b 1 )

REM --- playerglobal 11.2 (swf-version 15 / Flash Player 11.2, Stage3D for Starling) ---
set "PG_DIR=%SDK_DIR%\frameworks\libs\player\11.2"
if not exist "%PG_DIR%\playerglobal.swc" (
  echo [setup] Downloading playerglobal 11.2...
  if not exist "%PG_DIR%" mkdir "%PG_DIR%"
  powershell -NoProfile -Command "[Net.ServicePointManager]::SecurityProtocol='Tls12'; Invoke-WebRequest '%PG_URL%' -OutFile '%PG_DIR%\playerglobal.swc'" || ( echo [ERROR] playerglobal download failed. & popd & pause & exit /b 1 )
)
set "PLAYERGLOBAL_HOME=%SDK_DIR%\frameworks\libs\player"

REM --- compile (args mirror client\.actionScriptProperties) ---
if not exist "%ROOT%\bin-debug" mkdir "%ROOT%\bin-debug"
echo [build] Compiling src\WebMain.as  (first build takes a minute)...
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

echo(
if exist "%ROOT%\bin-debug\WebMain.swf" (
  echo ============================================================
  echo [OK] Built:  %ROOT%\bin-debug\WebMain.swf
  echo Run it in the standalone Flash Player Projector, pointed at your server.
  echo ============================================================
) else (
  echo [FAILED] No SWF produced - scroll up for the first "Error:" line.
)
popd
pause
