using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayFriendlyObjectCommand
{
    private static Command[] arrCmd = new Command[(int)EFriendlyObjectCommand.LENGTH];

    public static void Add(EFriendlyObjectCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EFriendlyObjectCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }

    //public static void Use(EFriendlyObjectCommand _eCmd, GameObject _removeGo, ESpawnUnitType _type, int _barrackIdx)
    //{
    //    arrCmd[(int)_eCmd].Execute(_removeGo, _type, _barrackIdx);
    //}
}
