# xidata

This houses all my various data and scripts for extracting resources from FF11

You can find information on batch exporting:
- Noesis > Animations
- Noesis > Armor
- Noesis > NPCs
- Noesis > NPCs 2 (using Look data, random NPCs you see about)
- Noesis > Weapons
- Noesis > Zones
- PolUtils > Music
- PolUtils > Sound Effects

# xidata-v2 batch app

This tool was created to automate the batch exporting of FFXI content via Noesis and ensuring a consistent format, structure and output for Game/3D Development.

> Note: **Once you set your settings, when you launch the app again it will take a couple seconds as it builds a local data list of JSON files to make batch export simplier**

**Features include**

- Full automation of ff11datsets
- Handling noemultifbx to split animations with correct frame length
- Correct skeletons for gear
- Managing data extract using Altana Lists.

All source code is available. Download the current version in the Releases secton.

![image](https://user-images.githubusercontent.com/270800/220974133-c7fc950a-9e5a-4364-b87f-bba478141804.png)

## Batch Export Collision Mesh

#### https://github.com/MurphyCodes/FFxi-Navmesh-Builder

MurphyCodes/Xenonsmurf create an excellent tool that can do this for you. Go to his repository to find a NavMesh builder.

You can import these straight into UE4 as map collision meshes.
