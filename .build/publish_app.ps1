
# utf-8
Chcp 65001

# 目录
$appPath = "..\publish\App"

# 模块目录
$modulesPath = "$appPath\Modules"
$demosPath = "$appPath\Demos"

# 1.删除原发布目录
if(Test-Path $appPath)
{
    Remove-Item -Path $appPath -Recurse
}

# 2.检测文件夹
if(!(Test-Path $modulesPath))
{ 
    New-Item -itemType Directory -Path $modulesPath
}

if(!(Test-Path $demosPath))
{ 
    New-Item -itemType Directory -Path $demosPath
}

# 3.发布
dotnet publish ..\Rubik.sln -f netcoreapp3.1 -c Release --no-self-contained -o $appPath\

# 4.分类
Move-Item -Path $appPath\Rubik.Demo.* -Destination $demosPath -Force
Move-Item -Path $appPath\Rubik.Module.* -Destination $modulesPath -Force