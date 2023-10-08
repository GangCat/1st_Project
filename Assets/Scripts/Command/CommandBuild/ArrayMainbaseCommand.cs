using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayMainbaseCommand
{
    private static Command[] arrCmd = new Command[(int)EMainbaseCommnad.LENGTH];

    public static void Add(EMainbaseCommnad _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EMainbaseCommnad _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
