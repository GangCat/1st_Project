Shader "YJJ/PlayerShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _OutlineThickness("OutlineThickness", Range(0,1)) = 0.001
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        cull front

        LOD 200

        CGPROGRAM

        #pragma surface surf Nolight vertex:vert noshadow noambient

        fixed _OutlineThickness;

        void vert(inout appdata_full v){
        v.vertex.xyz += v.normal.xyz * _OutlineThickness;
        }

        struct Input { float4 color:COLOR; };

        // 실제로 컬러를 처리할 것이 아니라 Input IN이 필수라서 그냥 만들어주기만 함.
        void surf (Input IN, inout SurfaceOutput o) {}

        // 우리가 커스텀 라이팅을 계산하는 함수지만 실제로는 그냥 검은색으로 그리기 위해서 함수를 호출함
		float4 LightingNolight (SurfaceOutput s, float3 lightDir, float atten) {
			return float4(1, 0, 0, 1);
		}
		ENDCG


        cull back

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
