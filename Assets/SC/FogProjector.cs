using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FogProjector : MonoBehaviour
{
    private void Awake()
    {
        projector = GetComponent<DecalProjector>(); // ���� ��ü�� ������ �������� ������Ʈ�� ������
        projector.enabled = true; // �������� Ȱ��ȭ

        //curFogTexture = GenerateCurFogTexture(); // ���� �Ȱ� �ؽ�ó ����
        //prevFogTexture = GeneratePrevFogTexture(); // ���� �Ȱ� �ؽ�ó ����

        //// Projector materials aren't instanced, resulting in the material asset getting changed.
        //// Instance it here to prevent us from having to check in or discard these changes manually.
        //// �������� ��Ƽ������ �����Ͽ� �ν��Ͻ�ȭ�Ͽ� ��Ƽ������ ������ ����
        //projector.material = new Material(projectorMaterial);
        //// ������ �Ȱ� �ؽ�ó�� ���̴� ������Ƽ�� ����
        projector.material.SetTexture("_FogTexture", fogTexture);
        projector.material.SetTexture("_BackBufferTexture", backBufTexture);

        //StartFogUpdate(); // �Ȱ� ���� ����
    }

    [SerializeField]
    private float fogUpdateDelay; // ���� ���� �ð�
    [SerializeField]
    private Material projectorMaterial; // �Ȱ� ȿ���� ����� ��Ƽ����
    [SerializeField]
    private int textureScale; // �Ȱ� �ؽ�ó Ȯ�� ����
    [SerializeField]
    private RenderTexture fogTexture; // �Ȱ� �ؽ�ó
    [SerializeField]
    private RenderTexture backBufTexture; // �Ȱ� �ؽ�ó

    private DecalProjector projector; // �������� ������Ʈ


}
