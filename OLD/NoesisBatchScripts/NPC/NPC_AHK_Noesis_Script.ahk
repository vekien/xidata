#NoEnv
#SingleInstance,Force
DetectHiddenWindows, on
CoordMode Mouse, Screen
CoordMode Pixel, Screen

F3::
	LogReset()
	Log("Starting batch export in 1 second!")
	Sleep, 1000

	; read npc_ahk text
	FileRead, fileContent, NPC_AHK.txt
	RomFiles := StrSplit(fileContent, "`n")
	
	; Loop through each rom
	for index, rom in RomFiles {

		romdata := StrSplit(rom, "|")

		rom_name := romdata[1]
		rom_destination := romdata[2]
		rom_source := romdata[3]

		Log("Starting: " rom_name " --> " rom_source " --> " rom_destination)

		Send {Alt down}{F}{Alt up}
		Sleep, 100
		Send {O}
		Sleep, 300

		WinActivate, Open ; focus window
		SendInput {Raw}%rom_source%
		Sleep, 600

		Send {Enter}
		Sleep, 1500 ; let it load

		Send {Alt down}{F}{Alt up}
		Sleep, 300
		Send {E} ; Export button
		Sleep, 200

		Send {Tab} ; Ensure focus
		Sleep, 80
		Send {Tab} ; Source File
		Sleep, 80
		Send {Tab} ; Destination file
		Sleep, 80
		SendInput {Raw}%rom_destination%
		Sleep, 180

		Send {Tab} ; Browse
		Sleep, 80
		Send {Tab} ; Main output type
		Sleep, 80
		Send {Tab} ; Additional texture output
		Sleep, 80

		Loop, 21
		{
			Send {DOWN}
			Sleep, 25
		}

		Send {Tab} ; Additional animation output
		Sleep, 80

		Loop, 12
		{
			Send {DOWN}
			Sleep, 25
		}

		Send {Tab} ; Advanced Options
		Sleep, 80

		SendInput {Raw}-ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11nolodchange -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 0 -scale 120
		Sleep, 180

		ExportWindow := WinExist("A")

		Send {Enter} ; Start exporting

		Loop, 600
		{
			; Wait for active window to change
			if (WinExist("A") != ExportWindow) {
				Sleep, 180
				Send {Esc}
				Sleep, 50
				Send {Esc}
				
				break
			}

			Sleep, 100
		}

		; Next
		Sleep, 1000
	}
		
	return

F8::
	ExitApp
	Return


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