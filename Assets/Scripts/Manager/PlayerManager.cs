using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public void Init()
    {
        playerMove = GetComponentInChildren<PlayerMovement>();
    }

    public void MovePlayerByPicking(Vector3 _pickPos)
    {
        playerMove.MovePlayerByPicking(_pickPos);
    }



    private PlayerMovement playerMove = null;
}
