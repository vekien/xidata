<?php
require_once __DIR__ . "/common.php";

// Build everything again
require_once __DIR__ . "/build.php";
sleep(1);

/**
 * This builds the txt files for the xidata C# App since I cba dealing with json structs I will just make simple lists.
 */

$files = [
    [ "animations", "anims_", ],
    [ "faces", "faces", ],
    [ "fx", "fx_", ],
    [ "gear", "gear_", ],
    [ "npc", "npc_", ],
    [ "zones", "zones_", ],
];

foreach($files as $filedata) {
    $directory = __DIR__ . "/out";

    [$type, $prefix] = $filedata;

    // Scan the directory
    $files = scandir($directory);

    // Filter files that start with the specified prefix
    $files_filtered = array_filter($files, function($file) use ($prefix) {
        return strpos($file, $prefix) === 0;
    });

    // Output the result
    $lines = [];
    foreach($files_filtered as $filename) {
        // Skip the missing id lists
        if (strpos($filename, "missing")) {
            continue;
        }

        // grab data
        echo("- flatten: {$filename}\n");
        $data = load_json("\\out\\{$filename}");

        // grab each line from it
        foreach($data as $row) {
            if (!$row) {
                continue;
            }

            $lines[] = "{$filename}|" . implode("|", $row);
        }
    }

    // save
    echo("Saving: xidata_{$type}.txt ...\n");
    file_put_contents(__DIR__ ."\\out\\xidata_{$type}.txt", implode(PHP_EOL, $lines));
    echo("\n");
}