
import sys
import os
import keyboard
import threading

from settings import extract_output_path
from extract_animations import extract_animations
from extract_gear import extract_gear
from extract_npcs import extract_npcs
from extract_trusts import extract_trusts
from extract_weapons import extract_weapons
from extract_zones import extract_zones
from extract_faces import extract_faces


def listen_for_cancel():
    global cancelled
    keyboard.wait('space')
    cancelled = True
    print("! Spacebar detected - ending application...")
    os._exit(1)

listener = threading.Thread(target=listen_for_cancel, daemon=True)
listener.start()


if __name__ == "__main__":
    print(f"{'-'*100}")
    print("Welcome to the Final Fantasy XI Asset Extractor!")
    print("Built by: Vekien")
    print("You can extract various types of assets from Final Fantasy XI via Noesis.")
    print("Press SPACE at any time to cancel the extraction process.")
    print(f"{'-'*100}\n")

    # create the output directory if it doesn't exist
    if not os.path.exists(extract_output_path):
        os.makedirs(extract_output_path)

    # Check for command-line argument
    if len(sys.argv) > 1:
        choice = sys.argv[1].strip().lower()
        print(f"Argument detected: {choice}")
    else:
        print("No argument provided, please provide one: python app.py <extract function>")
        exit(1)

    print(f"Selected extraction option: {choice}\n")

    functions = {
        "zones": extract_zones,
        "gear": extract_gear,
        "faces": extract_faces,
        "npcs": extract_npcs,
        "trusts": extract_trusts,
        "weapons": extract_weapons,
        "animations": extract_animations
    }

    print(f"Loading Function: {choice}\n")
    functions.get(choice)()
    print("\nExtraction  completed successfully!")
