using System.Numerics;
using DeadworksManaged.Api;

namespace DeadlockMovementTraining;

public partial class DeadlockMovementTraining
{
    private readonly Vector3[] _spawnPoints =
    [
        // leif_zip_spot1
        new(156.03125f, -2954.5f, 639.84375f),
        // leif_zip_spot2
        new(-814.4375f, -3492.3125f, 640.0625f),
        // leif_zip_spot3
        new(-8619.6875f, -2127.03125f, 256.0625f),
        // leif_zip_spot4
        new(7497.46875f, -2078f, 256.0625f),
        // leif_zip_spot5
        new(-165.8125f, 3025.6875f, 640.96875f),
        // leif_zip_spot6
        new(586.25f, 3388.4375f, 640f),
        // leif_zip_spot7
        new(8528.25f, 2115f, 256f),
        // leif_zip_spot8
        new(-7416.25f, 1965.28125f, 256.0625f),
        // leif_vent_spot1
        new(-2153.15625f, 527.8125f, 703.84375f),
        // leif_vent_spot2
        new(-3802.75f, -1454.71875f, 893.6875f),
        // leif_vent_spot3
        new(-5248.78125f, -1588.875f, 1144.03125f),
        // leif_vent_spot4
        new(-5993.15625f, -1225.84375f, 563.125f),
        // leif_vent_spot5
        new(2187.5625f, -581.375f, 704.0625f),
        // leif_vent_spot6
        new(3849.46875f, 1415.125f, 1128f),
        // leif_vent_spot7
        new(5164.28125f, 1135.71875f, 1169.03125f),
        // leif_vent_spot8
        new(5985.5625f, 1213.15625f, 563.125f),
        // leif_vent_spot9
        new(-4504.09375f, 2246.3125f, 624f),
        // leif_vent_spot10
        new(-1833.5f, 2120.09375f, 609.0625f),
        // leif_vent_spot11
        new(-1217.46875f, 2796.21875f, 656.03125f),
        // leif_vent_spot12
        new(4458.5f, -2266.15625f, 624.0625f),
        // leif_vent_spot13
        new(1940.65625f, -2142.5625f, 768.0625f),
        // leif_vent_spot14
        new(2027.6875f, -2426.15625f, 768.0625f),
        // leif_vent_spot15
        new(2680.53125f, -3888.625f, 640.03125f),
        // leif_vent_spot16
        new(-2708.375f, 3835.21875f, 640.03125f),
        // leif_mboost_spot1
        new(607.375f, -346.625f, 248.03125f),
        // leif_mboost_spot2
        new(-1228.28125f, -377.0625f, 384.03125f),
        // leif_mboost_spot3
        new(-1467.5625f, -372.3125f, 256f),
        // leif_mboost_spot4
        new(-3330.3125f, -220.25f, 256.03125f),
        // leif_mboost_spot5
        new(-3758.78125f, -210.71875f, 256.03125f),
        // leif_mboost_spot6
        new(-4059.3125f, -229.59375f, 256.03125f),
        // leif_mboost_spot7
        new(-5426.09375f, -219.28125f, 256.03125f),
        // leif_mboost_spot8
        new(-5897.1875f, -511.125f, 256.03125f),
        // leif_mboost_spot9
        new(-7776.53125f, -1329.53125f, 248.03125f),
        // leif_mboost_spot10
        new(-7969.9375f, -803.0625f, 160.1875f),
        // leif_mboost_spot11
        new(-7882.6875f, -1964.96875f, 256.0625f),
        // leif_mboost_spot12
        new(-3529.125f, 467.5f, 256.03125f),
        // leif_mboost_spot13
        new(-7507.09375f, 1035.71875f, 136.75f),
        // leif_mboost_spot14
        new(-7513.125f, 1248.03125f, 256f),
        // leif_mboost_spot15
        new(-5801.125f, 413.53125f, 128f),
        // leif_mboost_spot16
        new(-5494.03125f, 1135.75f, 256.0625f),
        // leif_mboost_spot17
        new(-1260.625f, -1448.125f, 384.03125f),
        // leif_mboost_spot18
        new(-4314.625f, -2975.71875f, 384.03125f),
        // leif_mboost_spot19
        new(-5055.21875f, -3233.6875f, 450.84375f),
        // leif_mboost_spot20
        new(145.1875f, -735.9375f, 257.8125f),
        // leif_mboost_spot21
        new(-2850.3125f, -2791.3125f, 640.03125f),
        // leif_mboost_spot22
        new(-1979.96875f, -4341.65625f, 512f),
        // leif_mboost_spot23
        new(1882.21875f, -3397.59375f, 1040.03125f),
        // leif_mboost_spot24
        new(3060.1875f, -2493.3125f, 384.03125f),
        // leif_mboost_spot25
        new(2195.46875f, -1073.1875f, 384.03125f),
        // leif_mboost_spot26
        new(5189.78125f, 694.125f, 256.03125f),
        // leif_mboost_spot27
        new(-5153.4375f, 1600.34375f, 928f),
        // leif_mboost_spot28
        new(5209.59375f, -1591.4375f, 928f),
        // leif_mboost_spot29
        new(-2109.90625f, 3251.09375f, 384.0625f),
        // leif_mboost_spot30
        new(564.59375f, 878.5f, 384.0625f),
        // leif_mboost_spot31
        new(-1113.3125f, 3648.84375f, 656f),
        // leif_mboost_spot32
        new(-944.53125f, 3842.46875f, 384.0625f),
        // leif_mboost_spot33
        new(-3954.5625f, -1645.5f, 893.71875f),
        // leif_mboost_spot34
        new(-3672.875f, -3136.65625f, 1216.0625f),
        // leif_basezip_spot1
        new(-1234.71875f, 6723.5625f, 1120f),
        // leif_basezip_spot2
        new(-0.375f, 6045.5f, 1120.03125f),
        // leif_basezip_spot3
        new(1217.3125f, 6776.03125f, 1120f),
        // leif_basezipm_spot1
        new(1228.28125f, -6737.28125f, 1152.0625f),
        // leif_basezipm_spot2
        new(-16.90625f, -6112.25f, 1152f),
        // leif_basezipm_spot3
        new(-1251.9375f, -6754f, 1152.0625f),
        // leif_urn_spot1
        new(-6583.625f, 1.40625f, 132.0625f),
        // leif_urn_spot2
        new(6565.40625f, 3.8125f, 128f),
    ];

