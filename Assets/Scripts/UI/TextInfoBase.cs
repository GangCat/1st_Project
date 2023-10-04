using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TextInfoBase : MonoBehaviour
{
    public void Init()
    {
        myText = GetComponent<TMP_Text>();
    }

    public abstract void UpdateText(params object[] _objects);

    protected TMP_Text myText = null;
}
