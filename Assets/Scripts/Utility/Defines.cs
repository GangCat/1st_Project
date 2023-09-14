using System.Collections.Generic;
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
public enum ESelectableObjectType { None = - 1, UNIT, HERO, MAIN_BASE, TURRET, BUNKER, WALL, NUCLEAR, ENEMY_UNIT, ENEMY_STRUCTURE}

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
    public int nodeIdx;

    public float moveSpeed;
    public float traceStartRange;
    public float traceEndRange;
    public float attRange;
    public float attRate;
    
    public int attDmg;

    public bool isHold;
    public bool isAttackMove;

    public IState[] arrState;
    public NodeUpdateDelegate updateNodeCallback;
}

public enum EState { NONE = -1, IDLE, MOVE, STOP, HOLD, ATTACK, LENGTH}

public enum EUnitButtonCommand { NONE = -1, CANCLE, MOVE, STOP, HOLD, PATROL, ATTACK}