using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public void Init(VoidVoidDelegate _openDoorCallback, VoidVoidDelegate _closeDoorCallback)
    {
        openDoorCallback = _openDoorCallback;
        closeDoorCallback = _closeDoorCallback;

        StartCoroutine("CheckIsUnitEnter");
    }

    private IEnumerator CheckIsUnitEnter()
    {
        Collider[] arrCol;
        bool isUnitIn = false;
        while (true)
        {
            isUnitIn = false;
            arrCol = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("SelectableObject"));

            for (int i = 0; i < arrCol.Length; ++i)
            {
                if (arrCol[i].CompareTag("FriendlyUnit"))
                {
                    isUnitIn = true;
                    break;
                }
            }

            if (isUnitIn)
            {
                if(!isOpen)
                {
                    openDoorCallback?.Invoke();
                    isOpen = true;
                }
            }
            else
            {
                if (isOpen)
                {
                    closeDoorCallback?.Invoke();
                    isOpen = false;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }


    //private void OnTriggerEnter(Collider _other)
    //{
    //    if (_other.CompareTag("FriendlyUnit"))
    //    {
    //        openDoorCallback?.Invoke();
    //    }
    //}

    //private void OnTriggerExit(Collider _other)
    //{
    //    if (_other.CompareTag("FriendlyUnit"))
    //    {
    //        closeDoorCallback?.Invoke();
    //    }
    //}

    private VoidVoidDelegate openDoorCallback = null;
    private VoidVoidDelegate closeDoorCallback = null;

    private bool isOpen = false;
}
