<?php

function console($text) {
    echo("{$text}\n");
}

console("Building Common NPC Database");

$npcs = file_get_contents(__DIR__ .'/npc_2.json');
$npcs = json_decode($npcs, true);


function build_dat_to_name_list() {
    $lookup_list = [];
    $gear = file_get_contents(__DIR__ .'/gear_1.json');
    $gear = json_decode($gear, true);

    foreach ($gear as $race => $geardata) {
        foreach($geardata['Gear'] as $slot => $list) {
            foreach($list as $item) {
                [$name, $dat] = explode("|", $item);

                $lookup_list[$dat] = "{$race}_{$slot}_{$name}";
                
            }
        }
    }

    return $lookup_list;
}

$datlist = build_dat_to_name_list();

$csv = [];
$csv[] = "---,LBS_ID,Name,PosRot,PosX,PosY,PosZ,Content,Zone,Race,Face,Head,Body,Hands,Legs,Feet,Main,Sub,Range,IsVisible";

foreach ($npcs as $npc) {
    //
    // Filter some stuff that I'm just not going to be including in the VR experience...
    //

    // if it doesn't have a position x/y, skip
    $x = abs((float)$npc['PosX']);
    $y = abs((float)$npc['PosY']);

    if (!$x && !$y) {
        continue;
    }

    // if name is ??? skip, not sure what to do with these
    if ($npc['Name'] == '???') {
        continue;
    }

    // at the moment i don't have custom faces... Need to sort this
    if ($npc['Face'] == '') {
        continue;
    }

    //
    // Build Unreal Engine table
    //
    $race = explode("|", $npc["Race"])[0];

    // get gear names that match ue4
    $face = explode("|", $npc["Face"])[2] ?? null;
    $face = $face ? ($datlist[$face]) : "";

    $head = explode("|", $npc["Head"])[2] ?? null;
    $head = $head ? ($datlist[$head] ?? "") : "";

    $body = explode("|", $npc["Body"])[2] ?? null;
    $body = $body ? ($datlist[$body] ?? "") : "";

    $hands = explode("|", $npc["Hands"])[2] ?? null;
    $hands = $hands ? ($datlist[$hands] ?? "") : "";

    $legs = explode("|", $npc["Legs"])[2] ?? null;
    $legs = $legs ? ($datlist[$legs] ?? "") : "";

    $feet = explode("|", $npc["Feet"])[2] ?? null;
    $feet = $feet ? ($datlist[$feet] ?? "") : "";

    $main = explode("|", $npc["Main"])[2] ?? null;
    $main = $main ? ($datlist[$main] ?? "") : "";

    $sub = explode("|", $npc["Sub"])[2] ?? null;
    $sub = $sub ? ($datlist[$sub] ?? "") : "";
    
    $range = explode("|", $npc["Range"])[2] ?? null;
    $range = $range ? ($datlist[$range] ?? "") : "";

    $csv[] = sprintf(
        '%s,%s,"%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","%s","True"',
        $npc['UEID'], // id
        $npc['ID'],
        $npc['Name'],
        $npc['PosRot'],
        $npc['PosX'],
        $npc['PosY'],
        $npc['PosZ'],
        (empty(trim($npc['Content'])) ? 'BASE_GAME' : trim($npc['Content'])),
        $npc['Zone'],
        $race,
        str_ireplace("_Face_", "_", $face),
        empty($head) ? "{$race}_1FA" : $head,
        empty($body) ? "{$race}_Body_Default" : $body,
        empty($hands) ? "{$race}_Hands_Default" : $hands,
        empty($legs) ? "{$race}_Legs_Default" : $legs,
        empty($feet) ? "{$race}_Feet_Default" : $feet,
        $main,
        $sub,
        $range
    );
}

$csv = implode("\n", $csv);
file_put_contents(__DIR__ . '/FF11_DB_NPC.csv', $csv);