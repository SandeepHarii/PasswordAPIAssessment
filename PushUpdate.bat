@echo off
REM --- Update existing repository ---

REM Add all changes
git add .

REM Commit with message
set /p commitmsg=Enter commit message: 
git commit -m "%commitmsg%"

REM Push changes to GitHub
git push -u origin main

pause