<?php

// List of roms to extract
$romfile  = 'Weapons.txt';

// folders to everything
$saveto   = 'X:\\project-vr\\Weapons\\[[TYPE]]\\[[NAME]]\\[[NAME]].fbx';
$romdir   = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\ROM\\[[ROM]]';
$noesis   = 'X:\\FF11 Movie\\3D - Noesis\\Noesis64.exe';

// Skele Prompt: (Hume)
// E:/SquareEnix/SquareEnix/FINAL FANTASY XI/ROM/27/82.DAT

// noesis arguments
$args     = '-noanims -ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11nolodchange  -ff11optimizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale 60';

// command structure
$command = "\"{$noesis}\" ?cmode \"{$romdir}\" \"{$saveto}\" {$args}";

// find replacements
$replace  = [ "[[ROM]]",  "[[TYPE]]", "[[NAME]]" ];

// grab all roms
$weapons = file_get_contents($romfile);
$weapons = explode(PHP_EOL, $weapons);

$type = null;

foreach ($weapons as $weapon) {
    $weapon = trim($weapon);

    if (empty($weapon)) {
        continue;
    }

    if ($weapon[0] == '@') {
        $type = substr($weapon, 1);
        $type = str_ireplace('.', '_', $type);
        echo ("New Type: {$type}\n");
        continue;
    }

    [$datfile, $name] = explode(',', $weapon);

    // fix dat file
    $datfile = str_ireplace('/', '\\', $datfile);
    $datfile = "{$datfile}.DAT";

    // format the zone name
    $name = str_ireplace(' ', '_', $name);
    $name = preg_replace("/[^a-z0-9._]+/i", "", $name);

    // buld the command
    $run = str_ireplace($replace, [$datfile, $type, $name], $command);

    // create the required folder for the zone
    $directory = dirname(str_ireplace($replace, [null, $type, $name], $saveto));

    if (!is_Dir($directory)) {
        mkdir($directory, 0777, true);
    }
    
    echo ("Extracting: {$type} - {$name}\n");

    shell_exec($run);

    echo ("> Complete! \n");
}