using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FogProjector : MonoBehaviour
{
    private void Awake()
    {
        projector = GetComponent<DecalProjector>(); // 현재 객체에 부착된 프로젝터 컴포넌트를 가져옴
        projector.enabled = true; // 프로젝터 활성화

        //curFogTexture = GenerateCurFogTexture(); // 현재 안개 텍스처 생성
        //prevFogTexture = GeneratePrevFogTexture(); // 이전 안개 텍스처 생성

        //// Projector materials aren't instanced, resulting in the material asset getting changed.
        //// Instance it here to prevent us from having to check in or discard these changes manually.
        //// 프로젝터 머티리얼을 복제하여 인스턴스화하여 머티리얼의 변경을 추적
        //projector.material = new Material(projectorMaterial);
        //// 생성된 안개 텍스처를 쉐이더 프로퍼티에 설정
        projector.material.SetTexture("_FogTexture", fogTexture);
        projector.material.SetTexture("_BackBufferTexture", backBufTexture);

        //StartFogUpdate(); // 안개 갱신 시작
    }

    [SerializeField]
    private float fogUpdateDelay; // 블렌딩 갱신 시간
    [SerializeField]
    private Material projectorMaterial; // 안개 효과에 사용할 머티리얼
    [SerializeField]
    private int textureScale; // 안개 텍스처 확대 비율
    [SerializeField]
    private RenderTexture fogTexture; // 안개 텍스처
    [SerializeField]
    private RenderTexture backBufTexture; // 안개 텍스처

    private DecalProjector projector; // 프로젝터 컴포넌트


}
