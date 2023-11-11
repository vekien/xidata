<?php

/**
 * This will decode a "look" string either from a Player or Character NPC (not a monster) and decode
 * it into it's correct model ids which can be used as gear ref lookups to rebuild the NPC.
 */

$lookTable = [];

/**
 * This is a translation from:
 * - process_npc_data
 * - unpack_gear_id
 * 
 * From the "NPCDecoder" Windower Addon.
 */
function getModelIdFromLookData($lookdata) {
    $a = hexdec(substr($lookdata, 0, 2));
    $b = hexdec(substr($lookdata, 2, 2));

    $shift = $b << 8;
    $combined = $a + $shift;
    $masked = $combined & 0x0FFF;

    return $masked;
}

// the look string to decode
$look = "0x01001d100C7004D009F003E00C301AA00330000";

/**
 *              | -------- this is all gear ----------|
 * 0x01000C01   E910 0421 C930 B940 0251 0060 0070 0000
 *              
 * 
 * 0x01 00 0C 01
 * 
 * 0x01 = skip (if you pull from MySQL you won't have 0x)
 * 00 = look/style
 * 0C = face
 * 01 = race
 */

$races = [
    [
        // None - I think these are objects like gates
    ],
    [
        'name'  => 'Hume Male',
        'dat'   => 'ROM/27/82.DAT',
        'look'  => 'HumeMale',
    ],
    [
        'name'  => 'Hume Female',
        'dat'   => 'ROM/32/58.DAT',
        'look'  => 'HumeFemale',
    ],
    [
        'name'  => 'Elvaan Male',
        'dat'   => 'ROM/37/31.DAT',
        'look'  => 'ElvaanMale',
    ],
    [
        'name'  => 'Elvaan Female',
        'dat'   => 'ROM/42/4.DAT',
        'look'  => 'ElvaanFemale',
    ],
    [
        'name'  => 'Tarutaru Male',
        'dat'   => 'ROM/46/93.DAT',
        'look'  => 'Tarutaru',
    ],
    [
        'name'  => 'Tarutaru Female',
        'dat'   => 'ROM/46/93.DAT',
        'look'  => 'Tarutaru',
    ],
    [
        'name'  => 'Mithra',
        'dat'   => 'ROM/51/89.DAT',
        'look'  => 'Mithra',
    ],
    [
        'name'  => 'Galkae',
        'dat'   => 'ROM/56/59.DAT',
        'look'  => 'Galka',
    ],
    
    // There is nothign for a bit here

    // 29 = Mithra Child
    // 30 = Hume/Elvaan Child F
    // 31 = Hume/Elvaan child M

    // 32,33,34,35,36 = Chocobo
];

/**
 * Huge thanks to Shozokui for providing these
 */
$faces = [
    0 => "F1A",
    1 => "F1B",
    2 => "F2A",
    3 => "F2B",
    4 => "F3A",
    5 => "F3B",
    6 => "F4A",
    7 => "F4B",
    8 => "F5A",
    9 => "F5B",
    10 => "F6A",
    11 => "F6B",
    12 => "F7A",
    13 => "F7B",
    14 => "F8A",
    15 => "F8B",
    26 => "NPC1",
    27 => "NPC2",
    28 => "NPC3",
    29 => "Fomor",
    30 => "Mannequin",
];


/**
 * This struct is based on the character length for
 * each bit of data, with gear being mostly 4 bytes.
 * 
 * As it loops through the struct, it will increment
 * a pointer, starting from 0
 */
$struct = [
    'init'      => 4, // init 0x01 (this will check for 0x)
    'stylelock' => 2, // stylelock
    'face'      => 2, // face
    'race'      => 2, // race

    // gear struct
    'head'      => 4, // head
    'body'      => 4, // body
    'hands'     => 4, // hands
    'legs'      => 4, // legs
    'feet'      => 4, // feet
    'main'      => 4, // main
    'sub'       => 4, // sub
    'ranged'    => 4, // ranged
];

/**
 * Basically matches my "struct" to the json 
 * filename in "gear_dat_json", i could have named them
 * the same but this is more convoluted :D
 */
$gearslots = [
    'head'   => 'Heads',
    'body'   => 'Body',
    'hands'  => 'Hands',
    'legs'   => 'Legs',
    'feet'   => 'Feet',
    'main'   => 'Main',
    'sub'    => 'Sub',
    'ranged' => 'Ranged'
];

$data = [];
$pointer = 0;
foreach ($struct as $type => $length) {
    if ($type == 'init') {
        // if there is 0x, increment by normal length otherwise by 2
        $pointer += substr($look, 0, 2) == '0x' ? $length : 2;
        continue;
    }

    if ($type == 'stylelock') {
        // we don't care about this for NPCs
        $pointer += $length;
        continue;
    }

    // grab the data
    $lookdata = substr($look, $pointer, $length);

    // debugging
    // print_r([$type, $look, $lookdata, hexdec($lookdata)]);

    // if this is face or race, just hexdec it and continue
    if (in_array($type, ['face', 'race'])) {
        $data[$type] = hexdec($lookdata);
        $pointer += $length;
        continue;
    }

    // onto gear
    $pointer += $length;

    // grab the model_id
    $data[$type] = getModelIdFromLookData($lookdata);
}

print_r($data);