    private static readonly TrainingGroup[] TrainingGroups =
    [
        new("zip",     "Zipline",      "Side 1 and Side 2 Zipline rotation practice spots loaded.", 0,  8),
        new("vent",    "Vent",         "All vent spots loaded.",                                     8,  16),
        new("mboost",  "Mantle Boost", "34 mantle to edgeboost spots loaded.",                      24, 34),
        new("basezip", "Base Zip",     "Base ziplines loaded.",                                      58, 6),
        new("urn",     "Urn",          "Urn spots loaded.",                                          64, 2),
    ];

    private static readonly Dictionary<int, string> TrainingSpotHints = new()
    {
        [0]  = "Blue -> Yellow: Zip toward base. Aim near the windows on the diagonal portion of the wall. . . . Zipdash right as soon as the blue zipline is aligned with you and strafe right. 2) Blue -> Sinners: Zip toward base. Aim at the vertical billboard to the right of the Broadway sign. . . . Zipdash right as soon as the blue zipline is aligned with you and strafe right. 3) Blue -> Sinners (height): Some characters (like doorman) ride zips higher and can cornerboost on the billboard on the right. . . . Aim at the top of the vertical zipline and zipdash right as soon as the close lamp leaves the screen.",
        [1]  = "Blue -> Green: Zip toward lane. Aim to the right until you pass Mildred Visions, then aim at the vertical grey line on the next building and zipdash right. . . . If you get stopped or bounced away from the wall, time the zipdash later.",
        [2]  = "Yellow -> Blue: Zip toward base. Aim at the red pipe, once it is all in view, zipdash left. . . . via Sinners: Edgeboost on the vertical dark pipe or wait and walljump behind it, strafe left. . . . via height: Walljump on the left wall and strafe around the shed to the right. Edgeboost on the wall inside warehouse and grab zip. . . . Try to avoid sliding across the green bridge, as it will cause slide speed decay.",
        [3]  = "Green -> Blue: Zip toward the base. Aim toward Blue Walker. Zipdash to the right before the lightpost gets to your crosshair. . . . Doublejump somewhat early and strafe into the door to the closet on the bridge and try to hit an edgeboost.",
        [4]  = "Blue -> Green: Zip toward base. Aim near the windows on the diagonal portion of the wall. . . . Zipdash right as soon as the blue zipline is aligned with you and strafe right. 2) Blue -> Sinners: Zip toward base. Aim at the white tower to the right of the Noir sign . . . Zipdash right as soon as the blue zipline is aligned with you and strafe right.",
        [5]  = "Blue -> Yellow: Zip toward lane. Aim to the right until you pass Mildred Visions . . . Aim between the middle and right window on the next building and zipdash right. . . . Strafe as needed to hit the rounded corner and walljump right toward the vent, proceed to edgeboostville",
        [6]  = "Yellow -> Blue: Zip toward base. Aim at the Danas sign, once it is all in view, zipdash left. . . . via Sinners: Edgeboost on the vertical dark pipe or wait and walljump behind it, strafe left. . . . via height: Walljump on the left wall and strafe around the shed to the right. Edgeboost on the wall inside warehouse and grab zip. . . . Try to avoid sliding across the green bridge, as it will cause slide speed decay.",
        [7]  = "Green -> Blue: Zip toward the base. Aim toward Blue Walker. Zipdash to the right before the lightpost gets to your crosshair. . . . Slide through the veil and edgeboost on the wall on the right side. Walljump up the stairs.",
        [24] = "1) Don't mantle too close to the bridge. Don't mantle too far either. Look slightly away from the wall and hold W+A. . . . A success should give more than 850 velocity, though it may not always register in showpos. Getting less means you are doing a normal walljump.",
        [25] = "2) Only jump once. Look almost parallel to the pillar, a little away, and hold W+D. Time the jump to get the edgeboost.",
        [26] = "3) Only jump once. Look parallel to the pillar and hold W+A. Time the jump to get the edgeboost.",
        [27] = "4) Mantle as close to the sign on the railing as you can. Only jump once. . . . Look perpendicular to the railing, then go about 10 degrees to the left. Hold W+A and time the jump to get the edgeboost. . . . If you are too far from the sign, your angle may need to be further left.",
        [28] = "5) Mantle as close to the fixture on the railing as you can. Only jump once. . . . Look perpendicular to the railing, then go about 10 degrees to the right. Hold W+D and time the jump to get the edgeboost. . . . If you are too far from the sign, your angle may need to be further right.",
        [29] = "6) Mantle as close to the fixture on the railing as you can. Only jump once. . . . Look perpendicular to the railing, then go about 10 degrees to the left. Hold W+A and time the jump to get the edgeboost. . . . If you are too far from the sign, your angle may need to be further left.",
        [30] = "7) Mantle as close to the fixture on the railing as you can. Only jump once. . . . Look perpendicular to the railing, then go about 10 degrees to the right. Hold W+D and time the jump to get the edgeboost. . . . If you are too far from the sign, your angle may need to be further right.",
        [31] = "8) Mantle as close to the fixture on the railing as you can. Only jump once. . . . Look perpendicular to the railing, then go about 10 degrees to the right. Hold W+D and time the jump to get the edgeboost. . . . If you are too far from the sign, your angle may need to be further right.",
        [32] = "9) Mantle the barrels parallel to the face of the sign. Look right about 30 degrees and hold W+A. Jump twice or scroll wheel to get the boost.",
        [33] = "10) Mantle the ledge on the stairs while looking 15-20 degrees left of your guardian. . . . Hold w until you tap jump once for the superglide, then release it. Tap space again to get the boost.",
        [34] = "11) Mantle the sign as close to the wall as possible, then look about 25 degrees to the right. . . . Hold W+D and only jump once to hit the boost.",
        [35] = "12) Mantle as close to the fixture on the railing as you can. Only jump once. . . . Look perpendicular to the railing, then go about 10 degrees to the right. Hold W+D and time the jump to get the edgeboost. . . . If you are too far from the sign, your angle may need to be further right.",
        [36] = "13) Face the Uptown sign and try to mantle the railing as close to the fixture as possible, then look 5-10 degrees to the left. . . . Hold W+A and only jump once to hit the boost.",
        [37] = "14) Face the lane and mantle the railing as close to the fixture as possible, then look 5-10 degrees to the left. . . . Try not to bump the railing before you mantle it. Hold W+A and only jump once to hit the boost.",
        [38] = "15) Look toward Mid boss and mantle the wall the statue is sitting on. Look 10-20 degrees to the right and hold W+A. . . . Time two jumps, one for the superglide and one a little later for the boost.",
        [39] = "16) Slide the first set of stairs and jump right before the lower set, then walljump to the small railing section and try to hit the mantleboost.",
        [40] = "17) Mantle the railing and slide while looking very slightly into the wall, then look away and hold A . . . Time a jump when you pass the small vertical grey pillar to edgeboost.",
        [41] = "18) Mantle the box and look a bit into the wall to make sure you run into it when you superglide. . . . Hit an edgeboost when you reach the door.",
        [42] = "19) This one varies wildly depending on where exactly you mantle the shed. Don't get too used to the angle you use from the reset point since it'll change very easily.",
        [43] = "20) Mantle the ledge to the right of the stairs toward your guardian. . . . Look a bit to the right and superglide to try and hit the side of the small wall behind the statue. . . . Hold w until you jump, then release it immediately and look left. Time a second jump for the boost while holding D.",
        [44] = "21) Mantle the window facing the sinners building. You want your character to be in the middle of the window. . . . Look right 10-20 degrees and hold W+D, then scroll wheel some jumps to hit the boost.",
        [45] = "22) Mantleslide the railing and look 20-30 degrees left, hold W+A. Jump only once after the slide.",
        [46] = "23) Mantleslide the box and look 20-30 degrees left, hold W+A. Jump only once after the slide. . . . You can also hold W while looking slightly into the wall, but this could end up mantling it.",
        [47] = "24) Superglide on the dumpster and look slightly into the wall on the right, then look away from the wall and hold D. Time your jump.",
        [48] = "25) Mantleslide the shelving with the paint can on it, aiming toward the right side of the doorway. Time one jump to edgeboost.",
        [49] = "26) Mantleslide the metal scaffold as close to the green wall as possible. Look 10-20 degrees to the right and hold W+A. . . . Jump only once and immediately release W. Don't let go of space, strafe left toward the vent. It's makeable, I promise.",
        [50] = "27) Mantle the box, looking a bit right and holding W+D toward the corner of the shed. . . . Jump only once after the box breaks to hit the edgeboost.",
        [51] = "28) Mantle the box, looking a bit right and holding W+D toward the corner of the shed. . . . Jump only once, a bit later than the previous spot.",
        [52] = "29) Mantle the grey pillar south of you. Look toward the left side of the archway and superglide. . . . Look to the right and hold A, time a jump to boost and strafe back toward the vent.",
        [53] = "30) Mantleslide the wall South of you and hold W+A. Time a jump to edgeboost on the left wall.",
        [54] = "31) Mantleslide the West railing and look very slightly right. Hold W+A and time a jump to edgeboost on the left wall.",
        [55] = "32) Climb the wall South of you and mantleslide, holding w toward the left side of the doorway. . . . Snap to the right and hold A, only jump once.",
        [56] = "33) Superglide on the box, angling into the wall. Jump again to edgeboost and hold it down to mantle the building.",
        [57] = "34) Break the closest box, superglide on the second box. Hold W+D perpendicular to the wall. Jump again to edgeboost, strafe as desired.",
    };

