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

// Loop through each race
foreach($races as $race) {
    echo("Processing: {$race} \n");

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
    }
}