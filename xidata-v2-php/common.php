<?php
// Most scripts use a lot of memory...
ini_set("memory_limit", "-1");

$races = [
    "hume_male",
    "hume_female",
    "elvaan_male",
    "elvaan_female",
    "tarutaru",
    "mithra",
    "galka"
];

$races_altanaviewer = [
    "HumeM",
    "HumeF",
    "ElvaanM",
    "ElvaanF",
    "Tarutaru",
    "Mithra",
    "Galka"
];

$races_to_dats = [
    "hume_male" => "27\\82",
    "hume_female" => "32\\58",
    "elvaan_male" => "37\\31",
    "elvaan_female" => "42\\4",
    "tarutaru" => "46\\93",
    "mithra" => "51\\89",
    "galka" => "56\\59",
    
];

$slots = [
    "head", "body", "hands", "legs", "feet", "main", "sub", "ranged"
];

$slots_short = [
    "head", "body", "hands", "legs", "feet"
];

$slots_altana = [
    "Head", "Body", "Hands", "Legs", "Feet", "Main", "Sub", "Range"
];

/**
 * Whenever there is an error of any type, immediately cancel.
 * This makes it way easier to debug huge batch operations.
 */
function exception_error_handler($errno, $errstr, $errfile, $errline ) {
    throw new ErrorException($errstr, 0, $errno, $errfile, $errline);
}
set_error_handler("exception_error_handler");

/**
 * load and return the ftable.
 */
function load_ftable() {
    return load_json("\\out\\dats_ftable_retail.json");
}

/**
 * Load ftable organised by DAT > File_ID (reverse lookup)
 */
function load_ftable_as_roms() {
    $ftable = load_ftable();

    foreach ($ftable as $row) {
        $data[$row['dat']] = $row['file_id'];
    }

    return $data;
}

/**
 * Save json
 */
function save_data($filename, $data) {
    file_put_contents(__DIR__ ."\\out\\{$filename}", json_encode($data, JSON_PRETTY_PRINT));
}

function save_custom($filename, $data) {
    file_put_contents(__DIR__ ."\\custom\\{$filename}", json_encode($data, JSON_PRETTY_PRINT));
}

/**
 * Load some json and decode into an array.
 */
function load_json($filename) {
    $json = file_get_contents(__DIR__ . $filename);
    $json = json_decode($json, true);
    return $json;
}

/**
 * Load a simple new line list file
 */
function load_list($filename) {
    $data = file_get_contents(__DIR__ . $filename);
    $data = explode("\n", $data);
    return $data;
}

function load_skeletons() {
    return load_json("\\in\\in_race_skeletons.json");
}

function logit($string) {
    file_put_contents(__DIR__ . "/log.txt", "{$string}\n", FILE_APPEND);
}

function sort_output($a, $b) {
    return $a['file_id'] < $b['file_id'] ? -1 : 1;
}


function get_simple_name($string) {
    $cleanedString = preg_replace('/[^\w\d_-]/', '', $string);
    $cleanedString = strtolower(trim($cleanedString));
    return $cleanedString;
}

function uc_string($string) {
    return str_replace(' ', '_', ucwords(str_replace('_', ' ', $string)));
}

/**
 * Parses a AltanaView folder string...
 */
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

//
// Filename that matches the imported data
//
function get_windows_name($name) {
    if (empty($name)) {
        return;
    }
    // Remove characters that are not alphanumeric, hyphen, or underscore
    $name = preg_replace("/[^a-zA-Z0-9-_ ]+/", "", trim($name));

    $name = str_replace(" ", "_", $name);
    $name = str_replace("-", "", $name);
    $name = str_replace("__", "_", $name);

    return $name;
}