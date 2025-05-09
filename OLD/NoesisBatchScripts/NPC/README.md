# Batch Exporting NPC


todo - I neeed to update this readme, it is mostly still relevent however I have renamed some of the code and neeed to explain some bits.





------

### Old Readme

> Note: You need about 30GB of disk space for all NPCs

This is the most ghetto and complicated export because it is important that we export each NPC animation separately, this will make things a lot easier.

**Please Note:** You cannot use your PC while this is running, it will take MANY hours, it took me about 8 hours to finish in total.

The first step is to generate a list of NPCs, I have included an `NPC_AHK.txt` so you could skip this step if you like, however you will need to open it and find-replace the FF11 and Output paths.

If you'd like to generate the file yourself:

Open the NoesisNPC.php Code:
- PHP Code: [NoesisNPC.php](NoesisNPC.php)

Ensure the Rom file is correct (it's included in this folder)
- `$romfile  = 'NPC.txt';`

Edit the paths to where your FF11 install is and where you'd like things to save:

```php
// folders to everything
$saveto   = 'X:\\project-vr\\NPC\\[[TYPE]]\\[[NAME]]\\[[NAME]].fbx';
$romdir   = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\ROM[[ROM]]';
```

Now simply run this:

- `php NoesisNPC.php`

It will build a file: `NPC_AHK.txt`

**Now you must remove some entries**

Search the file for:

- `Doll_2` Remove both Doll_2 and Doll_3 (These have skeletons)
- `Goblet_2` I believe this is a bugged entry in AtlanaViewer
- `Weapon_1` You will see Weapon_1 through to Weapon_104, DELETE THEM ALL.
- `Orcish_Warmachine_5` / `Orcish_Warmachine_6` / `Orcish_Warmachine_8` / `Orcish_Warmachine_9` - Delete all these, Again I think bugged entries in AtlanaViewer

You can find these in `NPC_AHK_MISSING.txt` 

Now, Open up Noesis and get ready!

- Run: `NoesisBatchExport.ahk`
- Place your keyboard/mouse somewhere it won't get knocked...
- Start it: F3
- You can stop it with F8

Sit back and watch the chaos :D 

The AHK will simply use keyboard controls to open the file, navigate to the various Export options, then start the export. Once the "Complete" dialogue appears, it will close and repeat. 

It is very fast, however there are some NPCs such as Fomor that have hundreds of animations.

This will export all NPCs and their animations will be in individual files with the correct number of frames.

#### if you need to stop it

Press F8 to stop.

You should have a log.txt file, you can find the last entry it completed, eg: 
```
[10-02-23 13:43:08] Starting: Skeleton_2_26 --> E:\SquareEnix\SquareEnix\FINAL FANTASY XI\ROM\9\109.DAT --> X:\project-vr\NPC\Undead\Skeleton_2_26\Skeleton_2_26.fbx 
```

You can then edit the `NPC_AHK.txt` to remove everything UP TO that entry. Then simply start it again later and it will continue where it left off.