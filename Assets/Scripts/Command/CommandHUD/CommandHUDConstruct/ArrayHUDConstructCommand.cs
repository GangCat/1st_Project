using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHUDConstructCommand : MonoBehaviour
{
    private static Command[] arrCmd = new Command[(int)EHUDConstructCommand.LENGTH];

    public static void Add(EHUDConstructCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EHUDConstructCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
