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