using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureMainBase : Structure
{
    protected override void UpgradeComplete()
    {
        Debug.Log("UpgradeCompleteMainBase");
    }
}
