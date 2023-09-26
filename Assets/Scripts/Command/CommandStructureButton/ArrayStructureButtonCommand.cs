using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayStructureButtonCommand : MonoBehaviour
{
    private static Command[] arrCmd = new Command[(int)EStructureButtonCommand.LENGTH];

    public static void Add(EStructureButtonCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EStructureButtonCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}
