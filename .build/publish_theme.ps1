
# utf-8
Chcp 65001

# 发布目录
$packPath = "..\publish\Theme"

# 1.删除原发布目录
if(Test-Path $packPath)
{
    Remove-Item -Path $packPath -Recurse
}

# 2.发布
dotnet pack ..\src\Package\Theme\Rubik.Theme\Rubik.Theme.csproj -c Release -o $packPath\
dotnet pack ..\src\Package\Theme\Rubik.Theme.Extension\Rubik.Theme.Extension.csproj -c Release -o $packPath\
dotnet pack ..\src\Package\Theme\Rubik.Theme.VLC\Rubik.Theme.VLC.csproj -c Release -o $packPath\