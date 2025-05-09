<?php
require_once __DIR__ . "/common.php";

echo("Starting brute force ftable scan for model id, file ids, and dats.\n\n");

/**
 * This builds out a full list of Model IDs to their respective DAT Paths. Model IDs 
 * are sent by the server to describe the "look" for a character or NPC. This is useful
 * if you need to parse the LandSandBoat NPC DB list and convert the "look" (using LookDecoder.php)
 * and then convert those slot ids (head, body, etc) to their respective Dat File for each race.
 * 
 * I want togive a huge thank you to Xenonsmurf and Atom0s for their invaluable knowledge 
 * and work on this area of the game. Wouldn't have been possible without them.
 * 
 * - Xenonsmurf: https://github.com/MurphyCodes
 * - Atom0s: https://atom0s.com/
 * 
 * Notes: This mainly works for Armor, not Weapons/Range because they have very different
 *        header variations and I need to write them all down, but I don't need that for Unreal Engine so...
 */

function get_dat_for_file_number($file_num) {
    # $VTableFile = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI\\VTABLE.DAT";
    $ftable = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI\\FTABLE.DAT";

    $fbr = fopen($ftable, 'rb');
    fseek($fbr, 2 * $file_num, SEEK_SET);

    $data = fread($fbr, 2);
    
    if (strlen($data) < 2) {
        return false;
    }

    $pack = unpack("v", $data);

    $dat_id = $pack[1];
    $dat_dir  = (int)($dat_id / 0x80);
    $dat_path = (int)($dat_id % 0x80);
    
    rewind($fbr);
    fclose($fbr);

    return [ $dat_id, $dat_dir, $dat_path ];
}

$output = [];
$max_file_scan = 500000;


// build a header table
$valid = [];
$json = file_get_contents(__DIR__."/out/dats_ftable_retail.json");
$json = json_decode($json, true);

foreach($json as $row) {
    $slot = $row['dat_header'];

    // testing body pieces
    if ($slot != "0hf_") {
        continue;
    }

    $valid[] = $row['dat_id'];
}


// First we handle per race
foreach($races as $race_id => $race_name) {
    if ($race_name != "hume_female") continue;

    echo("Race: {$race_name}\n");

    // count for race
    $count_for_race = 0;

    // then we handle per slot
    foreach ($slots_short as $slot_name) {
        if ($slot_name != "body") continue;

        echo("- Slot: {$slot_name} \n");

        // count for slot
        $count_for_slot = 0;

        // now we scan the entire list
        foreach(range(0, $max_file_scan) as $i => $file_id) {
            $dat = get_dat_for_file_number($file_id);
        
            // if dat returns false, there are no more in the vtable.
            if ($dat === false) {
                break;
            }
        
            // The respected file ID, Dat Directory and Dat Name
            $dat_id = $dat[0];
            $dat_dir = $dat[1];
            $dat_name = $dat[2];

            // skip non body
            if ($dat_dir != 400 && !in_array($dat_id, $valid)) {
                continue;
            }
        
            // All gear is in ROM, it's never in Rom 2-9 because if you don't own the expansions you won't
            // have these folders, however you always need the gear because other people + npcs can wear expansion items.
            $dat_path = "ROM\\{$dat_dir}\\{$dat_name}.DAT";
        
            // record
            $output[$race_name][$slot_name][$file_id] = [ $dat_id, $dat_dir, $dat_name, $dat_path ];

            // counts
            $count_for_race++;
            $count_for_slot++;

            echo "- {$dat_id}, {$dat_dir}, {$dat_name}, {$dat_path}\n";
        }

        // report how many found
        echo("  Found: {$count_for_slot} {$slot_name} items\n");
    }

    // report how many found
    echo("  Finished: {$count_for_race} {$race_name} items!\n\n");
}

// 
// Rebase arrays so the model_ids match up, this will also append the "model id" onto the string of the dat path
//
echo("- Rebasing model ids...\n");
foreach ($output as $race_name => $slot_list) {
    foreach($slot_list as $slot_name => $models) {
        $model_id = 0;
        $model_list = [];

        foreach ($models as $file_id => $dat_data) {
            // grab the dat stuff from the recorded entry
            [ $dat_id, $dat_dir, $dat_name, $dat_path ] = $dat_data;

            $model_list[] = [
                "model_id" => $model_id,
                "file_id" => $file_id,
                "dat_id" => $dat_id,
                "dat_dir" => $dat_dir,
                "dat_name" => $dat_name,
                "dat_path" => $dat_path
            ];

            // increment the model id
            $model_id++;
        }

        // replace the list with the updated model id increment
        $output[$race_name][$slot_name] = $model_list;
    }
}

echo("- Writing out json lists...\n");
foreach($output as $race_name => $slot_data) {
    save_custom("autogen_model_ids_{$race_name}.json", $output);
}

echo("Finished!\n\n");