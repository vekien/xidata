<?php
require_once __DIR__ . "/common.php";

/**
 * This is built for my ARPG project, but it basically generates a common json
 * that I import as a DataTable. It's rather simple!
 * 
 * This doesn't do weapons because Main/Sub can swap/share dats and it becomes a pain in the ass
 * Plus i don't have weapon slots mapped atm, so fuck it. CBA
 */

echo("Building Unreal Engine DB NPC Commonn");

// load the gearsets
$gearsets = load_json("\\custom\\gearsets.json");

// unreal db
$unrealdb = [];
foreach($gearsets as $set) {
    $race = uc_string($set['race']);

    $face = $set['face'] ? finalize_name($set['face']['name']) : null;
    $head = $set['head'] ? finalize_name($set['head']['name']) : null;
    $body = $set['body'] ? finalize_name($set['body']['name']) : null;
    $hands = $set['hands'] ? finalize_name($set['hands']['name']) : null;
    $legs = $set['legs'] ? finalize_name($set['legs']['name']) : null;
    $feet = $set['feet'] ? finalize_name($set['feet']['name']) : null;

    $arr = [
        "Name" => $set['name'],
		"Race" => $race,
        "Face" => "None",
		"Head" => "None",
		"Body" => "None",
		"Hands" => "None",
		"Legs" => "None",
		"Feet" => "None",
    ];

    if ($face) {
        $arr["Face"] = "/Script/Engine.SkeletalMesh'/Game/Characters/XI_{$race}/Face/{$face}/{$face}.{$face}'";
    }

    if ($head) {
        $arr["Head"] = "/Script/Engine.SkeletalMesh'/Game/Characters/XI_{$race}/Head/{$head}/{$head}.{$head}'";
    }

    if ($body) {
        $arr["Body"] = "/Script/Engine.SkeletalMesh'/Game/Characters/XI_{$race}/Body/{$body}/{$body}.{$body}'";
    }

    if ($hands) {
        $arr["Hands"] = "/Script/Engine.SkeletalMesh'/Game/Characters/XI_{$race}/Hands/{$hands}/{$hands}.{$hands}'";
    }

    if ($legs) {
        $arr["Legs"] = "/Script/Engine.SkeletalMesh'/Game/Characters/XI_{$race}/Legs/{$legs}/{$legs}.{$legs}'";
    }

    if ($feet) {
        $arr["Feet"] = "/Script/Engine.SkeletalMesh'/Game/Characters/XI_{$race}/Feet/{$feet}/{$feet}.{$feet}'";
    }

    print_r($arr);

    $unrealdb[] = $arr;
}

save_custom("DB_NPC_Common_Gear.json", $unrealdb);



//
// Filename that matches the imported data
//
function finalize_name($name) {
    if (empty($name)) {
        return;
    }
    // Remove characters that are not alphanumeric, hyphen, or underscore
    $cleanedName = preg_replace("/[^a-zA-Z0-9-_ ]+/", "", trim($name));

    // Replace spaces with underscores
    $underscoredName = str_replace(" ", "_", $cleanedName);

    // Remove consecutive underscores
    $finalName = str_replace("__", "_", $underscoredName);

    return $finalName;
}