<?php

$roms = file_get_contents(__DIR__ .'/NPC_AHK.txt');
$roms = explode("\n", $roms);

$missing = [];

foreach ($roms as $rom) {
    [$name, $path, $dat] = explode("|", $rom);

    echo ("Checking: {$name} \n");

    if (!file_exists($path)) {
        echo ("-- Missing: {$name}");
        $missing[] = "{$name}|{$path}|{$dat}";
    }
}

file_put_contents(__DIR__.'/NPC_AHK_MISSING.txt', implode("\n", $missing));