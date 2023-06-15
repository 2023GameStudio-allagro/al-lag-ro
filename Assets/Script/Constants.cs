using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const float BASE_DURATION_BEAT = 8;
    public const float BASE_ENEMY_DISTANCE = 15;
    public const float LEFT_BOUND = -12f;

    public const float JUDGE_LINE_SIZE = 3f;
    public const float JUDGE_BASE_SIZE = 0.3f;

    public const float PERFECT_TIMING = 0.15f;
    public const float GOOD_TIMING = 0.3f;
    public const float STUN_DURATION = 0.3f;
    public const float INVINCIBLE_DURATION = 0.3f;

    public const int MAX_HEALTH = 3;
    public const float ATTACK_RADIUS = 3f;

    public const int ENEMY_LAYER = 9;

    public const string NORMAL_ENEMY = "normal";
    public const string FAST_ENEMY = "fast";
    public const string TANK_ENEMY = "tank";
    public const string ULTRAFAST_ENEMY = "ultrafast";

    public static readonly string[] SPAWN_PATTERN_RESOURCES = 
    {
        "enemyPatternStage1"
    };
    internal static int CELL_SIZE = 16;
    public const string TITLE_BGM_RUNNER = "Title BGM";
    public const string RESULT_UI_OBJECT = "Result UI";
    public const string SLOW_ZONE_RUNNER = "Beat Manager";
}
