<?php
require_once __DIR__ . "/common.php";

echo("Building Animations data jsons...\n");

// load ftables
$ftable = load_ftable();
$ftable_reversed = load_ftable_as_roms();
$skeletons = load_skeletons();

// loop through eac race.
$missing = [];
foreach ($races as $race_index => $race_name) {
    if ($race_name == "taru_male" || $race_name == "taru_female") {
        $race_name = "tarutaru";
    }
    
    echo("- Building anims for: {$race_name}\n");

    $anim_list = load_list("/in/in_anim_{$race_name}.csv");
    $category = null;
    $output = [];

    foreach ($anim_list as $row) {
        // if it starts with @, it's a category
        if ($row[0] == "@") {
            $category = str_ireplace("@", "", $row);
            continue;
        }

        [$dat, $name] = explode(",", $row);

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
            "category" => $category,
            "race_index" => $race_index,
            "race_name" => $race_name,
            "skeleton" => $skeletons[$race_name],
        ];

        $arr = array_merge($arr, $dat_data);
        $output[] = $arr;
    }

    // Save
    echo("- Saving: anims_{$race_name}.json\n");
    save_data("anims_{$race_name}.json", $output);
    save_data("anims_missing_file_ids.json", $missing);
}