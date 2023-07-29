<?php
/**
 * This builds out a full list of Model IDs to their respective DAT Paths. Model IDs 
 * are sent by the server to describe the "look" for a character or NPC. This is useful
 * if you need to parse the LandSandBoat NPC DB list and convert the "look" (using LookDecoder.php)
 * and then convert those slot ids (head, body, etc) to their respective Dat File for each race.
 * 
 * I want togive a huge thank you to Xenonsmurf and Atom0s for their invaluable knowledge 
 * and work on this area of the game. Wouldn't have been possible without them.
 * 
 * - Xenonsmurf: https://github.com/MurphyCodes
 * - Atom0s: https://atom0s.com/
 * 
 * Notes: This mainly works for Armor, not Weapons/Range because they have very different
 *        header variations and I need to write them all down, but I don't need that for Unreal Engine so...
 */

function getDatForFileNumber($fileNumber) {
    # $VTableFile = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI\\VTABLE.DAT";
    $FTableFile = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI\\FTABLE.DAT";

    $FBR = fopen($FTableFile, 'rb');
    fseek($FBR, 2 * $fileNumber, SEEK_SET);

    $data = fread($FBR, 2);
    
    if (strlen($data) < 2) {
        return false;
    }

    $pack = unpack("v", $data);

    $dat_id = $pack[1];
    $dat_dir  = (int)($dat_id / 0x80);
    $dat_path = (int)($dat_id % 0x80);
    
    rewind($FBR);
    fclose($FBR);

    return [ $dat_id, $dat_dir, $dat_path ];
}

function getFileHeaders($dat_path) {
    $dat_path = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI\\{$dat_path}";
    $handle = fopen($dat_path, "r");

    $slotBytesOffset = [0, 4];
    $weaponBytesOffset = [53, 8];
    

    // Read the headers
    $slotBytes = fread($handle, $slotBytesOffset[0] + $slotBytesOffset[1]);
    $weaponBytes = fread($handle, $weaponBytesOffset[0] + $weaponBytesOffset[1]);

    // truncate some of it
    $weaponBytes = substr($weaponBytes, $weaponBytesOffset[0], $weaponBytesOffset[1]);

    // close the file
    fclose($handle);

    return [
        trim($slotBytes), 
        trim($weaponBytes)
    ];
}

// Hume male 7373 = 28 /12
// {
//     "Identifier": "0hm_",
//     "Race": "Hume Male",
//     "Path": "ROM/28/12.DAT",
//     "model_id": 5,
//     "FileID": 7373,
//     "Type": "Body"
// },
$test = getDatForFileNumber(7373);

// The ID's are correct to the game, but the Names are just my own format
// You can change the actual name to however you like.
$race_list = [
    1 => "Hume_Male",
    2 => "Hume_Female",
    3 => "Elvaan_Male",
    4 => "Elvaan_Female",
    5 => "Tarutaru",
    6 => "Mithra",
    7 => "Galka"
];

// This list is just a "named" list against the race list and the type 
// which will be searched, for example for Main Mithra Weapons, it would be "main" then "mit_"
// for Hume Male it would be "main" then "00hm", the headers are in order of the race list above.
//
// todo - need to go through some of the Main/Sub/Range as they have values that don't start with 00
$slot_headers = [
    // "main"  => [ "", "00hm", "00hf", "00em", "00ef", "00tr", "00mt", "00gl" ], // 70hm
    // "sub"   => [ "", "01hm", "01hf", "01em", "01ef", "01tr", "01mt", "01gl" ],      // 15hm sword? 31hm axe
    // "range" => [ "", "02hm", "02hf", "02em", "02ef", "02tr", "02mt", "02gl" ],
    "body"  => [ "", "0hm_", "0hf_", "0em_", "0ef_", "0tr_", "0mt_", "0gl_" ],
    "head"  => [ "", "1hm_", "1hf_", "1em_", "1ef_", "1tr_", "1mt_", "1gl_" ],
    "hands" => [ "", "2hm_", "2hf_", "2em_", "2ef_", "2tr_", "2mt_", "2gl_" ],
    "legs"  => [ "", "3hm_", "3hf_", "3em_", "3ef_", "3tr_", "3mt_", "3gl_" ],
    "feet"  => [ "", "4hm_", "4hf_", "4em_", "4ef_", "4tr_", "4mt_", "4gl_" ],
];

