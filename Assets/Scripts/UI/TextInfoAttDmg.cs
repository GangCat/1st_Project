using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInfoAttDmg : TextBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("Attack Dmg : {0}", (float)_objects[0]);
    }
}
