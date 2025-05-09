<?php

// List of roms to extract
$romfile  = 'NPC_List.txt';

// folders to everything
$saveto   = 'X:\\project-vr\\NPC\\[[TYPE]]\\[[NAME]]\\[[NAME]].fbx';
$romdir   = 'E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI\\ROM[[ROM]]';

// find replacements
$replace  = [ "[[ROM]]", "[[TYPE]]", "[[NAME]]" ];

// grab all roms
$files = file_get_contents($romfile);
$files = explode(PHP_EOL, $files);

$type = null;

$output = [];

foreach ($files as $file) {
    $file = trim($file);

    if (empty($file)) {
        continue;
    }

    if ($file[0] == '@') {
        $type = substr($file, 1);
        $type = str_ireplace('.', '_', $type);
        $type = ucwords($type);
        echo ("New Type: {$type}\n");
        continue;
    }

    [$datfiles, $name] = explode(',', $file);

    $roms = [];
    $counter  = 1;

    foreach (explode(';', $datfiles) as $num => $df) {
        if (count(explode('-', $df)) > 1) {
            // split up
            $df = splitDatRange($df);
            $roms = array_merge($roms, $df);
        } else {
            $roms = array_merge($roms, [ $df ]);
        }   
    }

    foreach ($roms as $i => $rom) {
        $i = ($i + 1);

        // fix dat file
        $datfile = str_ireplace('/', '\\', $rom);
        $datfile = "{$datfile}.DAT";

        // format the zone name
        $name = str_ireplace(' ', '_', trim($name));
        $name = preg_replace("/[^a-z0-9_]+/i", "", $name);

        if ($name[0] == '_') {
            $name = substr($name, 1);
        }

        $newname = "{$name}_{$i}";

        // create the required folder for the zone
        $filename = str_ireplace($replace, [null, $type, $newname], $saveto);
        $directory = dirname($filename);

        if (!is_Dir($directory)) {
            mkdir($directory, 0777, true);
        }
        
        echo ("Created Directory: {$directory}\n");

        $rom = str_ireplace($replace, [$rom, null, null], $romdir);
        $rom = str_ireplace('ROM1', 'ROM', $rom);
        $rom = str_ireplace('/', '\\', $rom);
        $rom = "{$rom}.DAT";

        echo ("{$newname}|{$filename}|{$rom}\n");
        $output[] = "{$newname}|{$filename}|{$rom}";
    }    
}

file_put_contents(__DIR__ . '/NPC_AHK.txt', implode("\n", $output));

function splitDatRange($str) {
    $parts = explode("-", $str);

    [$a, $b, $c] = explode('/', $parts[0]);

    $start = $c;
    $end = $parts[1];

    $result = [];

    for ($i = $start; $i <= $end; $i++) {
        $result[] = "{$a}/{$b}/" . $i;
    }

    return $result;
}