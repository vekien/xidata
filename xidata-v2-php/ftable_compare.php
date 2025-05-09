<?php
require_once __DIR__ . "/common.php";

$dats_ftable_cexi = load_json("/out/dats_ftable_cexi.json");
$dats_ftable_hxi = load_json("/out/dats_ftable_hxi.json");
$dats_ftable_retail = load_json("/out/dats_ftable_retail.json");

$cexi_table = [];
foreach($dats_ftable_cexi as $rom => $dats) {
    foreach($dats as $file_id => $dat) {
        $cexi_table[$rom . $file_id] = $dat['dat'];
    }
}

$hxi_table = [];
foreach($dats_ftable_hxi as $rom => $dats) {
    foreach($dats as $file_id => $dat) {
        $hxi_table[$rom . $file_id] = $dat['dat'];
    }
}

$retail_table = [];
foreach($dats_ftable_retail as $rom => $dats) {
    foreach($dats as $file_id => $dat) {
        $retail_table[$rom . $file_id] = $dat['dat'];
    }
}

ksort($cexi_table);
ksort($hxi_table);
ksort($retail_table);

$cexi_total = count($cexi_table);
$hxi_total = count($hxi_table);
$retail_total = count($retail_table);

echo("cexi total = {$cexi_total} \n");
echo("hxi total = {$hxi_total} \n");
echo("retail total = {$retail_total} \n");

// Find what is missing from hxi compared to cexi
echo("\nListing dats in CEXI missing from HXI\n");
foreach($cexi_table as $index => $datpath) {
    if (!isset($hxi_table[$index])) {
        echo("CEXI >> HXI - Missing index: {$index} = {$datpath} \n");
    }
}

// Find what is missing from cexi compared to hxi
echo("\nListing dats in HXI missing from CEXI\n");
foreach($hxi_table as $index => $datpath) {
    if (!isset($cexi_table[$index])) {
        echo("HXI >> CEXI - Missing index: {$index} = {$datpath} \n");
    }
}

echo("\nComparing headers \n");
$count = 0;
foreach($dats_ftable_cexi as $rom => $dats) {
    foreach($dats as $file_id => $dat) {
        $cexi_dat_path = $dat['dat'];
        $cexi_dat_header = $dat['dat_header'];

        $hxi_dat_path = $dats_ftable_hxi[$rom][$file_id]['dat'] ?? null;
        $hxi_dat_header = $dats_ftable_hxi[$rom][$file_id]['dat_header'] ?? null;

        $retail_dat_path = $dats_ftable_retail[$rom][$file_id]['dat'] ?? null;
        $retail_dat_header = $dats_ftable_retail[$rom][$file_id]['dat_header'] ?? null;




        if ($hxi_dat_path && $cexi_dat_header != $hxi_dat_header) {
            $count++;

            $hxi_dat_path = str_pad($hxi_dat_path, 20);
            $cexi_dat_header = str_pad($cexi_dat_header, 5);
            $hxi_dat_header = str_pad($hxi_dat_header, 5);
            $retail_dat_header = str_pad($retail_dat_header, 5);

            echo("Header Diff - {$hxi_dat_path} [retail] {$retail_dat_header} [cexi] {$cexi_dat_header} [hxi] {$hxi_dat_header}\n");
        }
    }
}

echo(($count > 0 ? "{$count} files different..." : "No file headers were different.") . "\n");