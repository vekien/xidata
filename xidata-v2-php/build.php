<?php

echo ("\nBUILDING, EVERYTHING\n");
sleep(1);

echo("\nBUILDING ANIMATIONS\n");
require_once __DIR__ . "/build_animations.php";
sleep(1);

echo("\nBUILDING EFFECTS\n");
require_once __DIR__ . "/build_effects.php";
sleep(1);

echo("\nBUILDING GEAR\n");
require_once __DIR__ . "/build_gear.php";
sleep(1);

echo("\nBUILDING NPC\n");
require_once __DIR__ . "/build_npc.php";
sleep(1);

echo("\nBUILDING ZONES\n");
require_once __DIR__ . "/build_zones.php";
sleep(1);

echo("\nBUILDING FACES\n");
require_once __DIR__ . "/build_faces.php";
sleep(1);