@echo off
rem Build and publish all available configurations to releases folder.
call ../src/make/make.cmd -bc all
exit /b %ERRORLEVEL%