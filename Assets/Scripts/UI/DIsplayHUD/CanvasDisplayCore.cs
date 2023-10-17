using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasDisplayCore : MonoBehaviour
{
    public void UpdateCore(uint _curCore)
    {
        textCore.text = string.Format("{0:N0}", _curCore);
    }

    [SerializeField]
    private TMP_Text textCore = null;
}
