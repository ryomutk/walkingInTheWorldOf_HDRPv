Shader "Suz_Siki_Shader/NewSerface"
{
    Properties
    {
        _Color ("Color",Color) = (1,1,1,1)
        _MainTex ("Albedo(RGB)",2D) = "white"{}
        _Speed ("Speed",Range(0,10)) = 2
        _Multiplier ("Multiplier",float) = 4


        _Emission ("Emission",Color) = (1,1,1,1)
        _Occlusion ("Occlusion",Range(0,1)) = 1
        _Smoothness ("Smoothness",Range(0,1)) = 0
        _Metallic ("Metallic",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed _Speed;
        fixed4 _Color;
        fixed _Multiplier;

        half4 _Emission;
        fixed _Occlusion;
        fixed _Smoothness;
        fixed _Metallic;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed2 scrolledUV = IN.uv_MainTex;
            fixed scrval = _Speed * _SinTime;

            scrolledUV.x /= _Multiplier;
            scrolledUV += fixed2(scrval,0);

            half4 c = tex2D(_MainTex,scrolledUV);
            o.Albedo = c.rgb * _Color;
            o.Alpha = c.a;

            o.Emission = _Emission.rgb;            
            o.Occlusion = _Occlusion;
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
