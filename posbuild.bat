set "MOD_DEST=D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord\Modules\UnlimitedWorkshops"
xcopy resources "%MOD_DEST%" /s /e /y
xcopy bin\Win64_Shipping_Client\net472\*.dll "%MOD_DEST%\bin\Win64_Shipping_Client" /s /e /y