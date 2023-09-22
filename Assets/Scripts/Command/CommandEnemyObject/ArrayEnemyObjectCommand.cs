using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayEnemyObjectCommand
{
    private static Command[] arrCmd = new Command[(int)EEnemyObjectCommand.LENGTH];

    public static void Add(EEnemyObjectCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EEnemyObjectCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
