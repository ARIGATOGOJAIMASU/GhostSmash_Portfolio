Shader "Custom/Outline_AlwaysVisible"
{
    Properties
    {
        _MainTex("Main Texture (RBG)", 2D) = "white" {} // Allows for a texture property
        _Color("Main color", Color) = (0.5,0.5,0.5,1)
        _OutlineColor("OutlineColor",Color) = (0,0,0,1)
        _Outline("Outline Width",Range(0.0,0.1)) = 0.009
        _SubTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Opaque" "LightMode" = "Forwardbase" }

        pass
        {
            Name "OUTLINE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float _Outline;
            uniform float4 _OutlineColor;

            struct vI
            {
              float4 mPosition : POSITION;
              float3 mNormal : NORMAL;
            };

            struct vO
            {
              float4 mPosition : POSITION;
              float4 mcolor : COLOR;
            };

            vO vert(vI i)
            {
              vO o;
              //오브젝트 공간에서 계산
             
              o.mPosition.xyz = i.mPosition.xyz + i.mNormal * _Outline;
              o.mPosition.w = i.mPosition.w;
              o.mPosition = UnityObjectToClipPos(o.mPosition);
              //투영공간에서 계산 ------------------------------------------------ iso 뷰모드에서는 라인이 안보임, perspect 뷰에서는  거리에 따라 라인두께의 변화가 심함.
              // o.mPosition=mul(UNITY_MATRIX_MVP, i.mPosition);
              // float3 norm=mul((float3x3)UNITY_MATRIX_IT_MV,i.mNormal);
              // norm.x *= UNITY_MATRIX_P[0][0];    //행렬의 y위치
              // norm.y *= UNITY_MATRIX_P[1][1]; //행렬의 y위치
              // float2 offest=TransformViewToProjection(norm.xy);     //위의 행렬위치 대신 쓸수있는것 
              // o.mPosition.xy +=offest*o.mPosition.z*_Outline; 
              o.mcolor = _OutlineColor;
              return o;
            }

            float4 frag(vO i) : COLOR
            {
                return i.mcolor;
            }

            ENDCG
            Cull Back
            ZWrite off

            //Offset 0.5, -0.5
            //ZTest Greater
        }

        pass
        {
            Name "BASE"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct vI
            {
              float4 mPosition : POSITION;
              float2 uv : TEXCOORD0;
              //float3 mNormal : NORMAL;
            };

            struct vO
            {
              float4 mPosition : POSITION;
              float2 uv : TEXCOORD0;
              //float4 mcolor : COLOR;
            };

            float4 _Color;
            sampler2D _MainTex;

            vO vert(vI i)
            {
              vO o;
              o.mPosition = UnityObjectToClipPos(i.mPosition);
              o.uv = i.uv;
              return o;
            }

            fixed4 frag(vO IN) : COLOR
            {
               float4 texColor = tex2D(_MainTex, IN.uv);
               return texColor * _Color;
            }

            ENDCG
            Cull Off
            ZWrite on

            //ZTest Less
        }

        //pass
        //{
        //    Name "BASE"
        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    #include "UnityCG.cginc"

        //    struct vI
        //    {
        //        float4 mPosition : POSITION;
        //        float2 uv : TEXCOORD0;
        //        //float3 mNormal : NORMAL;
        //    };

        //    struct vO
        //    {
        //        float4 mPosition : POSITION;
        //        float2 uv : TEXCOORD0;
        //        //float4 mcolor : COLOR;
        //    };

        //    float4 _Color;
        //    sampler2D _MainTex;

        //    vO vert(vI i)
        //    {
        //        vO o;
        //        o.mPosition = UnityObjectToClipPos(i.mPosition);
        //        o.uv = i.uv;
        //        return o;
        //    }

        //    fixed4 frag(vO IN) : COLOR
        //    {
        //       float4 texColor = tex2D(_MainTex, IN.uv);
        //       return texColor * _Color;
        //    }

        //    ENDCG
        //    Cull off
        //    ZWrite off

        //    ZTest GEqual
        //}
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}