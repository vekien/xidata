0x1000 head
0x2000 body
0x3000 hands
0x4000 legs
0x5000 feet
0x6000 main
0x7000 sub
0x8000 ranged

SELECT *, MID + 0x1000 AS model_id FROM item_equipment WHERE slot = 16 ORDER BY name -- head
SELECT *, MID + 0x2000 AS model_id FROM item_equipment WHERE slot = 32 ORDER BY name -- body
SELECT *, MID + 0x3000 AS model_id FROM item_equipment WHERE slot = 64 ORDER BY name -- hands
SELECT *, MID + 0x4000 AS model_id FROM item_equipment WHERE slot = 128 ORDER BY name -- legs
SELECT *, MID + 0x5000 AS model_id FROM item_equipment WHERE slot = 256 ORDER BY name -- feet
SELECT *, MID + 0x6000 AS model_id FROM item_equipment WHERE slot = 1 ORDER BY name -- main
SELECT *, MID + 0x7000 AS model_id FROM item_equipment WHERE slot = 3 OR slot = 2 AND MID > 0 ORDER BY name -- sub
SELECT *, MID + 0x8000 AS model_id FROM item_equipment WHERE slot = 4 ORDER BY name -- ranged