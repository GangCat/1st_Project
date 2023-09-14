using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBuildCommand
{
    private static Command[] arrCmd = new Command[(int)EMainStructureCommnad.LENGTH];

    public static void Add(EMainStructureCommnad _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Remove(EMainStructureCommnad _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = null;
    }

    public static void Use(EMainStructureCommnad _eCmd)
    {
        arrCmd[(int)_eCmd].Execute();
    }
}
