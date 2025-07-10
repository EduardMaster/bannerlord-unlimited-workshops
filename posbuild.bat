set "MOD_DEST=C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\UnlimitedWorkshops"
if not exist "%MOD_DEST%" (
    mkdir "%MOD_DEST%"
)
xcopy resources "%MOD_DEST%" /s /e /y
set "BIN_DEST=%MOD_DEST%\bin\Win64_Shipping_Client"
if not exist "%BIN_DEST%" (
    mkdir "%BIN_DEST%"
)
xcopy bin\Win64_Shipping_Client\net472\*.dll "%BIN_DEST%" /s /e /y