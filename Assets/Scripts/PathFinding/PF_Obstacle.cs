using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Obstacle : MonoBehaviour
{
    private void OnEnable()
    {
        grid.UpdateNodeWalkable(grid.GetNodeFromWorldPoint(transform.position), false);
    }

    private void OnDisable()
    {
        grid.UpdateNodeWalkable(grid.GetNodeFromWorldPoint(transform.position), true);
    }

    [Header("-Test")]
    [SerializeField]
    private PF_Grid grid = null;

}
