<?php

$main   = file_get_contents("Main_Model_IDs.csv");
$ranged = file_get_contents("Ranged_Model_IDs.csv");
$sub    = file_get_contents("Sub_Model_IDs.csv");

function csv_to_json($csv, $type) {
    $csv = explode("\n", $csv);
    $csv = array_values(array_filter($csv));
    unset($csv[0]);

    $arr = [];
    foreach ($csv as $row) {
        $row = str_getcsv($row);

        $arr[$row[2]] = $row[1];
    }

    return $arr;
}

$gear_ids = [
    "main" => csv_to_json($main, "main"),
    "ranged" => csv_to_json($ranged, "ranged"),
    "sub" => csv_to_json($sub, "sub"),
];

file_put_contents("LookToWeapons.json", json_encode($gear_ids, JSON_PRETTY_PRINT));

$race_list = [
    1 => "Hume_Male",
    2 => "Hume_Female",
    3 => "Elvaan_Male",
    4 => "Elvaan_Female",
    5 => "Tarutaru",
    6 => "Mithra",
    7 => "Galka"
];

$face_list = [
    0 => "F1A",
    1 => "F1B",
    2 => "F2A",
    3 => "F2B",
    4 => "F3A",
    5 => "F3B",
    6 => "F4A",
    7 => "F4B",
    8 => "F5A",
    9 => "F5B",
    10 => "F6A",
    11 => "F6B",
    12 => "F7A",
    13 => "F7B",
    14 => "F8A",
    15 => "F8B",
    26 => "NPC1",
    27 => "NPC2",
    28 => "NPC3",
    29 => "Fomor",
    30 => "Mannequin",
];

$lookToFile = file_get_contents("LookTable.json");
$lookToFile = json_decode($lookToFile, true);

$gearFile = file_get_contents("gear_1.json");
$gearFile = json_decode($gearFile, true);

$weaponFile = file_get_contents("LookToWeapons.json");
$weaponFile = json_decode($weaponFile, true);

// format = face,race,head,body,hands,legs,feet,main,sub,range
$model = "29,1,199,77,159,62,195,426,51,0";
$model = explode(",", $model);

$face = $face_list[$model[0]];
$race = $race_list[$model[1]];

function get_gear($type, $model_id) {
    global $race, $lookToFile;

    $list = (array)$lookToFile[$race][$type];

    foreach ($list as $l) {
        if ($l['ModelID'] == $model_id) {
            return get_gear_name($l['Path'], $type);
        }
    }

    return null;
}

function get_gear_name($dat, $type) {
    global $gearFile, $race;

    $list = (array)$gearFile[$race]["Gear"][$type];

    $dat = str_ireplace("ROM","", $dat);
    $dat = str_ireplace(".DAT","", $dat);
    $dat = str_ireplace("/","\\", $dat);

    foreach ($list as $l) {
        $l = explode("|", $l);

        if ($dat == $l[1]) {
            return $l[0];
        }
    }

    return "(could not find {$dat})";
}

function get_weapon($type, $model_id) {
    global $weaponFile;

    if ($model_id == 0) {
        return "";
    }

    $name = $weaponFile[$type][$model_id] ?? null;

    if ($name == null && $type == "main") {
        $name = $weaponFile["sub"][$model_id] ?? null;
    }

    return $name ?? "(could not find {$type} = {$model_id})";
}


$head = get_gear("Head", $model[2]);
$body = get_gear("Body", $model[3]);
$hands = get_gear("Hands", $model[4]);
$legs = get_gear("Legs", $model[5]);
$feet = get_gear("Feet", $model[6]);

$main = get_weapon("main", $model[7]);
$sub = get_weapon("sub", $model[8]);
$ranged = get_weapon("ranged", $model[9]);

print_r([
    "face" => $face,
    "race" => $race,
    "head" => $head,
    "body" => $body,
    "hands" => $hands,
    "legs" => $legs,
    "feet" => $feet,
    "main" => $main,
    "sub" => $sub,
    "ranged" => $ranged,
]);
