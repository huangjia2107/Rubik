
# utf-8
Chcp 65001

# 目录
$packPath = "..\publish\Udp"

# 1.删除原发布目录
if(Test-Path $packPath)
{
    Remove-Item -Path $packPath -Recurse
}

# 2.发布
dotnet pack ..\src\Package\Comm\Rubik.Comm.Udp\Rubik.Comm.Udp.csproj -c Release -o $packPath\