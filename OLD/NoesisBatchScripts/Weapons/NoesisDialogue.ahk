#NoEnv
#SingleInstance,Force
DetectHiddenWindows, on

; Ensure mouse coords are always fetched via screen size 
; and not relative to the active window
CoordMode Mouse, Screen
CoordMode Pixel, Screen

F3::
	; reset log file
	LogReset()
	Log("Starting noesis window detection loop...")

	; Start the bot
	SetTimer, WindowLoop, 500
	return

F8::
	ExitApp
	Return

WindowLoop:
	; Logic loop
	Loop,
	{
		DetectNoesisWindow()
		Sleep, 200
	}
	
DetectNoesisWindow() {
	; Check if the open window is .. open
	if (WinExist("Open"))
	{
		WinActivate, Open
		Sleep, 300

		Log("Closing 'Open' Window...")
		Send {Esc}
		Sleep, 300
	}
}

Log(text) {
	global

	FormatTime, CurrentDateTime,, dd-MM-yy HH:mm:ss
	FileAppend, [%CurrentDateTime%] %text% `n, ./ahk_log.txt
}

LogReset() {
	FileDelete, ./ahk_log.txt
}