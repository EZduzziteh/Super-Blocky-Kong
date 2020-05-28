Shader "Unlit/simplewave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
              
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				v.vertex.y += 0.1*sin(v.vertex.x+_Time*50);
				v.vertex.z += 0.3*sin(v.vertex.x + _Time * 50);
                o.vertex = UnityObjectToClipPos(v.vertex);
				//o.vertex.y += sin(o.vertex.x);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
              
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv+float2(_Time.x*10.0,0.0));
       
                return col;
            }
            ENDCG
        }
    }
}
