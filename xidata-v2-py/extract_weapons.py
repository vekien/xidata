import os
import json

from noesis import Noesis
from settings import (
    extract_output_path,
    extract_overwrite_existing,
    ffxi_path
)


def extract_weapons():
    print("Extracting gear...")

    # open the hume male gear json file
    print("Loading gear data...")
    with open("data/gear_hume_male.json", "r", encoding="utf-8") as file:
        weapons_data = json.load(file)

    weapons_data = [d for d in weapons_data if d.get("category") == "weapon"]

    print(f"Found {len(weapons_data)} weapons to process.")

    current = 0
    for data in weapons_data:
        current += 1
        print(f"{current} / {len(weapons_data)} - ({data['dat']}) {data['name']}")

        item_id = data['item_id']
        item_name = f"{data['name_clean']}_{item_id}"

        output_path = os.path.join(extract_output_path, "weapons", data['slot'], item_name, f"{item_name}.fbx")

        # if the fbx file exists, we skip!
        if not extract_overwrite_existing and os.path.exists(output_path):
            continue

        # ensure the output directory exists
        os.makedirs(os.path.dirname(output_path), exist_ok=True)

        datset = "\n".join([
            "NOESIS_FF11_DAT_SET",
            "",
            f"setPathAbs \"{ffxi_path}\\\"",
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
