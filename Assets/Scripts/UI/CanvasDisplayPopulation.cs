using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasDisplayPopulation : MonoBehaviour
{
    public void UpdateCurPopulation(uint _curPopulation)
    {
        textCurPopulation.text = _curPopulation.ToString();
    }

    public void UpdateCurMaxPopulation(uint _curMaxPopulation)
    {
        textCurMaxPopulation.text = _curMaxPopulation.ToString();
    }

    [SerializeField]
    private TMP_Text textCurPopulation = null;
    [SerializeField]
    private TMP_Text textCurMaxPopulation = null;
}
