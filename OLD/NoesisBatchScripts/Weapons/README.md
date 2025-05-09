# Batch Exporting Weapons

_Note: NOT Armor_

This one is quite simple in that it can be done via `?cmode` however each time you export a piece of gear it will as you to provide the Skeleton, this obviously prevents full automation. For my purposes I do not need a weapon skeleton (VR) and in most animation or game engine software you will not have animations on the weapon since you'd socket the weapon onto a bone. So I recommend not exporting skeleton with the weapon.

First, we need to setup an AHK that will detect the "Open" dialog and handle it. 

- AHK Code: [NoesisDialogue.ahk](NoesisDialogue.ahk)

Open this file and scroll down to the function `DetectNoesisWindow`

Ensure that this is false:
- `AddSkeleton := false`

This code works by simply looking for ANY Windows Open File dialog box. (Even if it's not Noesis)

Now go into your script code, in this case: 
- PHP Code: [NoesisWeapons.php](NoesisWeapons.php)

Edit the following lines to your hard drive paths:

```php
$saveto   = 'X:\\project-vr\\Weapons\\[[TYPE]]\\[[NAME]]\\[[NAME]].fbx';
$romdir   = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\ROM\\[[ROM]]';
$noesis   = 'X:\\FF11 Movie\\3D - Noesis\\Noesis64.exe';;
```

In this case I am saving to `Weapons`, the `[[TYPE]]` will convert things like Hand, Sword, Staff, etc

These are the arguments I have included, which work good for UE4/5:

```
-noanims -ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11nolodchange  -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale 60
```

It is important we do `-noanims` because we do not want to export animations with these files, as mentioned above you'd socket a weapon onto the skeleton in your game engine or 3d program.

I also highly recommend `-fbxtexrelonly -fbxtexext .png ` which will fix file path names in the output so you do not need to assign the textures yourself, it'll be done for you.

Now the fun begins!

- Start the **AHK NoesisDialogue.ahk** Script and process `F3` 
- You can stop it by pressing `F8`

Next:
- Start the `NoesisWeapons.php` (or your own), eg: `php NoesisWeapons.php`

You should see that it is extracting each piece of gear and automatically CLOSING the Noesis Dialog box.

Now you wait a bit for it to finish :D