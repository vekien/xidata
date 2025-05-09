#NoEnv
#SingleInstance,Force
DetectHiddenWindows, on
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
	OpenIsFocused := false
	
	; Logic loop
	Loop,
	{
		; Check if the open window is .. open
		if (WinExist("Open") and !OpenIsFocused)
		{
			; prevent attempting on this open again
			OpenIsFocused := true
			
			; Read the AHK_SKELETON file to get the current skeleton
			FileRead, skeleton_path, AHK_SKELETON.txt
			Sleep, 100

			; Open select
			WinActivate, Open
			Sleep, 200

			; Input the skeleton path
			SendInput {Raw}%skeleton_path%
			Sleep, 200
			Send {Enter}
			Log("Inserted rom file to the open window.")
		}

		if (!WinExist("Open")) {
			OpenIsFocused := false
		}
		
		Sleep, 200
	}

; --------------------------------------------------------------------------------
; CUSTOM FUNCTIONS
; --------------------------------------------------------------------------------

Log(text) {
	global

	FormatTime, CurrentDateTime,, dd-MM-yy HH:mm:ss
	FileAppend, [%CurrentDateTime%] %text% `n, ./ahk_log.txt
}

LogReset() {
	FileDelete, ./ahk_log.txt
}