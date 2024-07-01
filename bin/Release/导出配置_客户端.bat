
chcp 65001

echo '导出配置_客户端'
Excel2Json.exe


echo '拷贝Json到客户端'
xcopy CJson\*.json ..\\..\\G01Client\\Assets\\Resources\\Dynamic\\TextAsset\\Config /s /y /h /r

echo '拷贝配置定义C#文件d到客户端'
xcopy CDef\*.cs ..\\..\\G01Client\\Assets\\Scripts\\Service\\Config /s /y /h /r

echo '导出完成'

pause

