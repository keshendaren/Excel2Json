
chcp 65001

echo '导出配置'
Excel2Json.exe


echo '拷贝Json到客户端'
xcopy CJson\*.json ..\\..\\G01Client\\Assets\\Resources\\Dynamic\\TextAsset\\Config /s /y /h /r

echo '拷贝配置定义C#文件到客户端'
xcopy CDef\*.cs ..\\..\\G01Client\\Assets\\Scripts\\Service\\Config /s /y /h /r



echo 'copy到服务器'
xcopy SJson\*.json ..\Server\SJson /s /y /h /r

echo '拷贝配置定义Cpp文件到服务器'
xcopy SDef\*.h ..\..\G01Server\Common /s /y /h /r
xcopy SDef\*.cpp ..\..\G01Server\Common /s /y /h /r


echo '导出完成'

pause

