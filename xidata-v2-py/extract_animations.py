import os
import json
import shutil

from noesis import Noesis
from xidata import xidata_race_to_skeleton
from settings import (
    extract_output_path,
    extract_overwrite_existing,
    ffxi_path
)


def extract_animations():
    print("Extracting Animations...")

    # open the data folder and fetch all jsons prefixed with anim_
    files = os.listdir("data")
    files = [f for f in files if f.startswith("anims_") and f.endswith(".json")]

    print(f"Found {len(files)} animation files to process.")
    current = 0

    # Open noesis
    Noesis().open()

    for file in files:
        print(f"Processing file: {file}")

        file_data = open(os.path.join("data", file), "r", encoding="utf-8")
        rows = json.load(file_data)

        print(f"Found {len(rows)} npcs in {file}.")

        for data in rows:
            current += 1

            # ignore the basic stuff
            if "file_id" not in data:
                continue

            if data['category'] != "basic" or "anims_hume_male.json" != file:
                continue

            race_name = data['race_name']
            output_path_temp = os.path.join(extract_output_path, "animations", race_name, f"{data['category']}_{data['name_clean']}_{data['file_id']}")
            output_path_basic = os.path.join(extract_output_path, "animations", race_name, "basic")
            input_path = os.path.join(ffxi_path, data['dat'])
            output_path = os.path.join(output_path_temp, f"{data['name_clean']}_{data['file_id']}.fbx")

            print(f"{current} / {len(rows)} - {race_name} - Extracting: {data['category']} {data['name']}")

            # if the fbx file exists, we skip!
            if not extract_overwrite_existing and os.path.exists(output_path):
                continue

            # ensure the output directory exists
            os.makedirs(os.path.dirname(output_path), exist_ok=True)

            # build skeleton
            skeleton = xidata_race_to_skeleton[race_name]
            skeleton = os.path.join(ffxi_path, skeleton)

            # if the skeleton and the input path match, set it to None so we dont use it, as it'll be the base anim
            if skeleton == input_path:
                skeleton = None

            # handle the Noesis export via cmode
            Noesis().export(input_path, output_path, skip_textures=True, skeleton=skeleton)

            # Cleanup the noefbxmulti files
            Noesis().noefbxmulti_cleanup(output_path_temp)

            # special setup for basic, as it's 3 separate dats. We will move all files into 1 folder
            if data['category'] == "basic":
                os.makedirs(output_path_basic, exist_ok=True)

                # move all files from output_path_temp/* to output_path_basic
                for file_name in os.listdir(output_path_temp):
                    source = os.path.join(output_path_temp, file_name)
                    destination = os.path.join(output_path_basic, file_name)
                    shutil.move(source, destination)

                shutil.rmtree(output_path_temp)