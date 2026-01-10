Shader "Custom/URP_PulsingRedMetal"
{
    Properties
    {
        _BaseMap("Base Map (Texture)", 2D) = "white" {}
        _BaseColor("Base Color Tint", Color) = (1,1,1,1)

        _PulseColor("Pulse Color", Color) = (1,0,0,1)
        _PulseIntensity("Pulse Intensity", Range(0, 3)) = 1.2

        _PeriodSeconds("Period Seconds", Float) = 2.0
        _BlinkWidth("Blink Width", Range(0.01, 0.30)) = 0.06

        _MetalFresnelPower("Metal Fresnel Power", Range(0.5, 8)) = 3.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float3 viewDirWS   : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;

                float4 _PulseColor;
                float  _PulseIntensity;

                float  _PeriodSeconds;
                float  _BlinkWidth;

                float  _MetalFresnelPower;
            CBUFFER_END

            // Triangular pulse around a center (0..1), width controls how sharp
            float PulseAt(float t, float center, float width)
            {
                float d = abs(t - center);
                return saturate(1.0 - d / max(width, 1e-5));
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionHCS = posInputs.positionCS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                VertexNormalInputs nInputs = GetVertexNormalInputs(IN.normalOS);
                OUT.normalWS = nInputs.normalWS;

                float3 viewWS = GetWorldSpaceViewDir(posInputs.positionWS);
                OUT.viewDirWS = viewWS;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Base texture * tint
                half4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half3 baseCol = baseTex.rgb * _BaseColor.rgb;

                // Time normalized 0..1 over period
                float period = max(_PeriodSeconds, 0.01);
                float t = frac(_Time.y / period);

                // Two blinks every period (2 sec): near start -> blink1, then blink2
                // Centers chosen to create "doble titileo"
                float b1 = PulseAt(t, 0.12, _BlinkWidth);
                float b2 = PulseAt(t, 0.28, _BlinkWidth);
                float blink = saturate(b1 + b2);

                // Metallic-ish fresnel highlight (view dependent)
                float3 N = normalize(IN.normalWS);
                float3 V = normalize(IN.viewDirWS);
                float fresnel = pow(1.0 - saturate(dot(N, V)), _MetalFresnelPower);

                // Highlight amount
                float highlight = blink * _PulseIntensity * (0.35 + 0.65 * fresnel);

                // Mix: keep texture, add red metallic flash on top
                half3 pulseCol = _PulseColor.rgb;
                half3 finalCol = baseCol + pulseCol * highlight;

                return half4(saturate(finalCol), baseTex.a * _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
