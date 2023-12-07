<?php
require_once __DIR__ . "/common.php";

echo("Generating Faces\n");

// load ftables
$ftable = load_ftable();
$ftable_reversed = load_ftable_as_roms();

$face_list = load_list("/in/in_faces.csv");
$race_name = null;
$missing = [];

foreach($face_list as $row) {
    if (empty(trim($row))) {
        continue;
    }

    // if it starts with @, it's a category
    if ($row[0] == "@") {
        $race_name = trim(str_ireplace("@", "", $row));
        continue;
    }

    [$dat, $name] = explode(",", $row);
    $name = preg_replace('/[^\w\s\-]/', '', trim($name));

    // format dat path fully
    $dat = substr_count($dat, "/") > 1 ? "ROM{$dat}.DAT" : "ROM/{$dat}.DAT";
    $dat = str_ireplace("/", "\\", $dat);

    // try get the dat-data
    $file_id = $ftable_reversed[$dat] ?? null;
    $dat_data = $file_id ? $ftable[$file_id] : [
        'dat' => $dat,
    ];

    if ($file_id === null) {
        $missing[] = $dat;
    }

    $arr = [
        "name" => $name,
        "name_clean" => get_simple_name($name),
        "race_index" => array_search($race_name, $races),
        "race_name" => $race_name,
    ];

    $arr = array_merge($arr, $dat_data);

    ksort($arr);

    $output[] = $arr;
}

 // Save
 echo("- Saving: faces.json\n");
 save_data("faces.json", $output);