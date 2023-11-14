<?php
require_once __DIR__ . "/common.php";
require_once __DIR__ . "/build_gear_tables.php";

echo("Starting JSON builder for FFXI Gear.\n\n");


class csv_item_row {
    public $item_id;
    public $name;
    public $level;
    public $item_level;
    public $jobs;
    public $mid;
    public $shieldSize;
    public $scriptType;
    public $slot;
    public $rslot;
    public $sulevel;
    public $model_id;
}

class windower_item_row {
    public $id;
    public $category;
    public $name;
    public $name_long;
}

function load_item_database($csv_file) {
    $output = [];
    $csv = load_list($csv_file);

    foreach ($csv as $i => $line) {
        if ($i == 0 || empty($line)) continue;

        $line = str_getcsv($line);

        $item = new csv_item_row();
        $item->item_id = filter_var($line[0], FILTER_SANITIZE_NUMBER_INT);
        $item->name = $line[1];
        $item->level = filter_var($line[2], FILTER_SANITIZE_NUMBER_INT);
        $item->item_level = filter_var($line[3], FILTER_SANITIZE_NUMBER_INT);
        $item->jobs = filter_var($line[4], FILTER_SANITIZE_NUMBER_INT);
        $item->mid = filter_var($line[5], FILTER_SANITIZE_NUMBER_INT);
        $item->shieldSize = filter_var($line[6], FILTER_SANITIZE_NUMBER_INT);
        $item->scriptType = filter_var($line[7], FILTER_SANITIZE_NUMBER_INT);
        $item->slot = filter_var($line[8], FILTER_SANITIZE_NUMBER_INT);
        $item->rslot = filter_var($line[9], FILTER_SANITIZE_NUMBER_INT);
        $item->sulevel = filter_var($line[10], FILTER_SANITIZE_NUMBER_INT);
        $item->model_id = filter_var($line[11], FILTER_SANITIZE_NUMBER_INT);
        $output[$item->item_id] = $item;
    }

    return $output;
}

function load_item_windower_data() {
    $output = [];
    $json = load_json("/in/in_windower_items.json");

    foreach ($json as $line) {
        $item = new windower_item_row();
        $item->id = filter_var($line['id'], FILTER_SANITIZE_NUMBER_INT);
        $item->category = $line['category'];
        $item->name = $line['name'];
        $item->name_long = $line['name_long'];
        $output[$item->id] = $item;
    }

    return $output;
}


function ffxi_get_race_table_slot($race_tbl, $equip_slot)
{
    $slots = [
        $race_tbl['face'],
        $race_tbl['head'],
        $race_tbl['body'],
        $race_tbl['hands'],
        $race_tbl['legs'],
        $race_tbl['feet'],
        $race_tbl['main'],
        $race_tbl['sub'],
        $race_tbl['ranged']
    ];

    return $slots[$equip_slot];
}

