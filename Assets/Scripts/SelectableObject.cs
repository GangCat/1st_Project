using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public ESelectableObjectType ObjectType => objectType;


    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
}
