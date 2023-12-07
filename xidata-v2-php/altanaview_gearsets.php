<?php
require_once __DIR__ . "/common.php";

echo("Generating GearSets CSV\n");

// Grab altana gearsets
$altana_viewer_ini = "E:\\FF11 Tools\\3D - Altana Viewer\\Gearsets.ini";
$altana_viewer_gearsets = parse_ini_file($altana_viewer_ini, true);

// grab some jsons
$faces = load_json("\\out\\faces.json");

// Build a huge ass array for gear
echo("Building gearlist ...\n");
$gear = [];
foreach($races as $race) {
    $gear[$race] = json_decode(file_get_contents(__DIR__ . "/out/gear_{$race}.json"), true);
}
echo("- Complete!");

$sets = [];
foreach($altana_viewer_gearsets as $gearset_name => $gearset) {
    $arr = [
        "name" => $gearset_name,
    ];

    echo("Gearset: {$gearset_name}\n");

    // get race name
    $race_dat = get_simple_datname($gearset["Race"]);
    $race_name = array_search($race_dat, $races_to_dats);
    $arr["race"] = $race_name;

    // Get the face
    $face_dat = get_simple_datname($gearset["Face"]);
    $face_dat = get_face_dat($face_dat);
    $arr["face"] = $face_dat;

    echo("- Race: {$race_name}\n");
    echo("- Face: {$face_dat['name']}\n");

    // gear json for this race
    $gear_dats = $gear[$race_name];

    // Do each slot!
    foreach($slots_altana as $slot_index => $slot_altana) {
        // grab the true slot name (not the altana one)
        $slot_name = $slots[$slot_index];

        // grab the dat name
        $slot_dat = get_simple_datname($gearset[$slot_altana]);

        echo("- {$slot_name}: {$slot_dat}\n");

        // store against the true name
        $arr[$slot_name] = get_gear_dat($slot_dat, $gear_dats);
    }

    $sets[] = $arr;
    echo("\n");
}

// save
echo("- Saving: gearsets.json\n");
save_custom("gearsets.json", $sets);

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

//
// 
//
function get_gear_dat($dat, $gear_dats) {
    foreach($gear_dats as $gear_dat) {
        if ($gear_dat["dat"] == "ROM\\{$dat}.DAT") {
            return $gear_dat;
        }
    }

    return null;
}

