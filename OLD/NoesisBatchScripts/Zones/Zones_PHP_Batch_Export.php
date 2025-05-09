<?php

// List of roms to extract
$roms = [
    'Zones/Zones_BaseGame.txt',
    'Zones/Zones_RizeOfZilart.txt',
    'Zones/Zones_ChainsOfPromathia.txt',
    'Zones/Zones_TreasuresOfAhtUrhgan.txt',
    'Zones/Zones_WingsOfTheGoddess.txt',
    'Zones/Zones_Abyssea.txt',
    'Zones/Zones_Rhapsodies.txt',
    'Zones/Zones_SeekersOfAdoulin.txt'
];

require_once('Zones_PHP_Batch_Settings.php');

// command structure
$command = "\"{$noesis}\" ?cmode \"{$romdir}\" \"{$saveto}\" {$args}";

// find replacements
$replace = [ "[[ROM]]", "[[EXPANSION]]", "[[NAME]]" ];

foreach ($roms as $romfile) {
    // grab all roms
    $dats = file_get_contents($romfile);
    $dats = explode(PHP_EOL, $dats);

    $expansion = explode("_", $romfile)[1];
    $expansion = explode(".", $expansion)[0];

    echo ("----- Starting: {$expansion} -----\n");

    foreach ($dats as $dat) {
        $dat = trim($dat);

        if (empty($dat)) {
            continue;
        }

        [$zonename, $datfile] = explode('	', $dat);

        // format the zone name
        $zonename = str_ireplace(' ', '_', $zonename);
        $zonename = preg_replace("/[^a-z0-9._]+/i", "", $zonename);

        // buld the command
        $run = str_ireplace($replace, [$datfile, $expansion, $zonename], $command);

        // create the required folder for the zone
        $directory = dirname(str_ireplace($replace, [null, $expansion, $zonename], $saveto));

        if (!is_Dir($directory)) {
            mkdir($directory, 0777, true);
        }
        
        echo ("Extracting: {$expansion} - {$zonename}\n");
        shell_exec($run);
        echo ("> Complete! \n");
    }
}