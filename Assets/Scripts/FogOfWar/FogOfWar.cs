using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    private void Awake()
    {
        renderTexture = new RenderTexture(cameraRenderTexture);
        StartCoroutine("UpdateFogCoroutine");
    }

    private IEnumerator UpdateFogCoroutine()
    {
        while (true)
        {
            UpdateMaskMap();
            Camera.main.targetTexture = renderTexture;
            yield return null;
        }
    }

    private void UpdateMaskMap()
    {
        renderTexture = Camera.main.activeTexture;
        // ���� �ؽ��� ���� �ϳ� ����
        // ����ũ�ʿ��� ������ ���� �ִ� ��ġ ������ alpha 1�� �����ϱ�
        // ����ũ�ʿ��� ������ �־��� ��ġ�� alpha 0.5�� ĥ�ϱ�



        int idx = 0;
        while (idx < 1920 * 1080)
        {
            
            ++idx;
        }
        //renderTexture
    }





    [SerializeField]
    private Texture2D maskMap = null;
    [SerializeField]
    private Texture2D defaultMap = null;
    [SerializeField]
    private RenderTexture cameraRenderTexture = null;

    private RenderTexture renderTexture = null;
}
