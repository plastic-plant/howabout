@echo off

rem Get the name of the current script without extension to use as a build config name.
setlocal enabledelayedexpansion
for %%F in ("%~dp0%~n0") do set "currentScript=%%~nF"

rem Build a release by its preset build configuration.
call ../../src/make/make.cmd --buildconfig !currentScript!
exit /b %ERRORLEVEL%