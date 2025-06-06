Option Explicit

Const DELETE_PATH_PATTERN = "\\(bin|obj)$"

Const DISP_MAX = 12 '確認ダイアログ ファイル表示数

Dim fso 'As Scripting.FileSystemObject
Dim reg 'As VBScript.RegExp
Dim filePaths 'As Scripting.Dictionary



Function GetCurrentFolder()
	Dim wsh ' WshShell オブジェクト
	Set wsh = CreateObject("WScript.Shell")
	Set GetCurrentFolder = fso.GetFolder(wsh.CurrentDirectory)
	Set wsh = Nothing
End Function

Function GetReg(pattern, ignoreCase)
	Dim reg
	'Set reg = New RegExp
	Set reg = CreateObject("VBScript.RegExp")
	reg.Pattern = pattern
	reg.IgnoreCase = ignoreCase
	Set GetReg = reg
End Function

Sub DeleteVBW(folder)
	Dim f  'As Folder

	'アクセス権限チェック
	On Error Resume Next
		f = folder.Files.Count
		If Err.Number <> 0 Then
			Exit Sub '## EXIT
		End If
	On Error Goto 0

	For Each f In folder.SubFolders
		If reg.Test(f.Path) Then
			Call filePaths.Add(f.Path, f.Path)
		Else
			Call DeleteVBW(f)
		End If
	Next

	Set f = Nothing
End Sub

'@param listStart (As Long) 対象ファイルリストの表示開始位置。1から開始。
'@param listEnd (As Long) 対象ファイルリストの表示終了位置。listStart 以下の値の場合は自動で設定。
'@return 削除した件数。キャンセル・失敗時は 0。
Function MsgLoop(listStart, listEnd) '(As Long)

	MsgLoop = 0 '## RESULT

	If listStart <= 0 Then
		Call MsgBox("Error: listStart(" & listStart & ") <= 0")
		Exit Function '## EXIT
	End If

	If listEnd <= listStart Then
		listEnd = listStart + DISP_MAX - 1
	End If

	If filePaths.Count < listEnd Then
		listEnd = filePaths.Count
	End If

	Dim hokaCt '(As Long)
	hokaCt = filePaths.Count - (listEnd - listStart + 1)
	If hokaCt < 0 Then
		Call MsgBox("Error: hokaCt(" & hokaCt & ") < 0")
		Exit Function '## EXIT
	End If

	Dim nextCt '(As Long)
	nextCt = filePaths.Count - listEnd
	If DISP_MAX < nextCt Then nextCt = DISP_MAX

	Dim pMsg '(As String)
	pMsg = "対象のファイルが " & filePaths.Count & " 見つかりました。" & vbNewLine & "全て削除しますか？" & vbNewLine & vbNewLine
	pMsg = pMsg & "   [ はい ]  全て削除    [ いいえ ]  "

	If 0 < nextCt Then
		pMsg = pMsg & "次の " & nextCt & "件を"
	Else
		pMsg = pMsg & "1件目から"
	End If

	pMsg = pMsg & "表示" & vbNewLine & vbNewLine & " -- 削除ファイル --" & vbNewLine & vbNewLine

	Dim i '(As Long)
	i = 1

	Dim key '(As String) ファイルパス
	For Each key in filePaths
		If listStart <= i Then
			pMsg = pMsg & "." & Mid(filePaths(key), cdLen + 1) & vbNewLine

			If listEnd <= i Then Exit For '## BREAK
		End If
		i = i + 1
	Next

	If 0 < hokaCt Then pMsg = pMsg & vbNewLine & "(他 " & hokaCt & " 件)" & vbNewLine

	Dim pMsgRes '(As DialogResult)
	pMsgRes = MsgBox(pMsg, vbYesNoCancel + vbDefaultButton2)

	If pMsgRes = vbYes Then
		'全件削除
		For Each key in filePaths
			Call fso.DeleteFolder(filePaths(key), True)
		Next

		MsgLoop = filePaths.Count '## RESULT
	ElseIf pMsgRes = vbNo Then
		'再帰
		If 0 < nextCt Then
			MsgLoop = MsgLoop(listEnd + 1, listEnd + nextCt) '## RESULT
		Else
			'1件目から
			MsgLoop = MsgLoop(1, 0) '## RESULT
		End If
	End If
End Function




Set fso = CreateObject("Scripting.FileSystemObject")
Set filePaths = CreateObject("Scripting.Dictionary")

Set reg = GetReg(DELETE_PATH_PATTERN, True)


Dim cdLen '(As Long) フォルダパスの文字数

'{
	Dim cd  'As Folder 'カレントディレクトリ
	Set cd = GetCurrentFolder()

	cdLen = Len(cd.Path)

	Call DeleteVBW(cd)

	Set cd = Nothing
'}

Set reg = Nothing

If filePaths.Count = 0 Then
	Call MsgBox("対象のフォルダは見つかりませんでした。")
Else
	Dim delCt '(As Long)
	delCt = MsgLoop(1, 0)
	If 0 < delCt Then Call MsgBox(delCt & "件 削除完了")
End If

Set filePaths = Nothing
Set fso = Nothing
