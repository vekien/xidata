import os
import json

from noesis import Noesis
from settings import (
    extract_output_path,
    extract_overwrite_existing,
    ffxi_path
)


def extract_trusts():
    print("Extracting Trusts...")

    # open the hume male gear json file
    print("Loading gear data...")
    with open("data/gear_hume_male.json", "r", encoding="utf-8") as file:
        trusts_data = json.load(file)

    print(f"Found {len(trusts_data)} weapons to process.")

    # Open noesis
    Noesis().open()

    current = 0
    for data in trusts_data:
        current += 1
        

        name_clean = data['name_clean']
        skeleton = data.get('skeleton', None)
        anims = data.get('anims', None)

        print(f"{current} / {len(trusts_data)} - {name_clean}")

        for dat in anims:
            # this needs to ber the npc one
            output_path = os.path.join(extract_output_path, data['output_path'], f"{name_clean}.fbx")

            # if the fbx file exists, we skip!
            if not extract_overwrite_existing and os.path.exists(output_path):
                continue

            # ensure the output directory exists
            os.makedirs(os.path.dirname(output_path), exist_ok=True)

            # handle the Noesis export via cmode
            Noesis().export(input_path, output_path)

            # Cleanup the noefbxmulti files
            Noesis().noefbxmulti_cleanup(output_path_temp)
