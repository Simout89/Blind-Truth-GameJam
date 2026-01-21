Shader "Custom/ProximityBarrier"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UseAlpha ("Use Texture Alpha", Float) = 1
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Range(0,100)) = 2
        _VisibleDistance ("Visible Distance", Range(0.1,100)) = 3
        _ScrollSpeedX ("Scroll Speed X", Range(-5,5)) = 0
        _ScrollSpeedY ("Scroll Speed Y", Range(-5,5)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UseAlpha;
            float4 _EmissionColor;
            float _EmissionStrength;
            float _VisibleDistance;
            float _ScrollSpeedX;
            float _ScrollSpeedY;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 camPos = _WorldSpaceCameraPos;
                float dist = distance(i.worldPos, camPos);

                // Анимация UV по времени
                float2 scrolledUV = i.uv + float2(_ScrollSpeedX, _ScrollSpeedY) * _Time.y;

                float4 texColor = tex2D(_MainTex, scrolledUV);

                // Прозрачность по расстоянию
                float visibility = saturate(1 - (dist / _VisibleDistance));

                // Учитываем альфа-канал
                float alpha = _UseAlpha > 0.5 ? texColor.a : 1.0;

                float3 emission = _EmissionColor.rgb * _EmissionStrength;

                float finalAlpha = visibility * alpha;

                return float4(texColor.rgb * visibility + emission * visibility, finalAlpha);
            }
            ENDHLSL
        }
    }
}
