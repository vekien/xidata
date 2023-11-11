<?php

$lookTable = [];

/**
 * Grab the current look table as we're only going to be replacing: Body, Head, Hands, Feet and Legs,
 * Face won't be changing and I haven't done Main/Range/Sub yet.
 */
$look_table = file_get_contents(__DIR__ .'/LookTable.json');
$look_table = json_decode($look_table, true);

// now load our up to date look to file list
$look_to_file_list = file_get_contents(__DIR__ .'/LookToFile_List.json');
$look_to_file_list = json_decode($look_to_file_list, true);

echo("Building Look Table JSON for xi data...\n");

// Loop through the look to file and replace the entries in look table
foreach($look_to_file_list as $race => $slot_models) {
    foreach ($slot_models as $slot => $dats) {
        $slot = ucwords($slot);

        foreach ($dats as $i => $datpath) {
            [$model_id, $file_id, $file_path] = explode("|", $datpath);

            $look_table[$race][$slot][$i] = [
                'FileID' => $file_id,
                'ModelID' => $model_id,
                'Path' => str_ireplace("\\", "/", $file_path)
            ];
        }
    }
}

// Store for simplicity
file_put_contents(__DIR__ .'/LookTable.json', json_encode($look_table, JSON_PRETTY_PRINT));

echo("Done!\n\n");