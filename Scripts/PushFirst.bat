@echo off
REM --- First push to GitHub ---

REM Initialize git repository
git init

REM Add all files
git add .

REM Commit with message
set /p commitmsg=Enter commit message: 
git commit -m "%commitmsg%"

REM Rename branch to main
git branch -M main

REM Set remote repository
set /p repo=Enter repository URL: 
git remote add origin %repo%

REM Push to GitHub
git push -u origin main --force

pause