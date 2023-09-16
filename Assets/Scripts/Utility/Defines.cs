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
    public int attDmg;
    
}

[System.Serializable]
public enum ESelectableObjectType { None = -1, UNIT, UNIT_HERO, MAIN_BASE, TURRET, BUNKER, WALL, BARRACK, NUCLEAR, ENEMY_UNIT, ENEMY_STRUCTURE, LENGTH }
public enum EState { NONE = -1, IDLE, MOVE, STOP, HOLD, ATTACK, LENGTH }
public enum ESpawnUnitType { NONE = -1, MELEE, RANGE, ROCKET, LENGTH }


public enum EUnitButtonCommand { NONE = -1, CANCLE, MOVE, STOP, HOLD, PATROL, ATTACK, LENGTH }
public enum EMainBaseCommnad { NONE = -1, CANCLE, CONFIRM, BUILD_TURRET, BUILD_BUNKER, BUILD_WALL, BUILD_NUCLEAR, BUILD_BARRACK, DEMOLITION, UPGRADE, LENGTH }
public enum EBarrackCommand { NONE = -1, CANCLE, SPAWN_MELEE, SPAWN_RANGE, SPAWN_ROCKET, RALLYPOINT, DEMOLITION, UPGRADE, RALLYPOINT_CONFIRM, LENGTH }
public enum EUnitCommand { NONE = -1, NODE_UPDATE, LENGTH }