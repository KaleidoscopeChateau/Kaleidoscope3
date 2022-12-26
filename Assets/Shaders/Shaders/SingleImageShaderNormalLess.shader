Shader "TriplanarUltimate/SingleSurfaceShaderNormalLess" {
        
    //DUAL SURFACE SHADER
    //FREE TO USE, JUST CREDIT ME SOMEWHERE - Luke "Mab" V

    //||ACKNOWLEDGEMENTS||
    // Reoriented Normal Mapping
    // https://bgolus.medium.com/normal-mapping-for-a-triplanar-shader-10bf39dca05a

    Properties{
        [NoScaleOffset] [MainTexture] _MainTex("Main Texture", 2D) = "white" {}                         // The Main Colour Texture.
        _MainScaleFactor("Main Scale Factor", Vector) = (1, 1, 1)                                       // Non uniform scaling. Smaller number bigger texture.
        _MainOffsetFactor("Main Offset Factor", Vector) = (0, 0, 0)                                     // Non uniform offset. Used to scroll the texture in different directions.
        _Blending("Blending", Range(0,1)) = 0                                                           // Controls how the different planes blend into each other. 0 is sharp, 1 is blended
        [MainColor]_MainColor("Main Color", Color) = (1,1,1,1)                                          // The main top and bottom colours 
        _SecColor("Secondary Color", Color) = (1,1,1,1)                                                 // 
        _CChangeHeight("Colour Change Point", Float) = 0.0                                              // The world y value that chooses which colour the surface will be coloured by.
        _CChangeGrad("Colour Change Distance", Float) = 0.0                                             // How far the gradient between colours travels.
        _CChangePenPos("Colour Depth", Range(0,1)) = 0                                                  //
        _CChangePenWidth("Colour Depth Width", Range(0,1)) = 0                                          // How deep the colour penetrates.
        _CChangePenGrad("Depth Cutoff Smoothness", Range(0,1)) = 0                                      // Smoothnes of the transition.
        _CChangeAmbCol("Underneath Colour", Color) = (1,1,1,1)                                          // The secondary colour
        _Glossiness("Smoothness", Range(0, 1)) = 0.5                                                    // Primary Smoothness and metalic values to make your material shiny.
        [Gamma] _Metallic("Metallic", Range(0, 1)) = 0.5                                                //
        _SmoothStart("Smooth+Metal Depth", Range(0, 1)) = 0.5                                           // Values dictating how deep the smoothness and metallic values apply,
        _SmoothEnd("Smooth+Metal Width", Range(0, 1)) = .5                                              // dictated by the height of the interpolator texture.
        _SmoothSmooth("Smooth+Metal Gradient", Range(0, 1)) = 0                                         // 
        _SecGlossiness("Secondary Smoothness", Range(0, 1)) = 0                                         // Determining what the shinyness of the materials are outside of this band 
        [Gamma] _SecMetallic("Secondary Metallic", Range(0, 1)) = 0                                     // 
        [NoScaleOffset] _InterpBump("Interpolator Bump Texture", 2D) = "black" {}                       // The Interpolator Texture that interpolated between the main and secondary textures.
        _IntScaleFactor("Int Scale Factor", Vector) = (1, 1, 1)                                         // Nonuniform scaling for the interpolator.
        _IntOffsetFactor("Int Offset Factor", Vector) = (0, 0, 0)                                       // Nonuniform scrolling for the interpolator.
        _Sin("Sin(Int Rotation Factor)", Vector) = (0, 0, 0)                                            // Non Uniform rotating for the interpolator.
        _Cos("Cos(Int Rotation Factor)", Vector) = (1, 1, 1)                                            // Saved as the sin and cos of itself to improve the performance a smidge
        _MinNoise("Interpolator Low Threshold", Range(0, 0.99)) = 0                                     // Clamping values for the interpolator texture, 
        _MaxNoise("Interpolator High Threshold", Range(0.01, 1)) = 1                                    // Determines what part of the surface is flat and what is bumpy
        [Toggle]_IntPosInf("Use Interpolator Clamp", Float) = 0                                         // Toggle using start and end position
        _IntClampPos("Int Clamp Position", Vector) = (0, 0, 0)                                          // Positions and gradient to determine which parts of the material are influenced by the interpretor
        _IntClampWidth("Int Clamp Width", Vector) = (1, 1, 1)                                           //
        _IntPosGrad("Int Clamp Multiplier", Vector) = (1, 1, 1)                                         // Gradients for smooth interpolation
        _NoiseBumpStrength("Noise Bump Strength", Range(-1,1)) = 1                                      // Determines the intensity of the noise bump map. Can be negative to make the
                                                                                                        // second texture look raised above the main
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            #pragma target 3.0

            #include "UnityStandardUtils.cginc"


            //Declaring variables
            //Variables with names shared with those in the properties get the values set from them 
            //Other variables work as normal
            //To change a value from a c# script, use material.Set(Inserttypehere)("NameofVariable", value)
            //Variables do not need to be properties in order for you to change them via script
            //When giving the name of the variable, make sure you use the actual name of the variable and not the display name given in the inspector
            //ie when modifying _MainColor("Main Color", Color), use material.SetColor("_MainColor", Color.White) and not material.SetColor("Main Colour", Color.White)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _MainScaleFactor;
            float3 _MainOffsetFactor;
            float _Blending;
            float4 _MainColor;
            float4 _SecColor;
            float _CChangeHeight;
            float _CChangeGrad;
            float _CChangePenPos;
            float _CChangePenWidth;
            float _CChangePenGrad;
            float4 _CChangeAmbCol;
            half _Glossiness;
            half _Metallic;
            half _SecGlossiness;
            half _SecMetallic;
            float _MinNoise;
            float _MaxNoise;
            float _NoiseBumpStrength;
            float _SmoothStart;
            float _SmoothEnd;
            float _SmoothSmooth;
            float3 _IntScaleFactor;
            float3 _IntOffsetFactor;
            float3 _Sin;
            float3 _Cos;
            sampler2D _InterpBump;
            float3 _IntClampPos;
            float3 _IntClampWidth;
            float _IntPosInf;
            float3 _IntPosGrad;
            float3 _IntPosEGrad;

            struct Input {
                float3 worldPos;
                float3 worldNormal;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {




                // calculate triplanar blend
                half3 triblend = saturate(pow(IN.worldNormal, 4));                                                  //Multiplies the world normal by a power of 4, then clamps the values to be between 0 and 1
                triblend /= max(dot(triblend, half3(1,1,1)), 0.0001);                                               //Gets the average or something idk 
                half m = max(triblend.x, max(triblend.y, triblend.z));                                              //Calculates which direction is the highest 
                half3 Noblend = half3(triblend.x == m, triblend.y == m, triblend.z == m);                           //Sets that direction to 1 and the rest to 0, this results in no blending between the planes
                triblend = lerp(Noblend, triblend, _Blending);                                                      //Interpolating between the average and the max versions to allow for user control over the smoothness of the blending

                //Rotation Matrixes for the interpolators
                float2x2 mat1 = float2x2(_Cos.x, -_Sin.x, _Sin.x, _Cos.x); float2x2 mat2 = float2x2(_Cos.y, -_Sin.y, _Sin.y, _Cos.y); float2x2 mat3 = float2x2(_Cos.z, -_Sin.z, _Sin.z, _Cos.z);
                //Warning! high rotation will result in the normals not being correct, so only rotate a little
                // calculate triplanar uvs
                float2 uvX = IN.worldPos.zy * _MainScaleFactor.zy + _MainOffsetFactor.zy;                           //Taking the different world planes and applying scaling and offsetting
                float2 uvY = IN.worldPos.xz * _MainScaleFactor.xz + _MainOffsetFactor.xz;                           //These apply to the main texture and its normal 
                float2 uvZ = IN.worldPos.xy * _MainScaleFactor.xy + _MainOffsetFactor.xy;
                float2 uvX3 = mul(mat1 , (IN.worldPos.zy * _IntScaleFactor.zy + _IntOffsetFactor.zy));              //And these to the interpolator texture and its normal
                float2 uvY3 = mul(mat2 , (IN.worldPos.xz * _IntScaleFactor.xz + _IntOffsetFactor.xz));
                float2 uvZ3 = mul(mat3 , (IN.worldPos.xy * _IntScaleFactor.xy + _IntOffsetFactor.xy));
               

                fixed4 c3olX = tex2D(_InterpBump, uvX3);                                                            //Interpolator Bump Texture value
                fixed4 c3olY = tex2D(_InterpBump, uvY3);
                fixed4 c3olZ = tex2D(_InterpBump, uvZ3);
                float dn = c3olX * triblend.x + c3olY * triblend.y + c3olZ * triblend.z;                            //Unclamped texture for more consistent reference points, Some n values would cause things to break lol
                float3 v = abs(IN.worldPos - _IntClampPos) - _IntClampWidth;

                float nn = max(v.x / _IntPosGrad.x, max(v.y / _IntPosGrad.y, v.z / _IntPosGrad.z)) / 2;
                nn = saturate(nn) * _IntPosInf;
                dn = saturate(dn - nn);
                float n = saturate(clamp(dn, _MinNoise, _MaxNoise) - _MinNoise) / (_MaxNoise - _MinNoise);

                //variables before calcs to allow for if statements
                //if statements are generally bad for performance, but this shader is so taxing otherwise that adding them actually saves performance 
                //without if statements, we are doing 12 image samples, with if statements, on the parts of the texture that are 0 or 1 in the interpolator, its 9
                //saves a surprising amount of frames from limited testing

                fixed4 col = fixed4(1, 1, 1, 1);
                fixed4 c2ol = fixed4(1, 1, 1, 1);

                // albedo textures
                
                    fixed4 colX = tex2D(_MainTex, uvX);                                                                         //Assigning the different planes of the triplanar mapping 
                    fixed4 colY = tex2D(_MainTex, uvY);
                    fixed4 colZ = tex2D(_MainTex, uvZ);
                    col = colX * triblend.x + colY * triblend.y + colZ * triblend.z;                                            //Blending them all together based on the triplanar blend value calculated earlier 
                


                
                // set surface ouput properties

                float colp = (abs(dn - _CChangePenPos) - _CChangePenWidth) / _CChangePenGrad;                                                                    //lerp value between the primary colours and the secondary colour

                float4 cc = lerp(lerp(_SecColor, _MainColor, clamp((IN.worldPos.y - _CChangeHeight) / _CChangeGrad, 0,1)), _CChangeAmbCol, saturate(colp));       //Applying the colour to the based on the parameters given
                float4 ccc = col * cc;                                                                                                           
                o.Albedo = ccc;                                                                                                                                 //setting the colour of the material 
                float sm = (abs(dn - _SmoothStart) - _SmoothEnd) / _SmoothSmooth;                                                                               //lerp value between the primary and secondary smoothness
                o.Metallic = lerp(_Metallic, _SecMetallic, saturate(sm))* ccc.a;                                                                                       //applying which smoothness value based on that
                o.Smoothness = lerp(_Glossiness, _SecGlossiness, saturate(sm))* ccc.a;




            }
            ENDCG
        }
            FallBack "Diffuse"
                CustomEditor "DualSurfaceShaderEditor"
}
