<?php
require_once __DIR__ . "/common.php";

echo("Building AltanaViewer data jsons...\n");

/**
 * Builds table for Effects
 */

$ftable = load_ftable();
$ftable_reversed = load_ftable_as_roms();

$in_files_effects = [
    [ "Ability", "SkillChain", "ability_skillchain.csv", ],
    [ "Ability", "Ability", "ability.csv", ],
    [ "Ability", "Automation", "automation.csv", ],
    [ "Ability", "Automation WS", "automation_ws.csv", ],

    [ "Expansion", "Crystalline Prophecy", "expansion_acp.csv", ],
    [ "Expansion", "A Moogle Kupo d'Etat", "expansion_amke.csv", ],
    [ "Expansion", "A Shantotto Ascension", "expansion_asa.csv", ],
    [ "Expansion", "Final Fantasy XI", "expansion_basegame.csv", ],
    [ "Expansion", "Chains of Promathia", "expansion_cop.csv", ],
    [ "Expansion", "Rise of the Zilart", "expansion_roz.csv", ],
    [ "Expansion", "Seekers of Adoulin", "expansion_soa.csv", ],
    [ "Expansion", "Treasures of Aht Urhgan", "expansion_toau.csv", ],
    [ "Expansion", "Wings Of the Goddess", "expansion_wotg.csv", ],

    [ "Misc", "Items", "items.csv", ],
    [ "Misc", "Items", "others.csv", ],
    [ "Misc", "Items", "unknown.csv", ],

    [ "Job", "White magic", "job_whitemagic.csv", ],
    [ "Job", "Black Magic", "job_blackmagic.csv", ],
    [ "Job", "Blue Magic", "job_bluemagic.csv", ],
    [ "Job", "Corsair", "job_corsair.csv", ],
    [ "Job", "Geomancer", "job_geomancer.csv", ],
    [ "Job", "Ninjitsu", "job_ninjitsu.csv", ],
    [ "Job", "Songs", "job_songs.csv", ],
    [ "Job", "Summoning", "job_summoning.csv", ],
];

$missing = [];
$output = [];
foreach ($in_files_effects as $af_effect) {
    [ $type, $category, $filename ] = $af_effect;

    echo ("- Processing: {$type} - {$category} - {$filename}\n");

    // load
    $file = load_list("\\in\\in_fx_{$filename}");

    foreach ($file as $line) {
        if (empty($line)) continue;

        // grab dat paths and the name
        [$dat, $name] = explode(",", $line);
        $name = preg_replace('/[^\w\s\-]/', '', trim($name));

        // build dat
        $dat = $dat = substr_count($dat, "/") > 1 ? "ROM{$dat}.DAT" : "ROM/{$dat}.DAT";
        $dat = str_ireplace("/", "\\", $dat);

        // try get the dat-data
        $file_id = $ftable_reversed[$dat] ?? null;
        $dat_data = $file_id ? $ftable[$file_id] : [
            'dat' => $dat,
        ];

        if ($file_id === null) {
            $missing[] = $dat;
        } 

        $arr = [
            "name" => $name,
            "name_full" => $name,
            "name_clean" => get_simple_name($name),
            "category" => $category,
            "type" => $type,
            "dat" => $dat,
        ];

        $arr = array_merge($arr, $dat_data);

        ksort($arr);

        $output[$type][] = $arr;
    }
}

echo("\nFinished! Saving....\n");
foreach ($output as $type => $data) {
    echo("- Save: {$type} \n");
    save_data("fx_". strtolower($type) .".json", $data);
}
