<?php
require_once __DIR__ . "/common.php";

echo("Building AltanaViewer data jsons...\n");

/**
 * Loop through all altana viewer data files and builds a json.
 */

$ftable = load_ftable();
$ftable_reversed = load_ftable_as_roms();

$altana_files_npcs = [
    [ "Monster", "Abyssea", "abyssea.csv", ],
    [ "Monster", "Amorph", "amorphs.csv", ],
    [ "Monster", "Aquan", "aquans.csv", ],
    [ "Monster", "Arcana", "arcana.csv", ],
    [ "Monster", "Beast", "beasts.csv", ],
    [ "Monster", "Bird", "birds.csv", ],
    [ "Monster", "Demons", "demons.csv", ],
    [ "Monster", "Dragon", "dragons.csv", ],
    [ "Monster", "Elemental", "elementals.csv", ],
    [ "Monster", "Promyvion", "empty.csv", ],
    [ "Monster", "Lizard", "lizards.csv", ],
    [ "Monster", "Naakual", "naakuals.csv", ],
    [ "Monster", "Odyssey", "odyssey.csv", ],
    [ "Monster", "Awoken", "peculiar_foes.csv", ],
    [ "Monster", "Plantoid", "plantoids.csv", ],
    [ "Monster", "Sea", "sea.csv", ],
    [ "Monster", "Undead", "undead.csv", ],
    [ "Monster", "Vermin", "vermin.csv", ],
    [ "Summon", "Avatar", "avatars.csv", ],
    [ "Beastmen", "Beastmen", "beastmen.csv", ],
    [ "Object", "Chair", "chairs.csv", ],
    [ "Object", "Event", "event.csv", ],
    [ "Object", "Object", "objects.csv", ],
    [ "Creature", "Creature", "creatures.csv", ],
    [ "Gods", "Supreme Being", "gods.csv", ],
    [ "Mount", "Mount", "mounts.csv", ],
    [ "Character", "Other", "misc.csv", ],
    [ "Character", "Story", "npcs.csv", ],
    [ "Character", "Trust", "trusts.csv", ],
];

// Parse AltanaViewer
function get_folder_list_from_string($input_string) {
    $segments = explode(';', $input_string);
    $result = [];
    $default_folder = null;

    foreach ($segments as $segment) {
        
        if (preg_match('/^(\d+)\/(\d+(?:-\d+)?)$/', $segment, $matches)) {
            $folder = $matches[1];
            $range = $matches[2];

            if (strpos($range, '-') !== false) {
                list($start, $end) = explode('-', $range);

                for ($i = $start; $i <= $end; $i++) {
                    $result[] = $folder . '/' . $i;
                }
            } else {
                $result[] = $segment;
            }

            $default_folder = $folder;
        } else {
            if (strpos($segment, '-') !== false) {
                list($start, $end) = explode('-', $segment);
                $default_folder = null;

                if (substr_count($start, "/") > 0) {
                    $temp_start = explode("/", $start);
                    $start = end($temp_start);

                    $default_folder = count($temp_start) > 2 ? "{$temp_start[0]}/{$temp_start[1]}" : "{$temp_start[0]}";
                }

                for ($i = $start; $i <= $end; $i++) {
                    $result[] = $default_folder . '/' . $i;
                }
            } else {
                $result[] = $default_folder . '/' . $segment;
            }
        }
    }

    foreach ($result as $i => $res) {
        $res = $res[0] == "/" ? substr($res, 1) : $res;
        $res = substr($res, 0, 2) == "1/" ? substr($res, 2) : $res;

        $result[$i] = $res;
    }

    return $result;
}

// Parse model id from look
// Credit: Shozokui
function get_model_id_for_npc($look_string) {
    $hexedit1 = substr($look_string, 6, 4);
    $hex2 = substr($hexedit1, 0, 2);
    $hex3 = substr($hexedit1, 2, 2);
    $hex = "0x" . $hex3 . $hex2;

    $dec = hexdec($hex);
    $modelval = 0;

    if ($dec < 1500) {
        $modelval = $dec + 1300;
    } elseif ($dec < 3000) {
        $modelval = $dec + 50295;
    } elseif ($dec < 3500) {
        $modelval = $dec + 96907;
    } elseif ($dec < 4000) {
        $modelval = $dec + 98239;
    }

    return $modelval;
}