$slot_names = [
    /*"main", "sub", "range",*/ "body", "head", "hands", "legs", "feet"
];

$weapon_list = [
    "hand2hand" => [ "clwj_mtd", "clw_e", "clq_drgn", "claw_dm", "hf_clw1", "hf_clw2", "hf_clw3", "hf_clw4", "hf_clw5", "hf_clw6", "hf_clw7", "hf_clw8", "hf_clw9", "hf_clwa", "claw_b", "clawb_d", "clawb_a", "claw_hl", "hf_clwd", "clwc", "cl_mog0", "claw_mt", "naknakl", "clwkl_0", "ahurama", "clwn_en", "clawb_s", "omg", "clw_eiy", "clw_ast", "p_heart", "clwn_d2", "hf_cl_g" ],
    "dagger"    => [ "knf_ald", "hf_knf1", "hf_knf2", "hf_knf3", "hf_knf4", "hf_knf5", "hf_knf6", "hf_knf7", "hf_knf8", "hf_knf9", "knife", "knfa", "hf_kknif", "knfa_s", "knfa_dk", "knfa_m", "vaj", "knife_c", "knf_dm", "e_knfe_l", "knife_h", "knfd", "kn_bst01", "oaknif_t", "knif_bs0", "ipetam", "knfd_dp", "knfnl_ha", "kn_mate", "knf4_sr", "kn_eiyu", "naknif_t", "knf_chkn", "knfe_d2", "an_knife", "kn_corp", "knfd_pur", "kn_air", "knf_l_b", "knf_l_g" ],
    "sword"     => [  "rap1", "rap2", "rap3", "rap4", "rap5", "rap6", "rap7", "rap8", "rap9", "sword", "swoe_mtd", "hf_rap1", "hf_rap2", "hf_rap3", "hf_rap4", "hf_rap5", "hf_rap6", "hf_rap7", "hf_rap8", "hf_rap9", "hf_5rap", "hf_swo1", "hf_swo2", "hf_swo3", "hf_swo4", "hf_swo5", "hf_swo6", "hf_swo7", "hf_swo8", "hf_swo9", "hf_swoa", "hf_swob", "hf_swoc", "hf_swod", "hf_swoe", "hf_swof", "hf_swog", "swoi", "sh2", "swoi_dk", "swoi_ad", "swoi_s", "sword_m", "swol", "ev", "swo9_s", "sword_o", "e_swd_l", "swd_u_cd", "hf_sw_h5", "hf_sw_ef", "oasword0", "nasword0", "typ_swor", "swo2d", "sw_mate", "swoi_da", "hf_swo2a", "hf_swoz0", "sw2cl_pm", "sword_p", "swo2e", "swo_ast", "swo2h", "sw2konio", "swo2g", "hf_sw_sq", "hf_swo2j", "d2_swd_l", "an_sword", "hf_s_win"  ],
    "g_sword"   => [ "iron_s", "hf_2hs1", "hf_2hs2", "hf_2hs3", "hf_2hs4", "hf_2hs5", "hf_2hs6", "hf_2hs7", "hf_2hs8", "hf_2hs9", "2hsp", "2hs_3_s", "2hs_8", "2hs8_go", "2hs8_ad", "2hs_a", "2hsc_ma", "2hs8_dk", "hf_2hsb", "2hs_ice", "2h_b01", "oa2hs01", "n2hs_g1", "on2hs01", "2hs_j_tb", "maken1", "maken2", "2hs_8_ty", "2hsl_ha", "2hsk_01", "2hs_eiyu", "2hs_ast", "2hs_brav", "2hs_d2", "2hs_maou", "2hs_ice2", "od_sword", "2hsplb" ],
    "axe"       => [ "axeg_tbd", "hf_axe1", "hf_axe2", "hf_axe3", "hf_axe4", "hf_axe5", "hf_axe6", "hf_axe7", "hf_axe8", "hf_axe9", "axe8", "hf_axa9", "hf_axe5m", "hf_axa9a", "oaaxe01", "ax_qb", "axegr_tb", "axe_mate", "axe_i_en", "ax_eiyu", "axe_nf", "axe_i_d2", "axe_l01", "an_axe", "hf_ax_mo", "dla_axe" ],
    "g_axe"     => [ "iron_h", "b_axek", "hf_b_ax1", "hf_b_ax2", "hf_b_ax3", "hf_b_ax4", "hf_b_ax5", "hf_b_ax6", "hf_b_ax7", "hf_b_ax8", "hf_b_ax9", "hf_bax6", "b_axe_7", "baxe7_dk", "baxe8", "hf_bax6", "b_axe9", "baxe7_ad", "na_dax01", "svarga_n", "b_oosan", "bax6_sin", "b_axf", "hf_baxa", "bax_eiyu", "b_axf_d2", "hf_b_axw", "b_axlb" ],
    "scythe"    => [ "hf_scy1", "hf_scy2", "hf_scy3", "hf_scy4", "hf_scy5", "hf_scy6", "hf_scy7", "hf_scy8", "hf_scy9", "scy4_ng", "scy5_lb", "scythe_1", "scythe_2", "scythe_3", "scythe_4", "scythe_5", "scythe_6", "scythe_7", "scythe_8", "scythe_9", "hf_scy_5", "nasyth_t", "scyd_tb", "scythe_e", "scy4_sin", "scyh_en", "scy_g_se", "scyi_eiu", "scyh_d2", "scyj_de", "hf_scy3r" ],
    "polearm"   => [ "speaj_si", "hf_spea1", "hf_spea2", "hf_spea3", "hf_spea4", "hf_spea5", "hf_spea6", "hf_spea7", "hf_spea8", "hf_spea9", "spear_a", "spear9_a", "spear9_b", "spear9_c", "spear9_d", "spear9_e", "spear9_f", "spear9_g", "spear_1", "spear_2", "spear_3", "spear_4", "spear_5", "spear_6", "spear_7", "spear_8", "spear_9", "sp_o", "hf_speaa", "hf_speab", "hf_speac", "hf_spead", "hf_speae", "hf_speaf", "hf_speag", "en_spr", "nagi", "oaspea_0", "naspea_0", "speal_pe", "speak_01", "sp_eiyu", "spr_d2", "sp_yag", "hou_yari", "an_spear", "hf_spe_p", "hf_speaf", "sagojou" ],
    "katana"    => [ "sinai_n", "hf_shie", "pmsw01", "hf_shi1", "hf_shi2", "hf_shi3", "hf_shi4", "hf_shi5", "hf_shi6", "hf_shi7", "hf_shi8", "hf_shi9", "hf_swoh", "shinobi7", "naktatu", "shi_dp", "sino7sin", "shic_en", "shd_eiyu", "shic_d2", "hf_shi_w" ],
    "g_katana"  => [ "hf_kata", "hf_kata1", "hf_kata2", "hf_kata3", "hf_kata4", "hf_kata5", "hf_kata6", "hf_kata7", "hf_kata8", "hf_kata9", "katana8", "boku", "muramasa", "sinai", "katab", "hf_kataa", "hf_katab", "hf_katac", "hf_katad", "hf_katae", "hf_kataf", "hf_katag", "nakatan0", "kata_en", "katnk_ei", "ka_zanma", "kata_d2", "hf_ka_se", "katn_mut" ],
    "club"      => [ "sti_f_yg", "clbm_uch", "hf_culbs", "clbb", "stksp01", "hf_clb1", "hf_clb2", "hf_clb3", "hf_clb4", "hf_clb5", "hf_clb6", "hf_clb7", "hf_clb8", "hf_clb9", "hf_sti1", "hf_sti2", "hf_sti3", "hf_sti4", "hf_sti5", "hf_sti6", "hf_sti7", "hf_sti8", "hf_sti9", "valen", "stia", "stia_1", "stia_2", "stia_3", "stia_4", "stia_5", "stia_6", "stia_7", "stia_8", "stia_9", "club_1", "club_2", "club_3", "club_4", "club_5", "club_6", "club_7", "club_8", "club_9", "club_a", "club_b", "club_c", "club_d", "club_e", "club_f", "club_g", "hf_clbc", "pikohan", "hf_st_b", "m_conda1", "m_normal", "hf_clu_f", "stk_kani", "clb_gob", "oaclub01", "ygdra_st", "naclub01", "clbl01", "clbl02", "fusui1", "2hs_maou", "sharur", "club_lea", "stii", "st_mate", "en_wan", "club_9hd", "hf_culbf", "hero", "clb_eiyu", "club_cha", "clb_uchb", "stick_d2", "mf_tue1", "clbb02", "hf_sti5s", "clb_e_ne", "mf_tue2" ],
    "staff"     => [ "wandy_ke", "hf_wands", "wandt", "wandt2", "wan_lich", "hf_wand1", "hf_wand2", "hf_wand3", "hf_wand4", "hf_wand5", "hf_wand6", "hf_wand7", "hf_wand8", "hf_wand9", "wand8bk", "wand8bl", "wand8g", "wand8lb", "wand8p", "wand8", "wand8w", "wand8y", "wand9", "hf_wanda", "hf_wandb", "hf_wandc", "hf_wandd", "hf_wande", "hf_wandf", "hf_wandg", "wandb", "wandf", "wand_g", "hf_wani", "en_wan", "wand_l_c", "wand8ra", "wandn", "wn2wn_o1", "na2hwnd", "en_wan", "hf_wnd_o", "wand8_si", "wandq01", "wanp_eiu", "pr_wndu", "wa_lamia", "wan_d2", "an_wand", "hf_wan_s", "twcwnd1" ],
    "shield"    => [ "shiel", "oa_lshif", "seka01", "sld9_mg1", "shield", "l_shield", "hf_lshi1", "hf_lshi2", "hf_lshi3", "hf_lshi4", "hf_lshi5", "hf_lshi6", "hf_lshi7", "hf_lshi8", "hf_lshi9", "ls7", "hf_lshia", "hf_lshib", "hf_lshic", "hf_lshid", "hf_lshie", "hf_lshif", "hf_lshig", "l_sd_a", "lshi1_oo", "shld000", "l_shi_cd", "m_shld1", "m_shld2", "m_shld3", "m_shld4", "m_shld5", "m_shld5", "m_shld6", "m_shld7", "m_shld8", "m_shld9", "shi_cont", "na_lshif", "shie_sli", "lshih01", "shiec_01", "moru_01", "ls_mate", "lshil_en", "shi_ome", "lshil_ei", "cat_sld", "lshi_mok", "lshi_kaj", "lshi_cyk", "lshi_sai", "lshi_kaw", "lshi_hor", "lshi_ren", "lshi_cyr", "lshindia", "hf_lshio", "dla_sld", "moru_02", "l_shi_e", ],
    "throwing"  => [ "hf_boom1", "hf_boom2", "hf_boom3", "hf_boom4", "hf_boom5", "hf_boom6", "hf_boom7", "hf_boom8", "hf_boom9", "hf_booma", "hf_boomb", "hf_boomc", "hf_boomd", "hf_boome", "hf_boomf", "hf_boomg", "th_axe1", "th_axe2", "th_axe3", "th_axe4", "th_axe5", "th_axe6", "th_axe7", "th_axe8", "th_axe9", "th_spr1", "th_spr2", "th_spr3", "th_spr4", "th_spr5", "th_spr6", "th_spr7", "th_spr8", "th_spr9", "boom1", "boom2", "boom3", "boom4", "boom5", "boom6", "boom7", "boom8", "boom9" ],
    "archery"   => [ "bwb_blda", "hf_bow1", "hf_bow2", "hf_bow3", "hf_bow4", "hf_bow5", "hf_bow6", "hf_bow7", "hf_bow8", "bow_eiy", "bowd_en", "hf_bow_s", "hf_bow", "bow_arte", "bowd_d2", "bow6", "oabow_01", "nabow_01", "bwb_bl01", "phaespor", "bow_mate", "boe_pmt", "bow_eiyu", "bow_iroh", "hf_bow_s" ],
    "gun"       => [ "croi_exe", "hf_cro1", "hf_cro2", "hf_cro3", "hf_cro4", "hf_cro5", "hf_cro6", "hf_cro7", "hf_cro8", "hf_cro9", "l_m9p13", "cro1", "cro2", "cro3", "cro4", "cro5", "cro6", "cro7", "cro8", "cro9", "cro7s", "cro7_dk", "cro7_oh", "croa", "crob", "croc", "crod", "croe", "crof", "crof", "na_cb01", "camera", "crof_en", "cbw_g_ei", "crof_d2", "cbw_trol" ],
    "wind"      => [ "hf_flu1", "hf_flu2", "hf_flu3", "hf_flu4", "hf_flu5", "hf_flu6", "hf_flu7", "hf_flu8", "hf_flu9", "nahorn_t", "linos", "flu_eiyu", "flu6_mv", "flu1_mv", "flu2_mv", "flu3_mv", "flu4_mv", "flu5_mv", "flu6_mv", "flu7_mv", "flu8_mv", "flu9_mv" ],
    "string"    => [ "hf_harp1", "hf_harp2", "hf_harp3", "hf_harp4", "hf_harp5", "hf_harp6", "hf_harp7", "hf_harp8", "hf_harp9", "harp1_si", "harp4_en", "harp1", "harp2", "harp3", "harp4", "harp5", "harp6", "harp7", "harp8", "harp9" ],
    "bell"      => [ "f_bel01", "f_bel02", "f_bel03", "f_bel04", "f_bel05", "f_bel06", "f_bel07", "f_bel08", "f_bel09" ],
];

