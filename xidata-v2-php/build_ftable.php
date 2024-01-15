<?php
require_once __DIR__ . "/common.php";

// Returns specific headers
function get_dat_file_header($dat_file) {
    $handle = fopen($dat_file, "r");
    $bytes = fread($handle, 0 + 4);
    fclose($handle);
    $bytes = preg_replace('/[^\w]+/', '', $bytes);
    return $bytes;
}

echo("Starting brute force ftable scanning...\n\n");

$scan_headers = true;
$ffxi_install_name = "retail";
$ffxi_install = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI";
//$ffxi_install = "D:\\SquareEnix\\HXI\\HorizonXI\\Game\\SquareEnix\\FINAL FANTASY XI";
//$ffxi_install = "D:\\catseyexi\\catseyexi-client\\Game\\FINAL FANTASY XI";
$ffxi_rom_folders = [
    1 => "",
    2 => "ROM2\\",
    3 => "ROM3\\",
    4 => "ROM4\\",
    5 => "ROM5\\",
    6 => "ROM6\\",
    7 => "ROM7\\",
    8 => "ROM8\\",
    9 => "ROM9\\",
    10 => "ROM10\\",
    11 => "ROM11\\",
    12 => "ROM12\\",
    13 => "ROM13\\",
    14 => "ROM14\\",
];

$dats_vtable = [];
$dats_ftable = [];

// loop through each rom
foreach($ffxi_rom_folders as $rom_index => $rom_folder) {
    // Build the paths for ftable and vtable
    $ftable_filename = $rom_index == 1 ? "FTABLE.DAT" : "FTABLE{$rom_index}.DAT";
    $vtable_filename = $rom_index == 1 ? "VTABLE.DAT" : "VTABLE{$rom_index}.DAT";

    $ftable_filename = "{$ffxi_install}\\{$rom_folder}{$ftable_filename}";
    $vtable_filename = "{$ffxi_install}\\{$rom_folder}{$vtable_filename}";

    // Skip if this rom doesn't exist
    if (!file_exists($ftable_filename)) {
        continue;
    }

    echo("- Scanning: ROM {$rom_index} {$ftable_filename} ...\n");

    // // pad if needed
    // if (count($dats_vtable) < filesize($vtable_filename)) {
    //     $dats_vtable = array_pad($dats_vtable, filesize($ftable_filename), 0);
    // }

    
    $write = false;
    $fid = 73163;

    if ($write)
    {
        $vtable_fs = fopen($vtable_filename, 'r+b');
        $ftable_fs = fopen($ftable_filename, 'r+b');

        $dat_rom = 1;
        $dat_dir = 500; // looks like it can be anything...
        $dat_path = 88; // 127 is max (8 length 128 total, 0-127)

        $dat_id = $dat_dir * 0x80 + $dat_path;
        $dat_id = (int)$dat_id;

        $test_dat_dir = (int)($dat_id / 0x80);
        $test_dat_path = (int)($dat_id % 0x80);

        echo("Checking...");
        var_dump(
            $dat_dir, $dat_path, "dat_id", $dat_id, "test", (int)$test_dat_dir, (int)$test_dat_path
        );

        $packed_data = pack('v', $dat_id);
        
        fseek($vtable_fs, $fid);
        $result_v = fwrite($vtable_fs, chr($dat_rom));
        
        if ($result_v === false) {
            die("Error writing to vtable_fs");
        }
        
        fseek($ftable_fs, $fid * 2);
        $result_f = fwrite($ftable_fs, $packed_data);
        
        if ($result_f === false) {
            die("Error writing to ftable_fs");
        }
        
        fclose($vtable_fs);
        fclose($ftable_fs);

        echo("\n\nF/V Tables Injected\n\n");
    }

    // Open VTable and FTable files for this ROM Folder
    $vtable_fs = fopen($vtable_filename, 'rb');
    $ftable_fs = fopen($ftable_filename, 'rb');
    $vtable_position = 0;
    $ftable_position = 0;

    // Loop till the endof the file.
    $filesize = filesize($vtable_filename);

    for ($i = 0; $i < $filesize; $i++) {
        $file_id = $i;

        // read the next single byte from vtable
        $dat_rom = ord(fread($vtable_fs, 1));
        $vtable_position = ftell($vtable_fs) - 1;

        // read the next sinfle byte from ftable
        $dat_id = unpack('v', fread($ftable_fs, 2))[1];
        $ftable_position = ftell($ftable_fs) - 2;

        $dat_dir = (int)($dat_id / 0x80);
        $dat_path = (int)($dat_id % 0x80);

        $dat_rom_dir = $rom_index == 1 ? "ROM" : "ROM{$rom_index}";
        $dat_filename = join(DIRECTORY_SEPARATOR, [$dat_rom_dir, $dat_dir, $dat_path . ".DAT"]);

        // Used to verify injection
        /*
        if ($file_id >= $fid -3 && $file_id <= $fid +3) {
            $hit = $file_id == $fid ? "<<<<<<" : "";
            echo("[vpos = {$vtable_position} / fpos = {$ftable_position}] file_id = {$file_id} | dat = {$dat_filename}  {$hit} \n");
        }*/

        if ($dat_rom > 0) {
            $dats_vtable[$rom_index][$i] = $dat_rom;

            $dat_dir = (int)($dat_id / 0x80);
            $dat_path = (int)($dat_id % 0x80);

            $dat_rom_dir = $rom_index == 1 ? "ROM" : "ROM{$rom_index}";
            $dat_filename = join(DIRECTORY_SEPARATOR, [$dat_rom_dir, $dat_dir, $dat_path . ".DAT"]);

            $dats_ftable[$rom_index][$i] = [
                'file_id' => $file_id,

                'rom_index' => $rom_index,
                'rom_folder' => $dat_rom_dir,

                'dat' => $dat_filename,
                'dat_id' => $dat_id,
                'dat_dir' => $dat_dir,
                'dat_path' => $dat_path,
                'dat_header' => null,
            ];
        }
    }

    fclose($vtable_fs);
    fclose($ftable_fs);
}

// header scan
if ($scan_headers) {
    foreach($dats_ftable as $rom_index => $dats) {
        $done = [];
        $total = count($dats);
        echo("- [ROM {$rom_index}] Appending Headers: {$total} rows.\n");

        foreach ($dats as $i => $row) {
            $path = "{$ffxi_install}\\{$row['dat']}";

            if (!file_exists($path)) {
                $dats_ftable[$rom_index][$i]['dat_header'] = "(file missing)";
                continue;
            }

            $cache_key = md5($path);

            $header = isset($done[$cache_key]) ? $done[$cache_key] : get_dat_file_header($path);
            $done[$cache_key] = $header;

            // add header
            $dats_ftable[$rom_index][$i]['dat_header'] = $header;
        }
        
    }
}

// ksort the files
ksort($dats_ftable);
ksort($dats_vtable);

// Save
echo("- Saving: ftable and vtable jsons\n");

save_data("dats_ftable_{$ffxi_install_name}.json", $dats_ftable);
save_data("dats_vtable_{$ffxi_install_name}.json", $dats_vtable);