// Build Dat to "Look" ref
$mobdb = [];
$in_mobdb = load_list("\\in\\in_mobdb.csv");
foreach ($in_mobdb as $line) {
    if (empty($line)) continue;

    [$name, $look, $dat, $header] = str_getcsv($line);

    $dat = str_ireplace("/", "\\", $dat);

    $mobdb[$dat] = [
        "name" => str_ireplace("_", " ", $name),
        "look" => $look
    ];
}


$missing = [];
$found = [];
$output = [];
foreach ($altana_files_npcs as $af_npc) {
    [ $type, $category, $filename ] = $af_npc;

    echo ("- Processing: {$category} - {$type} - {$filename}\n");

    // load
    $file = load_list("\\in\\in_altana_{$filename}");

    foreach ($file as $line) {
        if (empty($line)) continue;

        // grab dat paths and the name
        [$dat_paths, $name] = explode(",", $line);

        echo ("-- Entry: {$name}\n");

        // parse folders
        $dat_paths = get_folder_list_from_string($dat_paths);

        // store each one individually.
        foreach ($dat_paths as $i => $dat) {
            $num = $i + 1;

            // build dat
            $dat = $dat = substr_count($dat, "/") > 1 ? "ROM{$dat}.DAT" : "ROM/{$dat}.DAT";
            $dat = str_ireplace("/", "\\", $dat);

            // try get the dat-data
            $file_id = $ftable_reversed[$dat] ?? null;
            $dat_data = $file_id ? $ftable[$file_id] : [
                'dat' => $dat,
            ];

            if ($file_id === null) {
                $missing[] = $dat;
            }   
            
            $mob_data = isset($mobdb[$dat]) ? $mobdb[$dat] : [
                'name' => $name,
                'look' => null,
            ];

            $arr = [
                "num" => $num,
                "name_short" => $name,
                "name_full" => "{$name} - {$num}",
                "category" => $category,
                "type" => $type,
                "dat" => $dat,
            ];

            $arr = array_merge($arr, $dat_data);
            $arr = array_merge($arr, $mob_data);

            // add model_id if look exists
            $arr['model_id'] = $arr['look'] ? get_model_id_for_npc($arr['look']) : null;

            ksort($arr);

            $found[$dat] = 1;
            $output[$type][] = $arr;
        }
    }
}

echo("\nBuilding AltanaViewer full datamine jsons...\n");

// Process full datamine from: Shozokui
$full_datamine = load_list("\\in\\in_altana_full_datamine.csv");
foreach ($full_datamine as $line) {
    if (empty($line)) continue;

    [$dat, $name] = explode(",", $line);

    $dat = $dat = substr_count($dat, "/") > 1 ? "ROM{$dat}.DAT" : "ROM/{$dat}.DAT";
    $dat = str_ireplace("/", "\\", $dat);

    // Skip existing.
    if (isset($found[$dat])) continue;

    // try get the dat-data
    $file_id = $ftable_reversed[$dat] ?? null;
    $dat_data = $file_id ? $ftable[$file_id] : [
        'dat' => $dat,
    ];

    if ($file_id === null) {
        $missing[] = $dat;
    }   
    
    $mob_data = isset($mobdb[$dat]) ? $mobdb[$dat] : [
        'name' => $name,
        'look' => null,
    ];
    
    $name = explode(":", $name);
    $type = trim($name[0]) . "2";
    $type = $type == "MOB2" ? "Monster2" : $type;
    $type = $type == "NPC2" ? "Character2" : $type;
    $name = trim($name[1]);

    $arr = [
        "num" => 0,
        "name" => $name,
        "name_full" => $name,
        "category" => "Unknown",
        "type" => $type,
        "dat" => $dat,
    ];

    $arr = array_merge($arr, $dat_data);
    $arr = array_merge($arr, $mob_data);

    // add model_id if look exists
    $arr['model_id'] = $arr['look'] ? get_model_id_for_npc($arr['look']) : null;

    ksort($arr);

    $output[$type][] = $arr;
}

echo("\nFinished! Saving....\n");
foreach ($output as $type => $data) {
    echo("- Save: {$type} \n");
    save_data("npc_". strtolower($type) .".json", $data);
}


