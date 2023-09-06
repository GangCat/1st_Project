using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Obstacle : MonoBehaviour
{
    private void OnEnable()
    {
        grid.UpdateNodeWalkable(grid.NodeFromWorldPoint(transform.position), false);
    }

    private void OnDisable()
    {
        grid.UpdateNodeWalkable(grid.NodeFromWorldPoint(transform.position), true);
    }

    [Header("-Test")]
    [SerializeField]
    private PF_Grid grid = null;

}
