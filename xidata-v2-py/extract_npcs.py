import os
import json

from noesis import Noesis
from settings import (
    extract_output_path,
    extract_overwrite_existing,
    ffxi_path
)


def extract_npcs():
    print("Extracting NPCs...")

    npc_filter = []

    # if the file "npc_filter.json" exists, open it
    if os.path.exists("npc_filter.json"):
        print("NPC filter list found!")
        with open("npc_filter.json", "r", encoding="utf-8") as file:
            npc_filter = json.load(file)

    # open the data folder and fetch all jsons prefixed with npc_
    files = os.listdir("data")
    files = [f for f in files if f.startswith("npc_") and f.endswith(".json")]

    print(f"Found {len(files)} npc files to process.")

    # reset the npc broken file
    if os.path.exists("npc_broken.txt"):
        os.remove("npc_broken.txt")

    # Open noesis
    Noesis().open()

    for file in files:
        print(f"Processing file: {file}")

        if file == "npc_character2.json":
            continue

        file_data = open(os.path.join("data", file), "r", encoding="utf-8")
        rows = json.load(file_data)

        print(f"Found {len(rows)} npcs in {file}.")

        current = 0
        for data in rows:
            current += 1

            # some of the dats are borked.
            dat = data['dat']
            if len(dat.split("\\")) != 3:
                with open("npc_broken.txt", "a") as f:
                    f.write(f"broken dat: {data}\n")
                continue

            # if we have an npc filter list, we will check if the dat is within the list
            if npc_filter and data['dat'] not in npc_filter:
                continue

            print(f"{current} / {len(rows)} - {file} - ({data['dat']}) {data['name']}")

            rom = data['dat'].split("\\")[0]
            input_path = os.path.join(ffxi_path, data['dat'])
            output_path_temp = os.path.join(extract_output_path, "npcs", data['category'], data['name_clean'], rom, str(data['num']))
            output_path = os.path.join(output_path_temp, f"{data['name_clean']}_{data['num']}.fbx")

            # if the fbx file exists, we skip!
            if not extract_overwrite_existing and os.path.exists(output_path):
                continue

            # ensure the output directory exists
            os.makedirs(os.path.dirname(output_path), exist_ok=True)

            # handle the Noesis export via cmode
            Noesis().export(input_path, output_path)

            # Cleanup the noefbxmulti files
            Noesis().noefbxmulti_cleanup(output_path_temp)
