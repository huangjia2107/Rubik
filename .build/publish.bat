@echo off

:: utf-8
Chcp 65001>nul

:: 发布目录
set publish_path=..\publish

:: 发布目录下 App 目录
set publish_app_path=%publish_path%\App
:: 发布目录下 Package 目录
set publish_pack_path=%publish_path%\Package

:: 发布目录下的模块项目目录
set publish_app_modules_path=%publish_app_path%\Modules
set publish_app_demos_path=%publish_app_path%\Demos

:: 1.删除原发布目录
if exist %publish_path% (
    del /s/q %publish_path%\*.*
    rd /s/q %publish_path%
)

:: 2.检测文件夹
if not exist %publish_app_modules_path%\ md %publish_app_modules_path%\
if not exist %publish_app_demos_path%\ md %publish_app_demos_path%\

:: 3.发布 Demo
dotnet publish -c Release -f netcoreapp3.1 --no-self-contained -o %publish_app_path%\ ..\Rubik.sln

:: 4.分类
move /y %publish_app_path%\Rubik.Demo.* %publish_app_demos_path%\
move /y %publish_app_path%\Rubik.Module.* %publish_app_modules_path%\

:: 5.发布 Package
dotnet pack -c Release -o %publish_pack_path%\ ..\src\Rubik.Toolkit\Rubik.Toolkit.csproj
dotnet pack -c Release -o %publish_pack_path%\ ..\src\Rubik.Theme\Rubik.Theme.csproj
dotnet pack -c Release -o %publish_pack_path%\ ..\src\Rubik.Theme.Extension\Rubik.Theme.Extension.csproj

@IF %ERRORLEVEL% NEQ 0 pause