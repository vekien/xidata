# Batch Exporting NPC v2

todo - write a batch exporter
todo - write all datsets to a csv for importing into things like Unreal Engine / Unity.

------

This one is a bit different, there are a lot of NPCs which are simply just PC's with various combinations of gear, they do not have anything unique to them so they're not their own little DAT file.

In order to build a list of Noesis `ff11datsets` for every NPC in the game (just about) we need to be able to decode the `look` packet that the server sends.

Here is the look for: Sagheera
- `0100080487108720A0308740A050006000700000`

You can find these here: https://github.com/LandSandBoat/server/blob/base/sql/npc_list.sql

You can also use NPC Decoder to find them, which is a Windower Addon, this will save information in real time and you can debug out the look at line 141
- https://github.com/StarlitGhost/NPCDecoder

When decoding this look, we will be using 2 lookup json tables:
- Gear: https://github.com/MurphyCodes/FFXI-DATS/tree/main/Race%20specific%20model%20dats
- Faces: https://github.com/StarlitGhost/NPCDecoder/tree/main/json 

**Taking our example: `0100080487108720A0308740A050006000700000`**

We split it up by the first 8 characters.

```
| NPC  |   | --- this is all gear --------|
01000804   87108720A0308740A050006000700000
```

> Note: Sometimes you will have this starting with 0x, eg: `0x0100080487108720A0308740A050006000700000` - Just remove the 0x

The NPC section can be split up into 2 characters each

```
01 - Skip this
00 - Lockstyle or not
08 - Face, hex > dec it. In this case: Face = 8. Face ID's start from 1
04 - Race, hex > dec it. In this case: Race = 4. Race ID's start from 1 so this would be Elvaan Female
```

Next split up each section by 4, they're structured like so:
```
8710 - head
8720 - body
A030 - hands
8740 - legs
A050 - feet
0060 - main
0070 - sub
0000 - ranged
```

NPC's will very rarely have a ranged.

To decode each slot, you need to split it by 2 then modify it slightly.

Here is code example in PHP

```php
    $a = hexdec(substr($lookdata, 0, 2));
    $b = hexdec(substr($lookdata, 2, 2));

    $shift = $b << 8;
    $combined = $a + $shift;
    $masked = $combined & 0x0FFF;

    return $masked;
```

And an example in Python

```python
def getModelIdFromLookData(lookdata):
    a = int(lookdata[0:2], 16)
    b = int(lookdata[2:4], 16)

    shift = b << 8
    combined = a + shift
    masked = combined & 0x0FFF

    return masked
```

To get a full list of Noesis ff11dataset items, run my PHP code:

- `php LookDecoder.php`

I have also included my current extract here.

It won't get completely new stuff, like a lot of abyssea fails because the dats are new and not in the JSONs.

Someday I want to update those JSON files.

todo: Using these ff11datset files we can automate the extract using a similar approach to NPC and the AHK script.