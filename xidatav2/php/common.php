<?php
// Most scripts use a lot of memory...
ini_set("memory_limit", "-1");

$races = [
    "hume_male",
    "hume_female",
    "elvaan_male",
    "elvaan_female",
    "taru_male",
    "taru_female",
    "mithra",
    "galka"
];

$slots = [
    "head",
    "body",
    "hands",
    "legs",
    "feet",
    "main",
    "sub",
    "ranged"
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
    return load_json("\\out\\dats_ftable.json");
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