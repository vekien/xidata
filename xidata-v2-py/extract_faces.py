import os
import json
import string

from noesis import Noesis
from xidata import xidata_race_to_skeleton
from settings import (
    extract_output_path,
    extract_overwrite_existing,
    ffxi_path
)


def extract_faces():
    print("Extracting faces...")

    # open the faces.json file
    with open("data/faces.json", "r", encoding="utf-8") as file:
        faces_data = json.load(file)

    print(f"Found {len(faces_data)} faces to process.")
    current = 0

    for data in faces_data:
        current += 1
        print(f"{current} / {len(faces_data)} - Extracting: ({data['dat']}) {data['name']}")

        race_name = data['race_name']

        if race_name not in xidata_race_to_skeleton:
            print(f"Warning: No skeleton found for race '{race_name}'. Skipping...")
            continue

        skeleton = xidata_race_to_skeleton[race_name]
        output_path = os.path.join(extract_output_path, "faces", race_name, data['name_clean'], f"{data['name_clean']}.fbx")

        # if the fbx file exists, we skip!
        if not extract_overwrite_existing and os.path.exists(output_path):
            continue

        # ensure the output directory exists
        os.makedirs(os.path.dirname(output_path), exist_ok=True)

        datset = "\n".join([
            "NOESIS_FF11_DAT_SET",
            "",
            f"setPathAbs \"{ffxi_path}\\\"",
            f"dat \"__skeleton\" \"{skeleton}\"",
            f"dat \"gear\" \"{data['dat']}\""
        ])

        # write datset file output
        with open(f"temp.ff11datset", "w", encoding="utf-8") as ff11datset:
            ff11datset.write(datset)

        datset_path = os.path.abspath("temp.ff11datset")

        # build the Noesis command line arguments
        noesis_cmode_args = f"\"{datset_path}\" \"{output_path}\""

        # handle the Noesis export via cmode
        Noesis().cmode(noesis_cmode_args)
