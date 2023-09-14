using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraySpawnUnitCommand
{
    private static Command[] arrCmd = new Command[(int)EBarrackCommand.LENGTH];

    public static void Add(EBarrackCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Remove(EBarrackCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = null;
    }

    public static void Use(EBarrackCommand _eCmd)
    {
        arrCmd[(int)_eCmd].Execute();
    }
}
