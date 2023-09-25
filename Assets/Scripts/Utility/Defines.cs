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
    
}

[System.Serializable]
public enum EObjectType { NONE = -1, UNIT, UNIT_HERO, MAIN_BASE, TURRET, BUNKER, WALL, BARRACK, NUCLEAR, ENEMY_UNIT, ENEMY_STRUCTURE, HBEAM, LENGTH }
public enum EState { NONE = -1, IDLE, MOVE, STOP, HOLD, ATTACK, LENGTH }
public enum ESpawnUnitType { NONE = -1, MELEE, RANGE, ROCKET, LENGTH }

public enum EUnitButtonCommand { NONE = -1, CANCLE, MOVE, STOP, HOLD, PATROL, ATTACK, LAUNCH_NUCLEAR, LENGTH }
public enum EMainBaseCommnad { NONE = -1, CANCLE, CONFIRM, BUILD_STRUCTURE, DEMOLITION, UPGRADE, LENGTH }
public enum EBarrackCommand { NONE = -1, CANCLE, SPAWN_UNIT, RALLYPOINT, DEMOLITION, UPGRADE, RALLYPOINT_CONFIRM_TR, RALLYPOINT_CONFIRM_POS, LENGTH }
public enum EUnitCommand { NONE = -1, NODE_UPDATE, LENGTH }
public enum EBunkerCommand { NONE = -1, IN_UNIT, OUT_ONE_UNIT, OUT_ALL_UNIT, EXPAND_WALL, LENGTH }
public enum EEnemyObjectCommand { NONE = -1, WAVE_ENEMY_DEAD, MAP_ENEMY_DEAD, LENGTH }
public enum EFriendlyObjectCommand { NONE = -1, DEAD, DESTROY, DESTROY_HBEAM, DEMOLISH, LENGTH }
public enum ENuclearCommand { NONE = -1, SPAWN_NUCLEAR, LAUNCH_NUCLEAR, LENGTH }