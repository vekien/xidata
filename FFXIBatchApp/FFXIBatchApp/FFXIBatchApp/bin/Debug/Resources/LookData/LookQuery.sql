SELECT 
    npc_list.npcid as npc_id,
    npc_list.polutils_name as npc_name,
    npc_list.pos_rot as npc_pos_rotation,
    npc_list.pos_x as npc_pos_x,
    npc_list.pos_y as npc_pos_y,
    npc_list.pos_z as npc_pos_z,
    HEX(`look`) as npc_look,
    npc_list.content_tag as npc_content,
    zone_settings.name as zone_name
FROM npc_list 
INNER JOIN zone_settings
ON (npc_list.npcid & 0xFFF000) >> 12 = zone_settings.zoneid
WHERE 
    npc_list.polutils_name != ''
    AND npc_list.entityFlags IN (3, 25, 27, 29)
    AND npc_list.polutils_name != '???' -- can't be arsed dealing with these...
    AND npc_list.status = 0