    private static readonly EAbilitySlot[] NoCooldownSlots =
    [
        EAbilitySlot.Signature1,
        EAbilitySlot.Signature2,
        EAbilitySlot.Signature3,
        EAbilitySlot.Signature4,
        EAbilitySlot.Innate1,
        EAbilitySlot.Innate2,
        EAbilitySlot.Innate3,
        EAbilitySlot.ActiveItem1,
        EAbilitySlot.ActiveItem2,
        EAbilitySlot.ActiveItem3,
        EAbilitySlot.ActiveItem4,
        EAbilitySlot.Ability_Held,
        EAbilitySlot.Ability_ZipLine,
        EAbilitySlot.Ability_Mantle,
        EAbilitySlot.Ability_ClimbRope,
        EAbilitySlot.Ability_Jump,
        EAbilitySlot.Ability_Slide,
        EAbilitySlot.Ability_Teleport,
        EAbilitySlot.Ability_ZipLineBoost,
    ];

    // Entidades a remover no spawn (desativado)
    // public override void OnEntitySpawned(EntitySpawnedEvent args)
    // {
    //     if (_garbageEntities.Contains(args.Entity.DesignerName))
    //         args.Entity.Remove();
    //
    //     if (_garbageEntityNames.Contains(args.Entity.Name))
    //         args.Entity.Remove();
    // }
    string[] _garbageEntities =
    [
        "npc_trooper_boss",
        "npc_boss_tier2",
        "npc_boss_tier3",
        "baseanimgraph",
        "destroyable_building",
        "npc_base_defense_sentry",
        "citadel_herotest_orbspawner",
        "npc_barrack_boss",
        "info_neutral_trooper_camp",
        "npc_trooper",
        "npc_super_neutral",
        "npc_trooper_neutral",
        "npc_neutral_sinners_sacrifice",
        "trigger_item_shop",
        "citadel_trigger_shop_tunnel",
        "trigger_item_shop_safe_zone",
        "func_regenerate",
        "citadel_item_pickup_idol",
        "citadel_item_powerup_spawner",
        "citadel_punchable_powerup",
        "citadel_breakable_prop",
        "item_crate_spawn",
        "citadel_zap_trigger",
        "npc_neutral_bug",
    ];
    string[] _garbageEntityNames =
    [
        "amber_shrine_east",
        "amber_shrine_west",
        "amber_effigy_brush",
        "sapphire_shrine_east",
        "sapphire_shrine_west",
        "sapphire_effigy_brush",
        "sapphire_watcher_broadway_left",
        "sapphire_watcher_broadway_right",
        "sapphire_watcher_york_right",
        "sapphire_watcher_york_left",
        "sapphire_watcher_park_left",
        "sapphire_watcher_park_right",
        "amber_watcher_broadway_left",
        "amber_watcher_broadway_right",
        "amber_watcher_york_right",
        "amber_watcher_york_left",
        "amber_watcher_park_left",
        "amber_watcher_park_right",
    ];

    private readonly record struct TrainingGroup(string Name, string DisplayName, string LoadedMessage, int Start, int Count);
}
