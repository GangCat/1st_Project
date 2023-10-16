using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayMenuCommand
{
    private static Command[] arrCmd = new Command[(int)EMenuCommand.LENGTH];

    public static void Add(EMenuCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EMenuCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
