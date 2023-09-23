using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRuin : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("AutoDestroy");
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }
}
