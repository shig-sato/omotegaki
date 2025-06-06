
cd %SystemRoot%\Microsoft.NET\Framework\v2*

RegAsm.exe /u %SystemRoot%\system32\SeitokuEreceipt.dll /tlb

del %SystemRoot%\system32\SeitokuEreceipt.dll
del %SystemRoot%\system32\SeitokuEreceipt.tlb

pause