# Batch Exporting ARMOR

> Note: We will not export weapons here, we will do that differently. Please see `xidata/Weapons`.

> Note: This batch process involves Auto Hotkey. You cannot use your PC while it's going (but you can stop it)

For armor we will extract: Body, Feet, Face, Hands, Head and Legs.

We will extract tied to the race skeleton but NOT include animations, these can be exported separately much easily.

The armor text files you see here, eg: Hume_Male.txt are a combined CSV from AtlanaViewer via: List > PC > (Race). Then adding types via @body etc.

### Setup

Edit the following file: `Armor_PHP_Batch_Settings.php`

Change all the paths and arguments to your liking.

### Batch Exporting

This script uses a combination of PHP and AHK. Use ChatGPT to convert the PHP to something of your liking if you prefer!

The PHP will automatically write out a `AHK_SKELETON.txt` file with the current skeleton it is working with, this is then picked up by the Auto Hotkey code to correctly set the skeleton during the "Open" dialog that will popup.

The AHK is simply waiting for the "Open" popup and will handle it, it does not do anythign else.

To get going:

- Double-Click `Armor_AHK_Noesis_Script.ahk`
- Start AHK: **F3**
- Open Powershell (DO NOT USE cmder)
- Start the code: `php Armor_PHP_Batch_Export.php`

To stop:
- Stop AHK: **F8**
- CTRL+C the PHP 
