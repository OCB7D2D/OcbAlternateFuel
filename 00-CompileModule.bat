@echo off

call MC7D2D AlternateFuel.dll /reference:"%PATH_7D2D_MANAGED%\Assembly-CSharp.dll" Harmony\*.cs Library\*.cs && ^
echo Successfully compiled AlternateFuel.dll

pause