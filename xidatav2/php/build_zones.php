<?php
require_once __DIR__ . "/common.php";

echo("Building Zone data jsons...\n");

// load ftables
$ftable = load_ftable();
$ftable_reversed = load_ftable_as_roms();

// List of zones in key format
$zones = [
    "base_game",
    "rize_of_zilart",
    "chains_of_promathia",
    "treasures_of_aht_urhgan",
    "wings_of_the_goddess",
    "abyssea",
    "seekers_of_adoulin",
    "rhapsodies",
    "subrooms"
];

// Load zone data
$zone_info = load_json("/in/in_zone_info.json");

// Loop through zones
foreach ($zones as $zone_key) {
    echo("- Processing: {$zone_key}\n");
    $output = [];

    // Load stuff
    $info = $zone_info[$zone_key];
    $data = load_list("/in/in_zones_{$zone_key}.txt");

    // loop through zones
    foreach ($data as $line) {
        if (empty($line)) continue;

        [$name, $dat] = explode(",", $line);

        $name = trim($name);
        $dat = trim($dat);
        $dat = str_ireplace("/", "\\", $dat); 

        // try get the dat-data
        $file_id = $ftable_reversed[$dat];
        $dat_data = $ftable[$file_id];

        $arr = [
            "name" => $name,
            "category" => $info['category'],
            "file_rom" => $dat,
            "file_id" => $file_id,
        ];

        $arr = array_merge($arr, $dat_data);
        $output[] = $arr;
    }

    save_data("zones_{$zone_key}.json", $output);
}

echo ("\n✓ All zone data exported.\n");