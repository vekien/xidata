<?php

function signed2hex($value, $reverseEndianness = true) {
    $packed = pack('s', $value);
    $hex='';
    for ($i=0; $i < 2; $i++){
        $hex .= strtoupper( str_pad( dechex(ord($packed[$i])) , 2, '0', STR_PAD_LEFT) );
    }
    $tmp = str_split($hex, 2);
    $out = implode('', ($reverseEndianness ? array_reverse($tmp) : $tmp));
    return $out;
}

// race, face, head, body ,hands, legs, feet, main, sub, ranged
$model = "29,1,199,77,159,62,195,426,51,0";
$model = explode(",", $model);

$face   = dechex($model[0]);
$race   = dechex($model[1]);
$head   = signed2hex($model[2]);
$body   = signed2hex($model[3]);
$hands  = signed2hex($model[4]);
$legs   = signed2hex($model[5]);
$feet   = signed2hex($model[6]);
$main   = signed2hex($model[7]);
$sub    = signed2hex($model[8]);
$ranged = signed2hex($model[9]);

$look = "0x0100{$face}{$race}{$head}{$body}{$hands}{$legs}{$feet}{$main}{$sub}{$ranged}";

print_r($look);