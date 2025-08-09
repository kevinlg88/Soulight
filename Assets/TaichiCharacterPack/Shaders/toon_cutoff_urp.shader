Shader "Toon/Cutoff_URP" {
    Properties {
        _Color ("Color(RGBA)", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Shininess ("Shininess(0.0:)", Float) = 1.0
        _ShadowThreshold ("Shadow Threshold(0.0:1.0)", Float) = 0.5
        _ShadowColor ("Shadow Color(RGBA)", Color) = (0,0,0,0.5)
        _ShadowSharpness ("Shadow Sharpness(0.0:)", Float) = 100
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.9
    }
    SubShader {
        Tags { "RenderType"="TransparentCutout" "Queue"="Transparent" "IgnoreProjector"="False" }
        
        Pass {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }
            Cull Off
            ZWrite On
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            
            float4 _Color;
            float _Shininess;
            float _ShadowThreshold;
            float4 _ShadowColor;
            float _ShadowSharpness;
            float _Cutoff;
            
            struct Attributes {
                float3 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };
            
            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            Varyings vert(Attributes IN) {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.normalWS = normalize(TransformObjectToWorldDir(IN.normalOS));
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float4 sampled = texColor * _Color;
                clip(sampled.a - _Cutoff);
                
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float lightStrength = dot(IN.normalWS, lightDir) * 0.5 + 0.5;
                float shadowRate = abs(max(-1.0, (min(lightStrength, _ShadowThreshold) - _ShadowThreshold) * _ShadowSharpness)) * _ShadowColor.a;
                float4 toon = lerp(float4(1,1,1,1), _ShadowColor, shadowRate);
                
                float4 lightColor = _MainLightColor;
                float4 computed = _Color * lightColor * sampled * (2.0 * _Shininess);
                computed *= toon;
                computed.a = sampled.a;
                return computed;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}