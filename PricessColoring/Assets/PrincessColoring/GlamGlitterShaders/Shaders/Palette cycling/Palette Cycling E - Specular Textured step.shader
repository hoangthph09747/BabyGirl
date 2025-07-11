// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spaventacorvi/Palette Cycling/Palette Cycling E - Specular Textured step" {
    Properties {
        _Color ("Main Color", Color) = (0.7058823,0.7058823,0.7058823,1)
        xwvwvw ("Specular", Range(0, 1)) = 0
        vxwwww ("Shininess", Range(0, 1)) = 0
        _MainTex ("Base (RGB)", 2D) = "white" {}
        xvxwww ("Cycling pattern", 2D) = "white" {}
        vwwxwx ("Reflection cubemap", Cube) = "_Skybox" {}
        vwxvww ("Reflection cubemap power", Range(0, 1)) = 0.5
        xvwvwx ("Mask", 2D) = "black" {}
        vvxxww ("Palette", 2D) = "white" {}
        wwvxvx ("Palette power (1 - 2)", Range(0, 2)) = 1
        wvwwxv ("Palette cycling speed (-5 - 5)", Range(-5, 5)) = 1
        vwvxwv ("Palette stratification", Float ) = 1
        wxvwvv ("Step", Float ) = 16
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float wvwwxv;
            uniform sampler2D vvxxww; uniform float4 vvxxww_ST;
            uniform float vwvxwv;
            uniform samplerCUBE vwwxwx;
            uniform float vwxvww;
            uniform sampler2D xvwvwx; uniform float4 xvwvwx_ST;
            uniform float wwvxvx;
            uniform float4 _Color;
            uniform float xwvwvw;
            uniform float vxwwww;
            uniform sampler2D xvxwww; uniform float4 xvxwww_ST;
            uniform float wxvwvv;
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
                float gloss = vxwwww;
                float specPow = exp2( gloss * 10.0+1.0);
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 xvwvwx_var = tex2D(xvwvwx,TRANSFORM_TEX(i.uv0, xvwvwx));
                float3 specularColor = (xwvwvw*xvwvwx_var.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 indirectSpecular = (gi.indirect.specular)*specularColor;
                float3 specular = (directSpecular + indirectSpecular);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = (((texCUBE(vwwxwx,viewReflectDirection).rgb*vwxvww)+(_MainTex_var.rgb*_Color.rgb))*xvwvwx_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                float4 xvxwww_var = tex2D(xvxwww,TRANSFORM_TEX(i.uv0, xvxwww));
                float4 wvvxxw = _Time + _TimeEditor;
                float xvwxxv = (wxvwvv+1.0);
                float vwxwwv = (xvwxxv+(1.0/(xvwxxv*2.0)));
                float2 vwwvxw = ((float2(lerp(xvxwww_var.rgb,dot(xvxwww_var.rgb,float3(0.3,0.59,0.11)),1.0).r,0.0)*vwvxwv)+float2(floor((wvvxxw.g*(wvwwxv/wxvwvv)) * vwxwwv) / (vwxwwv - 1),0.0));
                float4 wvxwxw = tex2Dlod(vvxxww,float4(TRANSFORM_TEX(vwwvxw, vvxxww),0.0,0.0));
                float3 emissive = (wvxwxw.rgb*wwvxvx*(1.0 - xvwvwx_var.rgb));
                float3 finalColor = diffuse + specular + emissive;
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float wvwwxv;
            uniform sampler2D vvxxww; uniform float4 vvxxww_ST;
            uniform float vwvxwv;
            uniform samplerCUBE vwwxwx;
            uniform float vwxvww;
            uniform sampler2D xvwvwx; uniform float4 xvwvwx_ST;
            uniform float wwvxvx;
            uniform float4 _Color;
            uniform float xwvwvw;
            uniform float vxwwww;
            uniform sampler2D xvxwww; uniform float4 xvxwww_ST;
            uniform float wxvwvv;
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
                float gloss = vxwwww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 xvwvwx_var = tex2D(xvwvwx,TRANSFORM_TEX(i.uv0, xvwvwx));
                float3 specularColor = (xwvwvw*xvwvwx_var.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = (((texCUBE(vwwxwx,viewReflectDirection).rgb*vwxvww)+(_MainTex_var.rgb*_Color.rgb))*xvwvwx_var.rgb);
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
