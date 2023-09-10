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

[System.Serializable]
public enum ESelectableObjectType { None = - 1, UNIT, HERO, MAIN_BASE, TURRET, BUNKER, WALL, NUCLEAR, ENEMY_UNIT, ENEMY_STRUCTURE}