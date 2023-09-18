using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListBunkerCommand
{
    private static List<Command> listCmd = new List<Command>();

    public static void Add(EBunkerCommand _eCmd, Command _cmd, int _bunkerIdx)
    {
        //listCmd[(int)_eCmd + _bunkerIdx] = _cmd;
        listCmd.Add(_cmd);
    }

    public static void Use(EBunkerCommand _eCmd, int _bunkerIdx)
    {
        listCmd[(int)_eCmd + (_bunkerIdx * (int)EBunkerCommand.LENGTH)].Execute();
    }
}
