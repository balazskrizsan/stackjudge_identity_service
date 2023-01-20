
for %%I in ("WebStorm", "IntelliJ", "CLion", "Rider", "GoLand", "PhpStorm") do (
  for /d %%a in ("%USERPROFILE%\.%%I*") do (
    rd /s /q "%%a/config/eval"
    del /q "%%a\config\options\other.xml"
  )
)

rmdir /s /q "%APPDATA%\JetBrains"
reg delete "HKEY_CURRENT_USER\Software\JavaSoft" /f