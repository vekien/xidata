<?php

require_once('Armor_PHP_Batch_Settings.php');

$races = [
    "Hume_Male",
    "Hume_Female",
    "Elvaan_Male",
    "Elvaan_Female",
    "Tarutaru",
    "Mithra",
    "Galka"
];

// This will use romdir
$racesSkeletons = [
    "Hume_Male"     => "27\\82.DAT",
    "Hume_Female"   => "32\\58.DAT",
    "Elvaan_Male"   => "37\\31.DAT",
    "Elvaan_Female" => "42\\4.DAT",
    "Tarutaru"      => "46\\93.DAT",
    "Mithra"        => "51\\89.DAT",
    "Galka"         => "56\\59.DAT"
];

// command structure
$command = "\"{$noesis}\" ?cmode \"{$romdir}\" \"{$saveto}\" {$args}";

// find replacements
$replace = [ "[[RACE]]", "[[ROM]]",  "[[TYPE]]", "[[NAME]]" ];

// the name of the AHK skelet file
$ahkSkeletonFilename = 'AHK_SKELETON.txt';

// Loop through each race
foreach($races as $race) {
    echo("Processing: {$race} \n");
    // Grab the skeleton, inject the rom and swap the path slashes for Windows Open Dialog.
    $skeleton = $racesSkeletons[$race];
    $skeleton = str_ireplace("[[ROM]]", $skeleton, $romdir);

    // write out skeleton file
    file_put_contents(__DIR__ ."/{$ahkSkeletonFilename}", $skeleton);

    # --------------------------------------

    // Read the Armor file
    $armorDats = file_get_contents(__DIR__ ."/Armor_{$race}.txt");
    $armorDats = explode("\n", $armorDats);

    // Current type, used for organising the files
    $type = null;

    // Go through all the armor dats!
    foreach ($armorDats as $datname) {
        $datname = trim($datname);

        if (empty($datname)) {
            continue;
        }

        // If the datname starts with @ then it's a category
        if ($datname[0] == '@') {
            $type = substr($datname, 1);
            $type = str_ireplace('.', '_', $type);
            echo ("New Type: {$type}\n");
            continue;
        }

        // get the parts
        [$dat, $name] = explode(',', $datname);

        $dat = trim($dat);
        $name = trim($name);

        // fix dat file
        $dat = str_ireplace('/', '\\', $dat);
        $dat = "{$dat}.DAT";

        // format the armor name into just: alphanumeric and _
        $name = str_ireplace(' ', '_', trim($name));
        $name = preg_replace("/[^a-z0-9_]+/i", "", $name);

        // Minor fix for some names....
        if ($name[0] == '_') {
            $name = substr($name, 1);
        }

        $replaceValues = [$race, $dat, $type, $name];

        // build the command
        $run = str_ireplace($replace, $replaceValues, $command);

        // create the required folder for the zone
        $directory = dirname(str_ireplace($replace, $replaceValues, $saveto));

        if (!is_Dir($directory)) {
            mkdir($directory, 0777, true);
        }

        echo ("Extracting: {$type} - {$name}\n");
        shell_exec($run);
    }
}