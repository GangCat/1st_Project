using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListBuildCommand
{
    public static List<Command> listBuildCmd = new List<Command>();

    public static void Add(Command _cmd)
    {
        listBuildCmd.Add(_cmd);
    }

    public static void Remove(Command _cmd)
    {
        listBuildCmd.Remove(_cmd);
    }

    public static void Use(int _idx)
    {
        listBuildCmd[_idx].Execute();
    }
}
