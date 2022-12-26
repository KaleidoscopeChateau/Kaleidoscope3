Shader "TriplanarUltimate/SingleSurfaceShaderLightWeight" {
        
        //DUAL SURFACE SHADER
        //FREE TO USE, JUST CREDIT ME SOMEWHERE - Luke "Mab" V

        //||ACKNOWLEDGEMENTS||
        // Reoriented Normal Mapping
        // https://bgolus.medium.com/normal-mapping-for-a-triplanar-shader-10bf39dca05a

    Properties{
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "white" {}                         // The Main Colour Texture.
        [NoScaleOffset][Normal] _BumpMap("Normal Map", 2D) = "bump" {}                                  // The Nomal Textrue for your Main Colour Texture.
        _MainScaleFactor("Main Scale Factor", Vector) = (1, 1, 1)                                       // Non uniform scaling. Smaller number bigger texture.
        _MainOffsetFactor("Main Offset Factor", Vector) = (0, 0, 0)                                     // Non uniform offset. Used to scroll the texture in different directions.
        _MainColor("Main Color", Color) = (1,1,1,1)                                          // The main top and bottom colours 
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
        [NoScaleOffset][Normal] _InterpNormal("Interpolator Normal Texture", 2D) = "bump" {}            // The Normal Texture for that. Just get/make a heightmap of something and use https://cpetry.github.io/NormalMap-Online/ to turn it into a normal. 
                                                                                                        // On the site, crank the strength, level and sharp to the max. Dont forget to label the texture as a normal map in the editor!
                                                                                                        // Mark McKay made a great tool for generating tileing noise maps, cant recommend enough http://kitfox.com/projects/perlinNoiseMaker/
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
            sampler2D _BumpMap;
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
            sampler2D _InterpNormal;
            float3 _IntClampPos;
            float3 _IntClampWidth;
            float _IntPosInf;
            float3 _IntPosGrad;
            float3 _IntPosEGrad;

            struct Input {
                float3 worldPos;
                float3 worldNormal;
                INTERNAL_DATA
            };

            //Takes the world normal vector and converts it into a 2d tangent vector, otherwise it looks very wrong
            float3 WorldToTangentNormalVector(Input IN, float3 normal) {
                float3 t2w0 = WorldNormalVector(IN, float3(1,0,0));
                float3 t2w1 = WorldNormalVector(IN, float3(0,1,0));
                float3 t2w2 = WorldNormalVector(IN, float3(0,0,1));
                float3x3 t2w = float3x3(t2w0, t2w1, t2w2);
                return normalize(mul(t2w, normal));
            }


            void surf(Input IN, inout SurfaceOutputStandard o) {

                

                //  For whatever reason, even when displaying the normal map as the colour shows that everything is fine, without doing this the whole material turns black
                //  This also makes it so that if you dont input the normal at the end of the script, the colour displays incorrectly
                //  Why? No clue
                IN.worldNormal = WorldNormalVector(IN, float3(0,0,1));

                // calculate triplanar blend
                half3 triblend = saturate(pow(IN.worldNormal, 4));                                                  //Multiplies the world normal by a power of 4, then clips the values to be between 0 and 1
                triblend /= max(dot(triblend, half3(1,1,1)), 0.0001);                                               //Gets the average or something idk 
                half m = max(triblend.x, max(triblend.y, triblend.z));                                              //Calculates which direction is the highest 
                triblend = half3(triblend.x == m, triblend.y == m, triblend.z == m);                                //Sets that direction to 1 and the rest to 0, this results in no blending between the planes

                //Rotation Matrixes for the interpolators
                float2x2 mat1 = triblend.x == 0 ? float2x2(_Cos.y, -_Sin.y, _Sin.y, _Cos.y) : float2x2(_Cos.x, -_Sin.x, _Sin.x, _Cos.x); mat1 = triblend.z == 0 ? mat1 : float2x2(_Cos.z, -_Sin.z, _Sin.z, _Cos.z);
                //Warning! high rotation will result in the normals not being correct, so only rotate a little
                // calculate triplanar uvs
                float2 uvX = IN.worldPos.zy * _MainScaleFactor.zy + _MainOffsetFactor.zy;                           //Taking the different world planes and applying scaling and offsetting
                float2 uvY = IN.worldPos.xz * _MainScaleFactor.xz + _MainOffsetFactor.xz;                           //These apply to the main texture and its normal 
                float2 uvZ = IN.worldPos.xy * _MainScaleFactor.xy + _MainOffsetFactor.xy;
               
                float2 uvX3 = IN.worldPos.zy * _IntScaleFactor.zy + _IntOffsetFactor.zy;                            //And these to the interpolator texture and its normal
                float2 uvY3 = IN.worldPos.xz * _IntScaleFactor.xz + _IntOffsetFactor.xz;
                float2 uvZ3 = IN.worldPos.xy * _IntScaleFactor.xy + _IntOffsetFactor.xy;

                float2 uv1 = triblend.x == 0 ? uvY : uvX; uv1 = triblend.z == 0 ? uv1 : uvZ;                        //Setting a single uv map for each triplanar face according to the max triblend value
                float2 uv3 = triblend.x == 0 ? uvY3 : uvX3; uv3 = triblend.z == 0 ? uv3 : uvZ3;                     
                uv3 = mul(mat1, uv3);

                //Interpolator Bump Texture value
                float dn = tex2D(_InterpBump, uv3);                    //Unclamped texture for more consistent reference points, Some n values would cause things to break lol
                
                float3 v = abs(IN.worldPos - _IntClampPos) - _IntClampWidth;

                float nn = max(v.x / _IntPosGrad.x, max(v.y / _IntPosGrad.y, v.z / _IntPosGrad.z)) / 2;
                nn = saturate(nn) * _IntPosInf;
                dn = saturate(dn - nn);
                float n = saturate(clamp(dn, _MinNoise, _MaxNoise) - _MinNoise) / (_MaxNoise - _MinNoise);

                //variables before calcs to allow for if statements
                //remnant from the full fat shader, but shouldnt be too taxing to keep

                fixed4 col = tex2D(_MainTex, uv1);
                half3 B1X = UnpackNormal(tex2D(_BumpMap, uv1));
                half3 B1Y = B1X;
                half3 B1Z = B1X;
                half3 inormalX = half3(0.5, 0.5, 1);
                half3 inormalY = half3(0.5, 0.5, 1);
                half3 inormalZ = half3(0.5, 0.5, 1);


                // tangent space normal maps
                half3 tnormalX = B1X;                       //
                half3 tnormalY = tnormalX;                  //Assigning the rest of the triplanar sides to be all the same, as while we're only using one sample for the chosen side,
                half3 tnormalZ = tnormalX;                  //We still need to do the proper normal transformations which still require triplanar mapping

                float i = 0;

                if (n - int(n) != 0)                        //Still keeping this if statement since its a bit chunky
                {
                    inormalX = UnpackNormal(tex2D(_InterpNormal, uv3));                                                                             //Assigning interpolator normal texture values
                    inormalX.x *= sign(_NoiseBumpStrength);                                                                                         //Flipping the normal texture if strength is negative
                    inormalX.y *= -sign(_NoiseBumpStrength);
                    inormalX = lerp(tnormalX, inormalX, abs(_NoiseBumpStrength));                                                                   //Lerping between the regular normal map and the interps for strength
                    inormalY = inormalX;        
                    inormalZ = inormalX;        
                    
                    
                                                                                 
                    
                    i = saturate((dn > _MinNoise && dn < _MaxNoise) - nn);
                    tnormalX = lerp(tnormalX, inormalX, i);
                    tnormalY = tnormalX;
                    tnormalZ = tnormalX;
                }
                
               
                tnormalX = half3(tnormalX.xy + IN.worldNormal.zy, IN.worldNormal.x);
                tnormalY = half3(tnormalY.xy + IN.worldNormal.xz, IN.worldNormal.y);
                tnormalZ = half3(tnormalZ.xy + IN.worldNormal.xy, IN.worldNormal.z); 


                // sizzle tangent normals to match world normal and blend together
                half3 worldNormal = normalize(
                    tnormalX.zyx * triblend.x +
                    tnormalY.xzy * triblend.y +
                    tnormalZ.xyz * triblend.z
                    );


                // set surface ouput properties
                
                float colp = (abs(dn - _CChangePenPos) - _CChangePenWidth) / _CChangePenGrad;                                                                           //lerp value between the primary colours and the secondary colour

                float3 Normal = WorldToTangentNormalVector(IN, worldNormal);                                                                                            //Turn the world normal into a tangent one
                float4 cc = lerp(lerp(_SecColor, _MainColor, clamp((IN.worldPos.y - _CChangeHeight) / _CChangeGrad, 0, 1)), _CChangeAmbCol, saturate(colp));            //Applying the colour to the based on the parameters given
                float4 ccc = col * cc;                                                                                                                                  // 
                o.Albedo = ccc;                                                                                                                                         //setting the colour of the material 
                float sm = (abs(dn - _SmoothStart) - _SmoothEnd) / _SmoothSmooth;                                                                                       //lerp value between the primary and secondary smoothness
                o.Metallic = lerp(_Metallic, _SecMetallic, saturate(sm))* ccc.a;                                                                                               //applying which smoothness value based on that
                o.Smoothness = lerp(_Glossiness, _SecGlossiness, saturate(sm))* ccc.a;

               
                
                
                

                o.Normal = Normal;                                                                                              //Applies normal
                                                                                                   
                                                                                                  
            }
            ENDCG
        }
            FallBack "Diffuse"
            CustomEditor "DualSurfaceShaderEditor"
}
