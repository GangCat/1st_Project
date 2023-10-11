Shader "Custom/FogProjection"
{
    Properties
    {
        _PrevTexture("Previous Texture", 2D) = "white" {}  // 이전 텍스처를 저장할 속성 정의
        _CurrTexture("Current Texture", 2D) = "white" {}  // 현재 텍스처를 저장할 속성 정의
        _MapTexture("Map Texture", 2D) = "white" {} // 맵 텍스쳐를 저장할 속성 정의
        _Color("Color", Color) = (0, 0, 0, 0)  // 텍스처에 적용할 색상 속성 정의
    }

        SubShader
        {
            Tags { "Queue" = "Transparent+100" }  // 그릴 때의 렌더링 순서 지정

            Pass
            {
                ZWrite Off  // Z 버퍼에 쓰기를 끔 (투명 물체를 위해 필요)
                Blend SrcAlpha OneMinusSrcAlpha  // 투명 물체의 블렌딩 설정
                ZTest Equal  // Z 테스트를 Equal로 지정하여 깊이값이 같을 때만 그림

                CGPROGRAM  // CG 프로그램 시작

                #pragma vertex vert  // 정점 셰이더 함수를 vert로 정의
                #pragma fragment frag  // 픽셀 셰이더 함수를 frag로 정의

                #include "UnityCG.cginc"  // UnityCG 쉐이더 라이브러리 포함

                struct appdata
                {
                    float4 vertex : POSITION;  // 정점의 위치 정보
                    float4 uv : TEXCOORD0;  // 텍스처 좌표 정보
                };

                struct v2f
                {
                    float4 uv : TEXCOORD0;  // 텍스처 좌표를 전달할 변수
                    float4 vertex : SV_POSITION;  // 정점의 스크린 좌표를 전달할 변수
                };

                float4x4 unity_Projector;  // 프로젝터 행렬
                sampler2D _CurrTexture;  // 현재 텍스처를 담을 변수
                sampler2D _PrevTexture;  // 이전 텍스처를 담을 변수
                sampler2D _MapTexture;
                fixed4 _Color;  // 색상 정보를 담을 변수

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);  // 정점 좌표를 클립 공간으로 변환
                    o.uv = mul(unity_Projector, v.vertex);  // 프로젝터 행렬을 사용하여 텍스처 좌표 계산
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target  // 픽셀 셰이더 함수
                {
                    float aPrev = tex2Dproj(_PrevTexture, i.uv).a;  // 이전 텍스처의 알파값을 가져옴
                    float aCurr = tex2Dproj(_CurrTexture, i.uv).a;  // 현재 텍스처의 알파값을 가져옴
                    
                    fixed a = (aPrev + aCurr) / 2;

                    // _Color는 0,0,0,1임

                    // 픽셀의 영역의 a가 0보다 크면 일단 뭐가 보여야함.
                    // 근데 한 0.7? 이상은 실제 맵이 보여야 함.
                    // 그리고 0보다 큰 경우 맵 텍스쳐가 보여야함.
                    // 이 때 맵 텍스쳐의 알파값은 거의 0에 가까워야함.

                    if (a > 0.7) {
                        _Color.a = max(0, _Color.a - a);
                        return _Color;  // 최종 색상 반환
                    }


                    // 일단 이게 맞음.
                    // 근데 맵이 어두워야 하는데 너무 밝음.
                    // 하지만 mapColor의 a를 조절하면 뒤의 맵이 보여서 a는 항상 1이어야함.
                    // 대신 맵은 a가 낮은 만큼 어두워져야 함.
                    // 그럼 rgb를 모두 낮추면 되나?

                    fixed4 mapColor = tex2D(_MapTexture, i.uv);
                    mapColor.r *= a;
                    mapColor.g *= a;
                    mapColor.b *= a;
                    return mapColor;

                    // mapColor에다가 곧바로 a를 곱하면 alpha도 떨어져서 맵이 보임.
                    // 그래서 각 항목에만 곱하고 그 값을 반환하게 함.
                    

                    //fixed4 mapColor = tex2D(_MapTexture, i.uv);
                    //mapColor.a = max(0, _Color.a - a);
                    //return mapColor;

                    // 색상의 알파 값을 계산하고 음수가 되지 않도록 보정

                }
                ENDCG  // CG 프로그램 종료
            }
        }
}
