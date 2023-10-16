using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRessurectionTimer : TextBase
{
    public override void UpdateText(params object[] _objects)
    {
        myText.text = string.Format("{0:D2}:{1:D2}", (int)_objects[0] / 60, (int)_objects[0] % 60);
    }
}
