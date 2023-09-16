using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBuildCommand
{
    private static Command[] arrCmd = new Command[(int)EMainBaseCommnad.LENGTH];

    public static void Add(EMainBaseCommnad _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EMainBaseCommnad _eCmd)
    {
        arrCmd[(int)_eCmd].Execute();
    }
}
