using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly float JUDGE_LINE_SIZE = 3f;
    public static readonly float JUDGE_BASE_SIZE = 0.3f;

    public static readonly float PERFECT_TIMING = 0.15f;
    public static readonly float GOOD_TIMING = 0.3f;
    public static readonly float STUN_DURATION = 1f;
    public static readonly float INVINCIBLE_DURATION = 0.3f;

    public static readonly int MAX_HEALTH = 3;
    public static readonly float ATTACK_RADIUS = 3f;

    public static readonly int ENEMY_LAYER = 9;

    public static readonly string NORMAL_ENEMY = "normal";
    public static readonly string FAST_ENEMY = "fast";
    public static readonly string TANK_ENEMY = "tank";
    public static readonly string ULTRAFAST_ENEMY = "ultrafast";

    public static readonly string[] SPAWN_PATTERN_RESOURCES = 
    {
        "enemyPatternStage1.json"
    };
}
