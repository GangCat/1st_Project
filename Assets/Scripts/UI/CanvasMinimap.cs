using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMinimap : MonoBehaviour
{
    public void UpdateMinimap(EObjectType _type, PF_Node _prevNode, PF_Node _curNode)
    {
        Debug.Log(_type);
    }



    [SerializeField]
    private Image imageMinimap = null;
}
