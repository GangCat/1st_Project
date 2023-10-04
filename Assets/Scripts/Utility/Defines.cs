using UnityEngine;

public delegate void VoidVoidDelegate();
public delegate void VoidBoolDelegate(bool _bool);
public delegate void VoidIntDelegate(int _int);
public delegate void VoidFloatDelegate(float _float);
public delegate void VoidStringDelegate(string _str);
public delegate void VoidVec3Delegate(Vector3 _vec3);
public delegate void VoidVec2Delegate(Vector2 _vec2);
public delegate void VoidTemplateDelegate<T>(T _list);
public delegate void VoidTransformDelegate(Transform _tr);
public delegate void NodeUpdateDelegate(Vector3 _pos, int _nodeIdx);
public delegate void VoidNuclearDelegate(StructureNuclear _nuclear);


[System.Serializable]
public struct SUnitState
{
    [HideInInspector]
    public Transform myTr;
    [HideInInspector]
    public Transform targetTr;
    [HideInInspector]
    public Vector3 targetPos;
    [HideInInspector]
    public bool isWaitForNewPath;

    public float moveSpeed;
    public float attRate;
    public float attDmg;

    public GameObject TurretMissile;
    public Transform missileSpawnTr;
}

public struct SUnitInfo
{
    public EObjectType objectType;
    public int maxHp;
    public float attDmg;
    public float attRate;
    public float attRange;
}

public struct SFriendlyUnitInfo
{
    public EUnitType unitType;
    public float curHpPercent;
}

[System.Serializable]
public enum EObjectType { NONE = -1, UNIT, UNIT_HERO, MAIN_BASE, TURRET, BUNKER, WALL, BARRACK, NUCLEAR, ENEMY_UNIT, ENEMY_STRUCTURE, HBEAM, PROCESSING_UPGRADE_STRUCTURE, LENGTH }
public enum EState { NONE = -1, IDLE, MOVE, STOP, HOLD, ATTACK, TURRET_ATTACK, LENGTH }
public enum EUnitType { NONE = -1, MELEE, RANGED, ROCKET, HERO, LENGTH }
public enum EUnitUpgradeType { NONE = -1, RANGED_UNIT_DMG, RANGED_UNIT_HP, MELEE_UNIT_DMG, MELEE_UNIT_HP, LENGTH }

public enum EUnitButtonCommand { NONE = -1, CANCLE, MOVE, STOP, HOLD, PATROL, ATTACK, LAUNCH_NUCLEAR, LENGTH }
public enum EMainBaseCommnad { NONE = -1, CANCLE, CONFIRM, BUILD_STRUCTURE, DEMOLITION, UPGRADE, BUILD_COMPLETE, LENGTH }
public enum EBarrackCommand { NONE = -1, CANCLE, SPAWN_UNIT, RALLYPOINT, DEMOLITION, UPGRADE, RALLYPOINT_CONFIRM_TR, RALLYPOINT_CONFIRM_POS, UPGRADE_UNIT, LENGTH }
public enum EUnitCommand { NONE = -1, NODE_UPDATE, LENGTH }
public enum EBunkerCommand { NONE = -1, IN_UNIT, OUT_ONE_UNIT, OUT_ALL_UNIT, EXPAND_WALL, LENGTH }
public enum EEnemyObjectCommand { NONE = -1, WAVE_ENEMY_DEAD, MAP_ENEMY_DEAD, LENGTH }
public enum EFriendlyObjectCommand { NONE = -1, DEAD, DEAD_HERO, DESTROY, DESTROY_HBEAM, COMPLETE_UPGRADE_RANGED_UNIT_DMG, COMPLETE_UPGRADE_RANGED_UNIT_HP, COMPLETE_UPGRADE_MELEE_UNIT_DMG, COMPLETE_UPGRADE_MELEE_UNIT_HP, LENGTH }
public enum ENuclearCommand { NONE = -1, SPAWN_NUCLEAR, LAUNCH_NUCLEAR, LENGTH }
public enum EStructureButtonCommand { NONE = -1, DEMOLISH, UPGRADE, LENGTH }
public enum EUpgradeCommand { NONE = -1, HP, ATTACK_DAMAGE, ATTACK_RANGE, BUFF_RATIO, ENERGY_INCOME, LENGTH }
public enum ECurrencyCommand { NONE = -1, UPDATE_CORE_HUD, UPDATE_ENERGY_HUD, COLLECT_CORE, UPGRADE_ENERGY_SUPPLY, UPGRADE_ENERGY_SUPPLY_COMPLETE, LENGTH }
public enum EPopulationCommand { NONE = -1, UPDATE_CURRENT_POPULATION_HUD, UPDATE_CURRENT_MAX_POPULATION_HUD, INCREASE_CUR_POPULATION, UPGRADE_MAX_POPULATION, UPGRADE_POPULATION_COMPLETE, LENGTH }
public enum EHUDCommand { NONE = -1, ADD_STRUCTURE_NODE_TO_MINIMAP, REMOVE_STRUCTURE_NODE_FROM_MINIMAP, INIT_WAVE_TIME, UPDATE_WAVE_TIME, INIT_DISPLAY_GROUP_INFO, INIT_DISPLAY_SINGLE_INFO, DISPLAY_GROUP_INFO, DISPLAY_SINGLE_INFO, HIDE_UNIT_INFO, LENGTH }

public enum EPublisherType { NONE = -1, SELECTABLE_MANAGER, ENERGY_UPDATE, CORE_UPDATE, POPULATION_MANAGER, LENGTH }
public enum EMessageType { NONE = -1, UPGRADE_RANGED_DMG, UPGRADE_RANGED_HP, UPGRADE_MELEE_DMG, UPGRADE_MELEE_HP, START_SPAWN, STOP_SPAWN, LENGTH }

public enum EUpgradeETCType { NONE = -1, CURRENT_MAX_POPULATION, ENERGY_SUPPLY, LENGTH }