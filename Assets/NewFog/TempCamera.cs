using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamera : MonoBehaviour
{
    private void Awake()
    {
        myCam = GetComponent<Camera>();
        oriCullingLayer = myCam.cullingMask;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTr.position + camOffset;
    }

    public void RenderFog()
    {
        curSize = myCam.orthographicSize;
        oriQuaternion = transform.rotation;

        transform.position = fogCamPos;
        transform.rotation = fogCamRot;
        myCam.orthographicSize = fogCamSize;
        myCam.targetTexture = fogRenderTexture;
        myCam.cullingMask = visibleLayer;
        myCam.Render();

        transform.position = playerTr.position + camOffset;
        transform.rotation = oriQuaternion;
        myCam.targetTexture = null;
        myCam.cullingMask = oriCullingLayer;
        myCam.orthographicSize = curSize;
        myCam.Render();
    }

    [SerializeField]
    private Transform playerTr = null;
    [SerializeField]
    private Vector3 camOffset = Vector3.zero;
    [SerializeField]
    private Vector3 fogCamPos = Vector3.zero;
    [SerializeField]
    private Quaternion fogCamRot;
    [SerializeField]
    private float fogCamSize = 0f;
    [SerializeField]
    private RenderTexture fogRenderTexture = null;
    [SerializeField]
    private LayerMask visibleLayer;

    private LayerMask oriCullingLayer;
    private Quaternion oriQuaternion;
    private float curSize = 0f;
    private Camera myCam = null;
}
