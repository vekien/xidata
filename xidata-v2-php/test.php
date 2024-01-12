<?php

$json = file_get_contents(__DIR__."/out/dats_ftable.json");
$json = json_decode($json, true);

$output1 = [];
$output2 = [];

foreach($json as $row) {
    $slot = $row['dat_header'];

    // testing body pieces
    if ($slot != "0hf_") {
        continue;
    }

    $output1[$row['file_id']] = $row;
}

ksort($output1);

$output1 = array_values($output1);

foreach($output1 as $mid => $row) {
    //$mid = $mid - 0x2000;

    $output2[$mid] = "(mid = {$mid}) | file_id = {$row['file_id']} | {$row['dat']}";

}

ksort($output2);
file_put_contents(__DIR__ ."/hume_female_body_gear_test.txt", implode("\n", $output2));