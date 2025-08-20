import os
import subprocess
import win32gui
import win32con
import time

from settings import (
    noesis_title,
    noesis_path,
    noesis_args
)

from controller import (
    send_key,
    send_key_loop,
    send_text,
    send_alt_key,
    paste_text,
    get_active_window_title,
    wait_for_active_window,
    wait_for_active_window_count
)


class Noesis():
    def cmode(self, arguments):
        # Prepare the Noesis command line arguments and add our common args
        noesis_command = f"{noesis_path} ?cmode {arguments} {noesis_args}"
        # print(f"[Noesis cmode] {noesis_command}")

        subprocess.run(
            noesis_command,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            shell=False,
            creationflags=subprocess.CREATE_NO_WINDOW
        )


    def open(self):
        subprocess.Popen(noesis_path)
        
        found = wait_for_active_window(
            noesis_title,
            delay=10,
            stop_on_error=True
        )
        
        time.sleep(3)
        print("Noesis opened successfully!" if found else "Failed to open Noesis.")


    def focus(self):
        hwnd = win32gui.FindWindow(None, noesis_title)

        if hwnd:
            win32gui.ShowWindow(hwnd, win32con.SW_RESTORE)
            win32gui.SetForegroundWindow(hwnd)
            time.sleep(0.5)
        else:
            raise Exception("Could not find Noesis to focus?")


    def export(self, source, destination, skeleton=None, skip_textures=False):
        # Ensure Noesis is open and focused
        self.focus()

        time.sleep(0.5)  # Give it a moment to focus

        # Open a DAT
        send_alt_key('f')
        send_key('o')

        if wait_for_active_window_count("Open", window_count=1):
            time.sleep(0.2)

            paste_text(source)
            send_key('enter')

            # if we pass in a skele, we skip the 1st prompt
            if skeleton:
                if wait_for_active_window("Open", stop_on_error=True, delay=5):
                    paste_text(skeleton)
                    send_key('enter')

            time.sleep(0.5)

            # Now open the Export Window
            send_alt_key('f')
            send_key('e')

            # Wait for the Export Media window to open
            if wait_for_active_window_count("Export Media", window_count=1):
                # Destination
                send_key_loop('tab', 3)
                paste_text(destination)

                # Texture output type
                send_key_loop('tab', 3)

                # Loop down to png
                if not skip_textures:
                    send_key('pagedown', 0.05)
                    send_key_loop('up',4)

                # Animation output type
                send_key('tab')

                # Loop down to noemultifbx
                send_key('pagedown', 0.05)
                send_key_loop('up',3)

                # Advanced options
                send_key('tab')

                # Paste Noesis args
                paste_text(noesis_args)
                send_key('enter')

                # If the open window, opens again, it means this model has a skeleton
                if skeleton:
                    if wait_for_active_window("Open", stop_on_error=True, delay=5):
                        paste_text(skeleton)
                        send_key('enter')
                        time.sleep(0.5)

                # The complete window is called Noesis
                if wait_for_active_window_count("Noesis", window_count=2, must_be_exact=True):
                    send_key_loop('esc', 5)


    def noefbxmulti_cleanup(self, output_path_temp):
        try:
            # First, delete any file ending with .noefbxmulti
            for file in os.listdir(output_path_temp):
                if file.lower().endswith(".noefbxmulti"):
                    os.remove(os.path.join(output_path_temp, file))

            # Now process all .fbx files
            for file in os.listdir(output_path_temp):
                if file.lower().endswith(".fbx"):
                    # Remove ".noefbxmulti" if it exists
                    clean_name = file.replace(".noefbxmulti", "")

                    # Split on "-" and keep only the second part
                    parts = clean_name.split("-")
                    if len(parts) >= 2:
                        clean_name = parts[1].strip()

                    # Ensure it still ends with .fbx
                    if not clean_name.lower().endswith(".fbx"):
                        clean_name += ".fbx"

                    # Build full destination path
                    base_name, ext = os.path.splitext(clean_name)
                    dst = os.path.join(output_path_temp, clean_name)

                    # Conflict resolution: add -1, -2, etc.
                    counter = 1
                    while os.path.exists(dst):
                        dst = os.path.join(output_path_temp, f"{base_name}-{counter}{ext}")
                        counter += 1

                    # Rename file
                    src = os.path.join(output_path_temp, file)
                    os.rename(src, dst)

        except Exception as e:
            print(f"Exception: {e}")


    def texture_cleanup(self, output_path_temp):
        """
        Remove any file that is a *.png and doesn't start with 'model' 
        """
        try:
            for file in os.listdir(output_path_temp):
                if file.endswith(".png") and not file.startswith("model"):
                    os.remove(os.path.join(output_path_temp, file))
        except Exception as e:
            print(f"Exception: {e}")


    def files_exists(self, output_path_temp, extension=".fbx"):
        """
        Check if any .fbx files exist in the output path.
        """
        if not os.path.exists(output_path_temp):
            return False
        
        return any(file.lower().endswith(extension) for file in os.listdir(output_path_temp))
