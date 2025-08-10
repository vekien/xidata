import os
import json

from noesis import Noesis
from settings import (
    extract_output_path,
    extract_overwrite_existing,
    ffxi_path
)


def extract_zones():
    print("Extracting zones...")

    # open the data folder and fetch all jsons prefixed with zone_
    files = os.listdir("data")
    files = [f for f in files if f.startswith("zones_") and f.endswith(".json")]

    print(f"Found {len(files)} zone files to process.")

    for file in files:
        print(f"Processing file: {file}")

        file_data = open(os.path.join("data", file), "r", encoding="utf-8")
        rows = json.load(file_data)

        expansion_name = file.replace("zones_", "").replace(".json", "")

        print(f"\nFound {len(rows)} zones in {file}.\n")

        current = 0
        for data in rows:
            current += 1
            print(f"{current} / {len(rows)} - {file} - ({data['dat']}) {data['name']}")

            # if "xarcabard_s" not in data['name_clean'].lower():
            #     continue

            input_path = os.path.join(ffxi_path, data['dat'])
            output_path_temp = os.path.join(extract_output_path, "zones", expansion_name, data['name_clean'])
            output_path = os.path.join(output_path_temp, f"{data['name_clean']}.fbx")

            # if the fbx file exists, we skip!
            if not extract_overwrite_existing and os.path.exists(output_path):
                continue

            # ensure the output directory exists
            os.makedirs(os.path.dirname(output_path), exist_ok=True)

            # build the Noesis command line arguments
            noesis_cmode_args = f"\"{input_path}\" \"{output_path}\""

            # handle the Noesis export via cmode
            Noesis().cmode(noesis_cmode_args)

            # cleanup the output directory
            Noesis().texture_cleanup(output_path_temp)
