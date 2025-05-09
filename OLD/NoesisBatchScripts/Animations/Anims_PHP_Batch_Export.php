<?php

$races = [
    "Hume_Male",
    "Hume_Female",
    "Elvaan_Male",
    "Elvaan_Female",
    "Tarutaru",
    "Mithra",
    "Galka"
];

// Filename format
$animFilename = "Animations_%s.csv";
$templateFilename = "Animations_%s.template";

// placeholder to replace
$datsetReplace = '[[ALL_OTHER_ANIMATIONS]]';

// loop through each race
foreach ($races as $race) {
    echo("---[ {$race} ]---\n");

    $anim = sprintf($animFilename, $race);
    $template = sprintf($templateFilename, $race);

    // get the dataset template
    $template = file_get_contents(__DIR__ .'/'. $template);

    // open the animation file
    $dats = file_get_contents(__DIR__ .'/'. $anim);
    $dats = explode(PHP_EOL, $dats);

    // tmp
    $type   = null;
    $datlines = [];

    $animationsPerType = [];

    foreach ($dats as $animations) {
        $animations = trim($animations);
    
        if (empty($animations)) {
            continue;
        }

        if ($animations[0] == '@') {
            $type = substr($animations, 1);
            $type = str_ireplace(' ', '_', $type);
            $type = str_ireplace('.', '_', $type);
            $type = ucwords($type);
            continue;
        }

        if (!isset($animationsPerType[$type])) {
            $animationsPerType[$type] = [];
        } 

        $animationsPerType[$type][] = $animations;
    }

    // build datset files

    foreach($animationsPerType as $type => $datlines) {
        $dats = [];

        // split all lines into individual dats
        foreach ($datlines as $line) {
            // we don't care about the name
            $line = explode(",", $line)[0];
            $line = splitDatRange($line);

            $dats = array_merge($dats, $line);
        }

        $animationsPerType[$type] = $dats;
    }

    // build the datset files

    foreach ($animationsPerType as $type => $dats) {
        // write out the paths
        $paths = [];
        foreach ($dats as $datfile) {
            // add the datset entry and replace 1/ for the first ROM
            $paths[] = 'dat "__animation" "ROM'. str_ireplace('1/', '/', $datfile) .'.DAT"';
        }

        // replace placeholder
        $filedata = str_ireplace($datsetReplace, implode("\n", $paths), $template);

        // create folder
        $folder = __DIR__ ."/{$race}/";
        if (!is_dir($folder)) {
            mkdir($folder, 0777, true);
        }

        $filename = "Anims_{$race}_{$type}_Noesis.ff11datset";

        // write file
        echo ("Writing: {$race} anim: {$type} --> {$filename}\n");

        file_put_contents($folder . $filename, $filedata);
    }
}

/**
 * Correctly split up the AtlanaViewer list structures...
 */
function splitDatRange($line) {
    $segments = explode(";", $line);

    /**
     *  There are 4 variations
     *  -  R/x/y-z
     *  -  x/y-z
     *  -  x/y
     *  -  y-z
     * 
     *  Cases of R/x/y-z is when there is a Rom included
     * 
     *  Cases of X/Y-Z are simple, just go from Y > Z with X infront.
     * 
     *  Cases of X/Y are even more simple, just X/Y, return.
     * 
     *  Cases of Y-Z need to use X from $segment [0]
    **/

    $dats = [];

    foreach ($segments as $seg) {
        //
        // X/Y-Z
        //
        if (str_contains($seg, '/') && str_contains($seg, '-')) {
            $parts = explode("/", $seg);

            if (count($parts) == 3) {
                $rom   = $parts[0];
                $first = $parts[1];
                $range = explode('-', $parts[2]);

                foreach (range($range[0], $range[1]) as $i) {
                    $dats[] = "{$rom}/{$first}/{$i}";
                }

            } else {
                $first = $parts[0];
                $range = explode('-', $parts[1]);

                foreach (range($range[0], $range[1]) as $i) {
                    $dats[] = "1/{$first}/{$i}";
                }
            }

            
        }

        //
        // X/Y
        //
        if (str_contains($seg, '/') && !str_contains($seg, '-')) {
            $dats[] = "1/{$seg}";
        }

        //
        // Y-Z
        //
        if (!str_contains($seg, '/') && str_contains($seg, '-')) {
            $first = explode('/', $segments[0])[0];
            $range = explode('-', $seg);

            foreach (range($range[0], $range[1]) as $i) {
                $dats[] = "1/{$first}/{$i}";
            }
        }
    }
    
    return $dats;
}