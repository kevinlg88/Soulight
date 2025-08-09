Shader "Toon/Hair_URP" {
    Properties {
        _Color ("Color(RGBA)", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _SphereAddTex ("Texture(Sphere)", 2D) = "black" {}
        _Shininess ("Shininess", Float) = 1.0
        _ShadowThreshold ("Shadow Threshold(0.0:1.0)", Float) = 0.5
        _ShadowColor ("Shadow Color(RGBA)", Color) = (0,0,0,0.5)
        _ShadowSharpness ("Shadow Sharpness", Float) = 100
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            TEXTURE2D(_SphereAddTex);
            SAMPLER(sampler_SphereAddTex);

            float4 _Color;
            float _Shininess;
            float _ShadowThreshold;
            float4 _ShadowColor;
            float _ShadowSharpness;

            struct Attributes {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert (Attributes IN) {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.position = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.worldNormal = normalize(TransformObjectToWorldDir(IN.normal.xyz));
                OUT.worldPos = TransformObjectToWorld(IN.vertex.xyz);
                return OUT;
            }

            half4 LightingToonHair(float3 lightDir, float3 normal) {
                float lightStrength = saturate(dot(normalize(normal), normalize(lightDir)) * 0.5 + 0.5);
                half shadowRate = abs(min(lightStrength, _ShadowThreshold) - _ShadowThreshold)
                                  * _ShadowSharpness * _ShadowColor.a;
                float4 toon = lerp(float4(1,1,1,1), _ShadowColor, shadowRate);
                return toon;
            }

            half4 frag(Varyings IN) : SV_Target {
                float4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

                float2 sphereUv = IN.worldNormal.xz * 0.5 + 0.5;
                float4 sphereAdd = SAMPLE_TEXTURE2D(_SphereAddTex, sampler_SphereAddTex, sphereUv);

                float3 lightDir = normalize(float3(0.5, 1.0, 0.3));
                float4 toon = LightingToonHair(lightDir, IN.worldNormal);

                float4 color = baseColor * toon;
                color += sphereAdd * 0.2 * step(0.0, IN.worldNormal.y);
                color.a = baseColor.a;
                return color;
            }
            ENDHLSL
        }
    }
    Fallback "Universal Forward"
}