//
// Start mass scanning all files
//

$model_id_list = [];

// The total number of files to look for, if we hit the end of the vtable, it will stop anyway.
$maxFiles = 1000000;

$ffxi_scan = "6j0";
$ffxi_results = [];
$ffxi_rom_root = "D:\\SquareEnix\\SquareEnix\\PlayOnline\\SquareEnix\\FINAL FANTASY XI\\";
$ffxi_rom_folders = [
    "ROM", "ROM2", "ROM3", "ROM4", "ROM5", "ROM6", "ROM7", "ROM8", "ROM9"
];

function findDatFiles($dir, &$datFilesArray, $parentPath = '') {
    $files = scandir($dir);

    foreach ($files as $file) {
        if ($file === '.' || $file === '..') {
            continue;
        }

        $path = $dir . DIRECTORY_SEPARATOR . $file;

        if (is_file($path) && pathinfo($file, PATHINFO_EXTENSION) === 'DAT') {
            $datFilesArray[] = $parentPath . DIRECTORY_SEPARATOR . $file;
        } elseif (is_dir($path)) {
            findDatFiles($path, $datFilesArray, $parentPath . DIRECTORY_SEPARATOR . $file);
        }
    }
}

foreach ($ffxi_rom_folders as $rom) {
    $ffxi_rom_path = "{$ffxi_rom_root}{$rom}";

    echo("Building dat list: {$ffxi_rom_path} \n");

    $ffxi_rom_dat_folders = [];

    findDatFiles($ffxi_rom_path, $ffxi_rom_dat_folders);

    $ffxi_count = count($ffxi_rom_dat_folders);

    echo("Total = {$ffxi_count} \n");
    echo("Scanning ... \n");

    foreach ($ffxi_rom_dat_folders as $folder) {
        $fulldat = "{$ffxi_rom_path}{$folder}";
        echo("> SCAN: {$fulldat} \n");

        // get headers
        $headers = getFileHeaders("{$rom}\\{$folder}");

        // the two types of headers we look for
        $header_slot = $headers[0];
        $header_weapon = $headers[1];

        if (stripos($header_slot, $ffxi_scan) !== false || stripos($header_weapon, $ffxi_scan) !== false) {
            $ffxi_results[] = $fulldat;
        }
    }

    break;
}

