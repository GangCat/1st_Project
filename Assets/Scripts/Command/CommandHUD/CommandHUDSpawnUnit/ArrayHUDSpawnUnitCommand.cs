using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHUDSpawnUnitCommand
{
    private static Command[] arrCmd = new Command[(int)EHUDSpawnUnitCommand.LENGTH];

    public static void Add(EHUDSpawnUnitCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EHUDSpawnUnitCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
