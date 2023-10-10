using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHUDSpawnNuclearCommand : MonoBehaviour
{
    private static Command[] arrCmd = new Command[(int)EHUDSpawnNuclearCommand.LENGTH];

    public static void Add(EHUDSpawnNuclearCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EHUDSpawnNuclearCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
