Shader "Custom/Plant_Wobble_BlinnPhong_URP_Fixed"
{
    Properties
    {
        _BaseMap ("Base Map (Albedo)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)

        _SpecColor ("Specular Color", Color) = (0.15,0.15,0.15,1)
        _Shininess ("Shininess", Range(16,256)) = 64

        _WobbleAmp ("Wobble Amplitude", Range(0,0.3)) = 0.05
        _WobbleFreq ("Wobble Frequency", Range(0,10)) = 2.0
        _WobbleSpeed ("Wobble Speed", Range(0,5)) = 1.5
        _BottomHeight ("Bottom Lock Height", Range(-1,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        LOD 300

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _SpecColor;
                float  _Shininess;
                float  _WobbleAmp;
                float  _WobbleFreq;
                float  _WobbleSpeed;
                float  _BottomHeight;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float2 uv         : TEXCOORD2;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // --- MASKA WYSOKOŚCI (dół nieruchomy, góra się rusza) ---
                float heightMask = saturate(IN.positionOS.y - _BottomHeight);

                // --- WIATR ---
                float time = _Time.y * _WobbleSpeed;
                float wave = sin(time + IN.positionOS.x * _WobbleFreq);

                // --- PRZESUNIĘCIE (tylko w bok, NIE w normal) ---
                float3 offset;
                offset.x = wave * _WobbleAmp * heightMask;
                offset.y = 0;
                offset.z = 0;

                float3 displacedOS = IN.positionOS.xyz + offset;

                VertexPositionInputs pos = GetVertexPositionInputs(displacedOS);
                OUT.positionCS = pos.positionCS;
                OUT.positionWS = pos.positionWS;

                // Normal NIE RUSZAMY – stabilne światło
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);

                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 albedo =
                    SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb
                    * _Color.rgb;

                float3 N = normalize(IN.normalWS);
                float3 V = normalize(GetWorldSpaceViewDir(IN.positionWS));

                Light mainLight = GetMainLight();
                float3 L = normalize(mainLight.direction);
                float3 lightColor = mainLight.color.rgb;

                // --- BLINN–PHONG ---
                float NdotL = max(0, dot(N, L));
                float3 diffuse = albedo * NdotL;

                float3 H = normalize(L + V);
                float spec = pow(max(0, dot(N, H)), _Shininess);
                float3 specular = _SpecColor.rgb * spec;

                float3 ambient = SampleSH(N) * albedo;

                float3 color = ambient + (diffuse + specular) * lightColor;

                return half4(color, 1);
            }
            ENDHLSL
        }
    }
}
