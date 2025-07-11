// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spaventacorvi/Glass/Glass A - Bumped specular refraction" {
    Properties {
        _Specular010 ("Specular (0 - 10)", Range(0, 1)) = 0.5
        _Gloss01 ("Gloss (0 - 1)", Range(0, 1)) = 0.8
        _Color ("Color", Color) = (0.7058823,0.7058823,0.7058823,1)
        _Texture ("Texture", 2D) = "black" {}
        _Texturetransparency01 ("Texture transparency (0 - 1)", Range(0, 1)) = 1
        _Cubemap ("Cubemap", Cube) = "_Skybox" {}
        _Cubemapblur05 ("Cubemap blur (0 - 5)", Range(0, 5)) = 0
        _Cubemapopaque01 ("Cubemap opaque (0 - 1)", Range(0, 1)) = 0.5
        _Cubemaptransparent01 ("Cubemap transparent (0 - 1)", Range(0, 2)) = 1
        _Refractionnormalmap ("Refraction normal map", 2D) = "bump" {}
        _Refractionintensity ("Refraction intensity", Range(0, 1)) = 0.5
        _Glasscolor ("Glass color", Color) = (1,0,0,1)
        _Glassopacity01 ("Glass opacity (0 - 1)", Range(0, 1)) = 0.3155591
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ "Refraction" }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
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
            uniform sampler2D Refraction;
            uniform sampler2D _Refractionnormalmap; uniform float4 _Refractionnormalmap_ST;
            uniform float _Specular010;
            uniform float _Gloss01;
            uniform float _Glassopacity01;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform samplerCUBE _Cubemap;
            uniform float _Cubemapopaque01;
            uniform float _Cubemaptransparent01;
            uniform float _Texturetransparency01;
            uniform float _Cubemapblur05;
            uniform float4 _Glasscolor;
            uniform float4 _Color;
            uniform float _Refractionintensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Refractionnormalmap_var = UnpackNormal(tex2Dlod(_Refractionnormalmap,float4(TRANSFORM_TEX(i.uv0, _Refractionnormalmap),0.0,0.0)));
                float3 normalLocal = _Refractionnormalmap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 node_14 = (float2(_Refractionnormalmap_var.r,_Refractionnormalmap_var.g)*_Refractionintensity);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + node_14;
                float4 sceneColor = tex2D(Refraction, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = _Gloss01;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Specular010,_Specular010,_Specular010);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float node_6644 = (1.0 - _Texture_var.a);
                float3 diffuseColor = ((_Color.rgb*_Texture_var.rgb)+(node_6644*_Glasscolor.rgb));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_Cubemapblur05));
                float3 node_1403 = (_Cubemap_var.rgb*_Cubemaptransparent01*node_6644);
                float node_7617 = dot(node_1403,float3(0.3,0.59,0.11)).r;
                float3 emissive = ((_Cubemapopaque01*_Cubemap_var.rgb*_Texture_var.a)+node_1403+node_7617);
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,max(max(_Glassopacity01,(_Texture_var.a*_Texturetransparency01)),node_7617)),1);
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
            uniform sampler2D Refraction;
            uniform sampler2D _Refractionnormalmap; uniform float4 _Refractionnormalmap_ST;
            uniform float _Specular010;
            uniform float _Gloss01;
            uniform float _Glassopacity01;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform samplerCUBE _Cubemap;
            uniform float _Cubemapopaque01;
            uniform float _Cubemaptransparent01;
            uniform float _Texturetransparency01;
            uniform float _Cubemapblur05;
            uniform float4 _Glasscolor;
            uniform float4 _Color;
            uniform float _Refractionintensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Refractionnormalmap_var = UnpackNormal(tex2Dlod(_Refractionnormalmap,float4(TRANSFORM_TEX(i.uv0, _Refractionnormalmap),0.0,0.0)));
                float3 normalLocal = _Refractionnormalmap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 node_14 = (float2(_Refractionnormalmap_var.r,_Refractionnormalmap_var.g)*_Refractionintensity);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + node_14;
                float4 sceneColor = tex2D(Refraction, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = _Gloss01;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Specular010,_Specular010,_Specular010);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float node_6644 = (1.0 - _Texture_var.a);
                float3 diffuseColor = ((_Color.rgb*_Texture_var.rgb)+(node_6644*_Glasscolor.rgb));
                float3 diffuse = directDiffuse * diffuseColor;
                float3 finalColor = diffuse + specular;
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_Cubemapblur05));
                float3 node_1403 = (_Cubemap_var.rgb*_Cubemaptransparent01*node_6644);
                float node_7617 = dot(node_1403,float3(0.3,0.59,0.11)).r;
                fixed4 finalRGBA = fixed4(finalColor * max(max(_Glassopacity01,(_Texture_var.a*_Texturetransparency01)),node_7617),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
