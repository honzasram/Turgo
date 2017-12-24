Set installpath="%appdata%\Turgo\"
xcopy /E . %installpath% 
call Shortcut.bat %installpath%