@echo off

SET /P ResX=Enter the desired width of the window in pixels: 

ECHO.

SET /P ResY=Enter the desired height of the window in pixels: 

CardDemo.exe %ResX% %ResY%