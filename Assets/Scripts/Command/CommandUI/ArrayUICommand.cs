using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUICommand 
{
    private static Command[] arrCmd = new Command[(int)EUICommand.LENGTH];

    public static void Add(EUICommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EUICommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
