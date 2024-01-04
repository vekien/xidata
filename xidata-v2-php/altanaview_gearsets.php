<?php
require_once __DIR__ . "/common.php";

echo("Generating GearSets CSV\n");

// Grab altana gearsets
$altana_viewer_ini = "E:\\FF11 Tools\\3D - Altana Viewer\\Gearsets.ini";
$altana_viewer_gearsets = parse_ini_file($altana_viewer_ini, true);

// grab some jsons
$faces = load_json("\\out\\faces.json");

$sets = [];
foreach($altana_viewer_gearsets as $gearset_name => $gearset) {
    $arr = [
        "name" => $gearset_name,
    ];

    echo("Gearset: {$gearset_name}\n");

    // Get the race data
    $race_dat = get_simple_datname($gearset["Race"]);
    $race_name = array_search($race_dat, $races_to_dats);
    $race_index = array_search($race_name, $races);
    $race_altana = $races_altanaviewer[$race_index];
    
    $arr["race"] = $race_name;

    // Get the face
    $face_dat = get_simple_datname($gearset["Face"]);
    $face_dat = get_face_dat($face_dat);
    $arr["face"] = get_windows_name($face_dat['name']);

    // Do each slot!
    foreach($slots_altana as $slot_index => $slot_altana) {
        // grab the true slot name (not the altana one)
        $slot_name = $slots[$slot_index];

        // grab the dat name all cleaned up from the gearset
        $slot_dat = get_simple_datname($gearset[$slot_altana]);

        // grab the name from the altanaviewer list as that matches my unreal engine for now...
        $altana_name = search_altana_for_name($race_altana, $slot_altana, $slot_dat);
        $windows_name = get_windows_name($altana_name);

        // store against the true name
        $arr[$slot_name] = $windows_name;
    }

    print_r($arr);

    $sets[] = $arr;
    echo("\n");
}

// save
echo("- Saving: gearsets.json\n");
save_custom("gearsets.json", $sets);



// --------------------------------------------------------------------------------

//
// This searches altana files for the name and returns it that fits our windows one.
//
function search_altana_for_name($race_altana, $slot_altana, $dat) {
    $search_data = "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\{$race_altana}\\{$slot_altana}.csv";

    $search_data = file_get_contents($search_data);
    $search_data = explode("\n", $search_data);

    // convert back to altana viewer format
    $dat = str_ireplace("\\", "/", $dat);

    foreach ($search_data as $line) {
        // skip empty lines and skip lines without a comma (main contains categories)
        if (empty(trim($line))) continue;
        if (stripos($line, ",") === false) continue;

        // grab dat and name
        [$altana_dat, $altana_name] = explode(",", $line);

        if (trim($dat) == trim($altana_dat)) {
            return $altana_name;
        }
    }

    return null;
}

//
// Formats the dat name correctly for this generation
//
function get_simple_datname($string) {
    // It will always start with 1/ so we can just remove this
    $string = substr($string, 2);

    // Swap the slashes
    $string = str_ireplace("/", "\\", $string);
    
    return $string;
}

//
// Find the face dat
//
function get_face_dat($dat) {
    global $faces;

    foreach($faces as $face_dat) {
        if ($face_dat["dat"] == "ROM\\{$dat}.DAT") {
            return $face_dat;
        }
    }

    return null;
}