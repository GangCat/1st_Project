using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonCancle : Command
{
    public CommandButtonCancle() { }

    public override void Execute()
    {
        Debug.Log("Cancle");
    }
}
