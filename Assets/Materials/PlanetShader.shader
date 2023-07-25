Shader "PrometheusProject/PlanetShader"
{
    Properties
    {
        _MainTex ("LandTexture", 2D) = "white" {}
        _Color ("LandColor", Color) = (1,1,1,1)
        _WaterMap ("OceanMap", 2D) = "white" {}
        _WaterColor ("OceanColor", Color) = (1,1,1,1)
        _Glossiness ("OceanSmoothness", Range(0,1)) = 0.5
        _Metallic ("OceanMetallic", Range(0,1)) = 0.0
        _WaterCutoff ("OceanCutoff", Range(0,1)) = 0.0
        _IceMap ("IceMap", 2D) = "white" {}
        _IceNormal ("IceNormalMap", 2D) = "white" {}
        _IceColor ("IceColor", Color) = (1,1,1,1)
        _IceCutoff ("IceCutoff", Range(0,1)) = 0.0
        _Cloud ("CloudTexture", 2D) = "white" {}
        _CloudAlph ("CloudAlph", Range(0,1)) = 0.0
        _PlanetRotation ("PlanetRotation", Range(-1,1)) = 0.5
        _CloudRotation ("CloudRotation", Range(-1,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard noambient noshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _WaterMap;
        sampler2D _IceMap;
        sampler2D _IceNormal;
        sampler2D _Cloud;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_WaterMap;
            float2 uv_IceMap;
            float2 uv_IceNormal;
            float2 uv_Cloud;
        };

        fixed4 _Color;
        fixed4 _WaterColor;
        half _Glossiness;
        half _Metallic;
        half _WaterCutoff;
        fixed4 _IceColor;
        half _IceCutoff;
        half _CloudAlph;
        half _PlanetRotation;
        half _CloudRotation;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // 표면 정보
            fixed4 surfaceData;

            // 구름 영역
            surfaceData.x = tex2D(
                _Cloud,
                float2(IN.uv_Cloud.x + _Time.x * _CloudRotation, IN.uv_Cloud.y)
            ).a * _CloudAlph;

            // 빙하 영역
            surfaceData.y = 1 - step(
                _IceCutoff,
                tex2D(
                    _IceMap,
                    float2(IN.uv_IceMap.x + _Time.x * _PlanetRotation, IN.uv_IceMap.y)
                ).a
            );

            // 해양 영역
            surfaceData.z = max(
                0,
                step(
                    _WaterCutoff,
                    tex2D(
                        _WaterMap,
                        float2(IN.uv_WaterMap.x + _Time.x * _PlanetRotation, IN.uv_WaterMap.y)
                    ).a
                ) - surfaceData.y
            );

            // 지상 영역
            surfaceData.w = 1 - surfaceData.y - surfaceData.z;

            // 표면 색상
            fixed4 result = tex2D(
                _MainTex,
                float2(IN.uv_MainTex.x + _Time.x * _PlanetRotation, IN.uv_MainTex.y)
            ) * _Color;
            result.rgb = (result.rgb * surfaceData.w + _WaterColor.rgb * surfaceData.z + _IceColor.rgb * surfaceData.y) * (1 - surfaceData.x) + surfaceData.x;

            // 값 전달
            o.Albedo = result.rgb;
            fixed nonCloudArea = max(0, surfaceData.z - surfaceData.x);
            o.Metallic = _Metallic * nonCloudArea;
            o.Smoothness = _Glossiness * nonCloudArea;
            nonCloudArea = max(0, surfaceData.y - surfaceData.x * 0.5);
            o.Normal = UnpackNormal(tex2D(_IceNormal, IN.uv_IceNormal) * nonCloudArea + 0.5 * (1 - nonCloudArea));
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
