
# utf-8
Chcp 65001

# 发布目录
$targetPath = "..\publish"

# 发布目录下 App 目录
$appPath = "$targetPath\App"
# 发布目录下 Package 目录
$packPath = "$targetPath\Package"

# 发布目录下的模块项目目录
$modulesPath = "$appPath\Modules"
$demosPath = "$appPath\Demos"

# 1.删除原发布目录
if(Test-Path $targetPath)
{
    Remove-Item -Path $targetPath -Recurse
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
dotnet publish -c Release --no-self-contained -o $appPath\ ..\Rubik.sln

# 4.分类
Move-Item -Path $appPath\Rubik.Demo.* -Destination $demosPath -Force
Move-Item -Path $appPath\Rubik.Module.* -Destination $modulesPath -Force

# 5.发布 Package
dotnet pack -c Release -o $packPath\ ..\src\Package\Toolkit\Rubik.Toolkit\Rubik.Toolkit.csproj
dotnet pack -c Release -o $packPath\ ..\src\Package\Theme\Rubik.Theme\Rubik.Theme.csproj
dotnet pack -c Release -o $packPath\ ..\src\Package\Theme\Rubik.Theme.Extension\Rubik.Theme.Extension.csproj
dotnet pack -c Release -o $packPath\ ..\src\Package\Theme\Rubik.Theme.VLC\Rubik.Theme.VLC.csproj