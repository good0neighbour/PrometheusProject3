Shader "PrometheusProject/PlanetShader"
{
    Properties
    {
        _MainTex ("LandTexture", 2D) = "white" {}
        _Color ("LandColor", Color) = (1,1,1,1)
        _WaterMap ("OceanMap", 2D) = "white" {}
        _WaterColor ("OceanColor", Color) = (1,1,1,1)
        _WaterSpecularColour ("OceanSpecularColor", Color) = (1,1,1,1)
        _IceMap ("IceMap", 2D) = "white" {}
        _IceNormal ("IceNormalMap", 2D) = "white" {}
        _IceColor ("IceColor", Color) = (1,1,1,1)
        _IceLinear ("IceLinear", Range(0,1)) = 0.5
        _Cloud ("CloudTexture", 2D) = "white" {}
        _CloudAlph ("CloudAlph", Range(0,1)) = 0.0
        _AtmosphereColour ("AtmosphereColour", Color) = (1,1,1,1)
        _AtmpshereWidthness ("AtmpshereWidthness", Range(0,10)) = 5.0
        _PlanetRotation ("PlanetRotation", Range(-1,1)) = 0.5
        _CloudRotation ("CloudRotation", Range(-1,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf PlanetSurface//Standard noambient noshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _WaterMap;
        sampler2D _IceMap;
        sampler2D _IceNormal;
        sampler2D _Cloud;

        struct Input
        {
            fixed2 uv_MainTex;
            fixed2 uv_WaterMap;
            fixed2 uv_IceMap;
            fixed2 uv_IceNormal;
            fixed2 uv_Cloud;
        };

        fixed4 _Color;
        fixed4 _WaterColor;
        fixed4 _WaterSpecularColour;
        fixed4 _IceColor;
        fixed _IceLinear;
        fixed _CloudAlph;
        fixed4 _AtmosphereColour;
        fixed _AtmpshereWidthness;
        fixed _PlanetRotation;
        fixed _CloudRotation;

        struct PlanetSurfaceOutput
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            fixed Alpha;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout PlanetSurfaceOutput o)
        {
            // 표면 정보
            fixed4 surfaceData;

            // 구름 영역
            surfaceData.x = tex2D(
                _Cloud,
                fixed2(IN.uv_Cloud.x + _Time.x * _CloudRotation, IN.uv_Cloud.y)
            ).a * _CloudAlph;

            // 빙하 영역
            surfaceData.y = 1 - step(
                _IceColor.a,
                tex2D(
                    _WaterMap,
                    fixed2(IN.uv_IceMap.x + _Time.x * _PlanetRotation, IN.uv_IceMap.y)
                ).a * (1 - _IceLinear) + (1 - abs(IN.uv_IceMap.y - 0.5) * 2) * _IceLinear
            );

            // 해양 영역
            surfaceData.z = max(
                0,
                step(
                    1 - _WaterColor.a,
                    tex2D(
                        _WaterMap,
                        fixed2(IN.uv_WaterMap.x + _Time.x * _PlanetRotation, IN.uv_WaterMap.y)
                    ).a
                ) - surfaceData.y
            );

            // 지상 영역
            surfaceData.w = 1 - surfaceData.y - surfaceData.z;

            // 표면 색상
            fixed4 result = tex2D(
                _MainTex,
                fixed2(IN.uv_MainTex.x + _Time.x * _PlanetRotation, IN.uv_MainTex.y)
            ) * _Color;
            result.rgb = (result.rgb * surfaceData.w + _WaterColor.rgb * surfaceData.z + _IceColor.rgb * surfaceData.y) * (1 - surfaceData.x) + surfaceData.x;

            // 법선 계산
            fixed4 originalNormal;
            fixed4 iceNormal;
    
            originalNormal.xyz = o.Normal;
            iceNormal.xyz = UnpackNormal(tex2D(_IceNormal, IN.uv_IceNormal));
    
            // 값 전달
            _WaterSpecularColour.rgb = _WaterSpecularColour.rgb * saturate(surfaceData.z - surfaceData.x);
            iceNormal.w = saturate(surfaceData.y - surfaceData.x);
            o.Albedo = result.rgb;
            o.Normal = iceNormal.xyz * iceNormal.w + originalNormal.xyz * (1 - iceNormal.w);
        }

        inline fixed4 LightingPlanetSurface(PlanetSurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed tAtten)
        {
            // 표면
            fixed4 diffuse;
            diffuse.w = saturate(dot(s.Normal, lightDir));
            diffuse.rgb = diffuse.w * s.Albedo;
    
            // 해양 반사
            fixed4 specular;
            specular.rgb = pow(saturate(dot(s.Normal, normalize(lightDir + viewDir))), _WaterSpecularColour.a * 30);
    
            // 대기
            fixed4 atmosphere;
            atmosphere.rgb = pow(1 - saturate(dot(s.Normal, viewDir)), _AtmpshereWidthness) * diffuse.w * _AtmosphereColour.rgb * _AtmosphereColour.a;
    
            // 결과
            fixed4 result;
            result.rgb = diffuse.rgb + atmosphere.rgb + specular.rgb * _WaterSpecularColour.rgb;
            result.a = 1.0;
            return result;
}
        ENDCG
    }
    FallBack "Diffuse"
}
