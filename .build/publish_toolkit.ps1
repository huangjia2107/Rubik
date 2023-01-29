
# utf-8
Chcp 65001

# 目录
$packPath = "..\publish\Toolkit"

# 1.删除原发布目录
if(Test-Path $packPath)
{
    Remove-Item -Path $packPath -Recurse
}

# 2.发布
dotnet pack ..\src\Package\Core\Rubik.Toolkit\Rubik.Toolkit.csproj -c Release -o $packPath\