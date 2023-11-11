<?php

//
// Restore backups
//
$files = [
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\ElvaanF\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\ElvaanM\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\Galka\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\HumeF\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\HumeM\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\Mithra\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\Tarutaru\\Action.csv",
];

foreach ($files as $filename) {
    copy($filename . "_bkup", $filename);
}

//
// Fix stuff
//
$files = [
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\ElvaanF\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\ElvaanM\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\Galka\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\HumeF\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\HumeM\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\Mithra\\Action.csv",
    "E:\\FF11 Tools\\3D - Altana Viewer\\List\\PC\\Tarutaru\\Action.csv",
];

foreach ($files as $filename) {
    $list = file_get_contents($filename);
    $list = explode("\n", $list);

    $results = [];
    $category = null;

    foreach ($list as $line) {
        // if it's a category, skip
        if ($line[0] == "@") {
            $category = str_ireplace("@", "", $line);
            $results[$category] = [];
            continue;
        }

        // parse dat string
        $linedata = explode(",", $line);

        $datstring = $linedata[0];
        $folder = "";
        
        // if the first segment has 2 slashes, then the first number is a folder path
        if (substr_count(explode(";", $datstring)[0], "/") == 2) {
            print_r("yes");
            $folder = $datstring[0] . "/";
            $datstring = substr($datstring, 2);
        }

        print_r($line . PHP_EOL);

        // parse dat string
        $dat_results = get_folder_list_from_string($datstring);

        // Append a custom name
        foreach($dat_results as $i => $datline) {
            $name = $linedata[1] . "_" . $folder . $datline;
            $name = trim($name);
            $name = str_ireplace("/", "-", $name);

            $dat_results[$i] = "{$folder}{$datline},{$name}";
        }

        // merge into list
        $results[$category] = array_merge($results[$category], $dat_results);
    }

    // take a backup of the current action file
    copy($filename, $filename . "_bkup");

    $text = [];
    foreach ($results as $category => $dats) {
        $text[] = "@{$category}";

        foreach ($dats as $dat) {
            $text[] = $dat;
        }
    }

    // save
    $text = implode("\n", $text);

    file_put_contents($filename, $text);
    print_r("Updated {$filename}\n");
}

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

                for ($i = $start; $i <= $end; $i++) {
                    $result[] = $default_folder . '/' . $i;
                }
            } else {
                $result[] = $default_folder . '/' . $segment;
            }
        }
    }

    return $result;
}