// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spaventacorvi/Holographic/Holo A - Rough" {
    Properties {
        _Color ("Color", Color) = (0.7058823,0.7058823,0.7058823,1)
        xxwxww ("Gloss (0-1)", Range(0, 1)) = 0.2
        vxvvvw ("Mask", 2D) = "white" {}
        vxxxwx ("Foreground transp (White)", Range(0, 1)) = 0
        vvxxww ("Rainbow color", Color) = (1,1,1,1)
        vwwxwx ("Raimbow", 2D) = "white" {}
        wvwwxv ("Rainbow scale", Float ) = 1
        wvvxxw ("Rainbow shift (-1 - 1)", Range(-1, 1)) = 0
        vwxvww ("Rainbow power (0-2)", Range(0, 2)) = 1
        vwwvxw ("Silver power (0 - 1)", Range(0, 1)) = 0.2
        xvwvwx ("Holo map", 2D) = "bump" {}
        xvxwww ("Holo depth", Range(0, 1)) = 0.2
        wwvxvx ("Holo noise (0 - 0.5)", Range(0, 0.5)) = 0.2
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float xxwxww;
            uniform sampler2D vwwxwx; uniform float4 vwwxwx_ST;
            uniform float4 _Color;
            uniform float4 vvxxww;
            uniform float vwxvww;
            uniform sampler2D vxvvvw; uniform float4 vxvvvw_ST;
            uniform float vxxxwx;
            uniform sampler2D xvwvwx; uniform float4 xvwvwx_ST;
            uniform float wwvxvx;
            uniform float xvxwww;
            uniform float wvwwxv;
            uniform float vwwvxw;
            uniform float wvvxxw;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = xxwxww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 vxvvvw_var = tex2D(vxvvvw,TRANSFORM_TEX(i.uv0, vxvvvw));
                float3 wxxxww = (vxvvvw_var.rgb*vxxxwx);
                float3 xvwvwx_var = UnpackNormal(tex2Dlod(xvwvwx,float4(TRANSFORM_TEX(i.uv0, xvwvwx),0.0,0.0)));
                float2 vwvxwv = (wvwwxv*(lerp(float3(0,0,1),xvwvwx_var.rgb,xvxwww).rg+wvvxxw+mul( unity_WorldToObject, float4(((viewReflectDirection*viewDirection)+lightDirection),0) ).xyz.rgb.rg));
                float4 vwwxwx_var = tex2Dlod(vwwxwx,float4(TRANSFORM_TEX(vwvxwv, vwwxwx),0.0,0.0));
                float2 wvxwxw_skew = i.uv0 + 0.2127+i.uv0.x*0.3713*i.uv0.y;
                float2 wvxwxw_rnd = 4.789*sin(489.123*(wvxwxw_skew));
                float wvxwxw = frac(wvxwxw_rnd.x*wvxwxw_rnd.y*(1+wvxwxw_skew.x));
                float3 specularColor = ((1.0 - wxxxww)*((vvxxww.rgb*(vwwxwx_var.rgb*vwxvww))+(wvxwxw*wwvxvx)));
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
                float3 diffuseColor = lerp(lerp((vwwvxw*vwwxwx_var.rgb),dot((vwwvxw*vwwxwx_var.rgb),float3(0.3,0.59,0.11)),1.0),_Color.rgb,wxxxww);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float xxwxww;
            uniform sampler2D vwwxwx; uniform float4 vwwxwx_ST;
            uniform float4 _Color;
            uniform float4 vvxxww;
            uniform float vwxvww;
            uniform sampler2D vxvvvw; uniform float4 vxvvvw_ST;
            uniform float vxxxwx;
            uniform sampler2D xvwvwx; uniform float4 xvwvwx_ST;
            uniform float wwvxvx;
            uniform float xvxwww;
            uniform float wvwwxv;
            uniform float vwwvxw;
            uniform float wvvxxw;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = xxwxww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 vxvvvw_var = tex2D(vxvvvw,TRANSFORM_TEX(i.uv0, vxvvvw));
                float3 wxxxww = (vxvvvw_var.rgb*vxxxwx);
                float3 xvwvwx_var = UnpackNormal(tex2Dlod(xvwvwx,float4(TRANSFORM_TEX(i.uv0, xvwvwx),0.0,0.0)));
                float2 vwvxwv = (wvwwxv*(lerp(float3(0,0,1),xvwvwx_var.rgb,xvxwww).rg+wvvxxw+mul( unity_WorldToObject, float4(((viewReflectDirection*viewDirection)+lightDirection),0) ).xyz.rgb.rg));
                float4 vwwxwx_var = tex2Dlod(vwwxwx,float4(TRANSFORM_TEX(vwvxwv, vwwxwx),0.0,0.0));
                float2 wvxwxw_skew = i.uv0 + 0.2127+i.uv0.x*0.3713*i.uv0.y;
                float2 wvxwxw_rnd = 4.789*sin(489.123*(wvxwxw_skew));
                float wvxwxw = frac(wvxwxw_rnd.x*wvxwxw_rnd.y*(1+wvxwxw_skew.x));
                float3 specularColor = ((1.0 - wxxxww)*((vvxxww.rgb*(vwwxwx_var.rgb*vwxvww))+(wvxwxw*wwvxvx)));
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = lerp(lerp((vwwvxw*vwwxwx_var.rgb),dot((vwwvxw*vwwxwx_var.rgb),float3(0.3,0.59,0.11)),1.0),_Color.rgb,wxxxww);
                float3 diffuse = directDiffuse * diffuseColor;
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
