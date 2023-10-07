using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraySelectCommand
{
    private static Command[] arrCmd = new Command[(int)ESelectCommand.LENGTH];

    public static void Add(ESelectCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(ESelectCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