function ffxi_get_file_id($race_tbl, $equip_num)
{
    // Fix for 2 random fishing rods...
    $equip_slot = $equip_num >> 12;
    $equip_slot = $equip_slot == 16 ? 8 : $equip_slot;

    // get slot table
    $slot_tbl = ffxi_get_race_table_slot($race_tbl, $equip_slot);

    // temps
    $equip_tmp1 = $equip_num;
    $equip_tmp2 = $equip_num;

    // Ensure the slot has a base DAT id to work with..
    if ($slot_tbl['id1']) {
        // Ensure the raw model id does not exceed the total possible entries the slot has information for..
        if (($equip_tmp1 & 0xFFF) >= $slot_tbl['count1'] + $slot_tbl['count2'] + $slot_tbl['count3'] + $slot_tbl['count4'] + $slot_tbl['count5'] + $slot_tbl['count6']) {
            // Reset the model id back to the slot's 'naked' model id value on error..
            $equip_tmp1 = $equip_slot << 12;
            $equip_tmp2 = $equip_slot << 12;
        }

        $count1 = $slot_tbl['count1'];
        $count2 = $slot_tbl['count2'];
        $count3 = $slot_tbl['count3'];
        $count4 = $slot_tbl['count4'];
        $count5 = $slot_tbl['count5'];
        $count6 = $slot_tbl['count6'];
        $mid = 0;
        $did = 0;
        $dat_id = 0;

        // Ensure a valid race table was requested..
        // Determine which base id offset is proper to use for the given generalized model id..
        $mid = $equip_tmp1 & 0xFFF;
        if ($mid >= $count1) {
            if ($mid >= $count1 + $count2) {
                if ($mid >= $count1 + $count2 + $count3) {
                    if ($mid >= $count1 + $count2 + $count3 + $count4) {
                        if ($mid >= $count1 + $count2 + $count3 + $count4 + $count5) {
                            // If the model id is larger than all available slot ids, then it is invalid and ignored..
                            if ($mid >= $count1 + $count2 + $count3 + $count4 + $count5 + $count6) {
                                return -1;
                            }

                            $did = $slot_tbl['id6'] - $count1 - $count2 - $count3 - $count4 - $count5;
                        } else {
                            $did = $slot_tbl['id5'] - $count1 - $count2 - $count3 - $count4;
                        }
                    } else {
                        $did = $slot_tbl['id4'] - $count1 - $count2 - $count3;
                    }
                } else {
                    $did = $slot_tbl['id3'] - $count1 - $count2;
                }

                $equip_tmp1 = $equip_tmp2;
            } else {
                $did = $slot_tbl['id2'] - $count1;
            }
        } else {
            $did = $slot_tbl['id1'];
        }

        // Calculate the resulting actual DAT file id..
        $dat_id = $mid + $did;
        return $dat_id;
    } else {
        // Do nothing, the client will not load anything in this case..
    }

    return -1;
}

// preload windoer and ftable data.
$windower_data = load_item_windower_data();
$ftable = load_ftable();
$output = [];

// Loop through each race
foreach($slots as $slot) {
    // preload item slot data (could probably be bundled, but oh well!)
    $item_data = load_item_database("/in/in_{$slot}.csv");

    // loop through each race
    foreach ($races as $race_index => $race_name) {
        echo("- Generating Race Item Data: {$race_index} - {$race_name}\n");

        // Grab the race index
        $race_table = $race_tables[$race_index];

        // loop through all [csv_item_row] in [item_data]
        foreach($item_data as $item) {
            // Grab windower data
            $windower = $windower_data[$item->item_id];

            // calculate equip slot
            $equip_slot = ($item->model_id) >> 12;

            // Calculate file id from model id
            $file_id = ffxi_get_file_id($race_table, $item->model_id);

            // Grab rom
            $rom = $ftable[$file_id];

            $arr = [
                "item_id" => $item->item_id,
                "name" => $windower->name,
                "name_short" => $item->name,
                "name_long" => $windower->name_long,
                "name_clean" => get_simple_name($windower->name),
                "category" => $windower->category,
                "level" => $item->level,
                "item_level" => $item->item_level,
                "item_super" => $item->sulevel,
                "jobs" => $item->jobs,
                "slot" => $slot,
                "int_slot" => $item->slot,
                "int_rslot" => $item->rslot,
                "mid" => $item->mid,
                "model_id"=> $item->model_id,
                "file_id" => $rom['file_id'],
                "dat" => $rom['dat'],
                "dat_header" => $rom['dat_header'],
                "rom_index" => $rom['rom_index'],
                "race_index" => $race_index,
                "race_name" => $race_name,
            ];

            ksort($arr);

            // build array
            $output[$race_name][] = $arr;
        }
    }
}

echo("\nFinished! Saving....\n");
foreach ($output as $race_name => $gear_data) {
    // Save
    echo("- Saving: gear_{$race_name}.json\n");
    save_data("gear_{$race_name}.json", $gear_data);
}