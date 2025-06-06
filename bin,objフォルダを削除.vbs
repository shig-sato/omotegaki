Option Explicit

Const DELETE_PATH_PATTERN = "\\(bin|obj)$"

Const DISP_MAX = 12 '�m�F�_�C�A���O �t�@�C���\����

Dim fso 'As Scripting.FileSystemObject
Dim reg 'As VBScript.RegExp
Dim filePaths 'As Scripting.Dictionary



Function GetCurrentFolder()
	Dim wsh ' WshShell �I�u�W�F�N�g
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

	'�A�N�Z�X�����`�F�b�N
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

'@param listStart (As Long) �Ώۃt�@�C�����X�g�̕\���J�n�ʒu�B1����J�n�B
'@param listEnd (As Long) �Ώۃt�@�C�����X�g�̕\���I���ʒu�BlistStart �ȉ��̒l�̏ꍇ�͎����Őݒ�B
'@return �폜���������B�L�����Z���E���s���� 0�B
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
	pMsg = "�Ώۂ̃t�@�C���� " & filePaths.Count & " ������܂����B" & vbNewLine & "�S�č폜���܂����H" & vbNewLine & vbNewLine
	pMsg = pMsg & "   [ �͂� ]  �S�č폜    [ ������ ]  "

	If 0 < nextCt Then
		pMsg = pMsg & "���� " & nextCt & "����"
	Else
		pMsg = pMsg & "1���ڂ���"
	End If

	pMsg = pMsg & "�\��" & vbNewLine & vbNewLine & " -- �폜�t�@�C�� --" & vbNewLine & vbNewLine

	Dim i '(As Long)
	i = 1

	Dim key '(As String) �t�@�C���p�X
	For Each key in filePaths
		If listStart <= i Then
			pMsg = pMsg & "." & Mid(filePaths(key), cdLen + 1) & vbNewLine

			If listEnd <= i Then Exit For '## BREAK
		End If
		i = i + 1
	Next

	If 0 < hokaCt Then pMsg = pMsg & vbNewLine & "(�� " & hokaCt & " ��)" & vbNewLine

	Dim pMsgRes '(As DialogResult)
	pMsgRes = MsgBox(pMsg, vbYesNoCancel + vbDefaultButton2)

	If pMsgRes = vbYes Then
		'�S���폜
		For Each key in filePaths
			Call fso.DeleteFolder(filePaths(key), True)
		Next

		MsgLoop = filePaths.Count '## RESULT
	ElseIf pMsgRes = vbNo Then
		'�ċA
		If 0 < nextCt Then
			MsgLoop = MsgLoop(listEnd + 1, listEnd + nextCt) '## RESULT
		Else
			'1���ڂ���
			MsgLoop = MsgLoop(1, 0) '## RESULT
		End If
	End If
End Function




Set fso = CreateObject("Scripting.FileSystemObject")
Set filePaths = CreateObject("Scripting.Dictionary")

Set reg = GetReg(DELETE_PATH_PATTERN, True)


Dim cdLen '(As Long) �t�H���_�p�X�̕�����

'{
	Dim cd  'As Folder '�J�����g�f�B���N�g��
	Set cd = GetCurrentFolder()

	cdLen = Len(cd.Path)

	Call DeleteVBW(cd)

	Set cd = Nothing
'}

Set reg = Nothing

If filePaths.Count = 0 Then
	Call MsgBox("�Ώۂ̃t�H���_�͌�����܂���ł����B")
Else
	Dim delCt '(As Long)
	delCt = MsgLoop(1, 0)
	If 0 < delCt Then Call MsgBox(delCt & "�� �폜����")
End If

Set filePaths = Nothing
Set fso = Nothing
