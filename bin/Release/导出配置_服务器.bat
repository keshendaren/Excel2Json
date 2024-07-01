
chcp 65001

echo '导出配置并copy到服务器工程目录'


echo '导出配置'

Excel2Json.exe


echo 'copy到服务器'
xcopy SJson\*.json ..\Server\SJson /s /y /h /r


xcopy SDef\*.h ..\..\G01Server\Common /s /y /h /r
xcopy SDef\*.cpp ..\..\G01Server\Common /s /y /h /r



echo '导出结束'

pause