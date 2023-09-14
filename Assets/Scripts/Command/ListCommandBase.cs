using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCommandBase
{
    protected static List<Command> listCmd = new List<Command>();

    public static void Add(Command _cmd)
    {
        listCmd.Add(_cmd);
    }

    public static void Remove(Command _cmd)
    {
        listCmd.Remove(_cmd);
    }

    public static void Use(int _idx)
    {
        listCmd[_idx].Execute();
    }
}
