import os
import json

from noesis import Noesis
from xidata import xidata_race_to_skeleton
from settings import (
    extract_output_path,
    extract_weapons_with_skeleton,
    extract_overwrite_existing,
    ffxi_path
)


def extract_gear():
    print("Extracting gear...")

    # open the data folder and fetch all jsons prefixed with gear_
    files = os.listdir("data")
    files = [f for f in files if f.startswith("gear_") and f.endswith(".json")]

    print(f"Found {len(files)} zone files to process.")
    
    for file in files:
        print(f"Processing file: {file}")

        file_data = open(os.path.join("data", file), "r", encoding="utf-8")
        rows = json.load(file_data)

        print(f"Found {len(rows)} gear in {file}.")

        current = 0
        for data in rows:
            current += 1

            # we only care about armor
            if not extract_weapons_with_skeleton and data['category'] not in ["armor"]:
                continue

            race_name = data['race_name']
            skeleton = xidata_race_to_skeleton[race_name]
            item_id = data['item_id']
            item_name = f"{data['name_clean']}_{item_id}"

            output_path = os.path.join(extract_output_path, "gear", race_name, data['slot'], item_name, f"{item_name}.fbx")

            print(f"{current} / {len(rows)} - {race_name} - ({data['dat']}) {data['name']}")

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
