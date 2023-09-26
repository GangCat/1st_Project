using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBarrackCommand
{
    private static Command[] arrCmd = new Command[(int)EBarrackCommand.LENGTH];

    public static void Add(EBarrackCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EBarrackCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