echo("-- Finished scanning ... \n");
print_r($ffxi_results);

die;

// First we handle per race
foreach($race_list as $race_id => $race_name) {
    echo("--- Looking for: {$race_name} equipment... \n");

    // count for race
    $count_for_race = 0;

    // then we handle per slot
    foreach ($slot_names as $slot_name) {
        echo("--- Slot: {$slot_name} \n");

        // count for slot
        $count_for_slot = 0;

        // now we scan the entire list
        foreach(range(0, $maxFiles) as $i => $model_id) {
            $dat = getDatForFileNumber($model_id);
        
            // if dat returns false, there are no more in the vtable.
            if ($dat === false) {
                echo("- You have reached the end of the FTABLE for this race_slot. \n");
                break;
            }
        
            // The respected file ID, Dat Directory and Dat Name
            $dat_id = $dat[0];
            $dat_dir = $dat[1];
            $dat_name = $dat[2];
        
            // All gear is in ROM, it's never in Rom 2-9 because if you don't own the expansions you won't
            // have these folders, however you always need the gear because other people + npcs can wear expansion items.
            $dat_path = "ROM\\{$dat_dir}\\{$dat_name}.DAT";
        
            // Grab the headers from the file
            $headers = getFileHeaders($dat_path);

            // the two types of headers we look for
            $header_slot = $headers[0];
            $header_weapon = $headers[1];

            // This is the expected header type for this race + slot
            $expected_header_slot = $slot_headers[$slot_name][$race_id];

            // if the header is not the expected type, we continue
            if ($header_slot != $expected_header_slot) {
                continue;
            }
            
            // log
            echo("- ($model_id) {$race_name} / ({$header_slot}) {$slot_name} = {$dat_path}\n");
        
            // record
            $model_id_list[$race_name][$slot_name][$model_id] = $dat_path;
            
            // counts
            $count_for_race++;
            $count_for_slot++;
        }

        // report how many found
        echo("--- Found: {$count_for_slot} items for: {$race_name} !\n");
    }

    // report how many found
    echo("--- Found: {$count_for_race} items for: {$race_name} !\n");
}


echo("- Writing out json lists...\n");

// 
// Rebase arrays so the model_ids match up, this will also append the "model id" onto the string of the dat path
//
foreach ($model_id_list as $race_name => $slot_list) {
    foreach($slot_list as $slot_name => $models) {

        $model_id = 0;
        $model_list = [];

        foreach ($models as $file_id => $dat_path) {
            $model_list[] = "{$model_id}|{$file_id}|{$dat_path}";

            // increment the model id
            $model_id++;
        }

        // replace the list with the updated model id increment
        $model_id_list[$race_name][$slot_name] = $model_list;
    }
}

file_put_contents(__DIR__ .'/LookToFile_List.json', json_encode($model_id_list, JSON_PRETTY_PRINT));

echo("Finished!\n\n");