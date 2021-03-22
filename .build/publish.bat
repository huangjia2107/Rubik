@echo off

:: utf-8
Chcp 65001>nul

:: 发布目录
set publish_path=..\publish

:: 发布目录下 Demo 目录
set publish_demo_path=%publish_path%\Demo
:: 发布目录下 Package 目录
set publish_pack_path=%publish_path%\Package

:: 1.删除原发布目录
if exist %publish_path% (
    del /s/q %publish_path%\*.*
    rd /s/q %publish_path%
)

:: 2.检测文件夹
if not exist %publish_demo_path%\ md %publish_demo_path%\
if not exist %publish_pack_path%\ md %publish_pack_path%\

:: 3.发布 Demo
dotnet publish -c Release -f netcoreapp3.1 --no-self-contained -o %publish_demo_path%\ ..\Rubik.sln

:: 4.发布 Package
dotnet pack -c Release -o %publish_pack_path%\ ..\src\Rubik.Theme\Rubik.Theme.csproj

@IF %ERRORLEVEL% NEQ 0 pause