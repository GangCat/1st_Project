using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUnitCommandETC : MonoBehaviour
{
    private static Command[] arrCmd = new Command[(int)EUnitCommand.LENGTH];

    public static void Add(EUnitCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EUnitCommand _eCmd)
    {
        arrCmd[(int)_eCmd].Execute();
    }
}
