using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using HFPS.Systems;


[AddComponentMenu("Dizzy Media/Puzzler/Components/Dynamic/Puzzler Wave")]
[RequireComponent(typeof(LineRenderer))]
public class Puzzler_Wave : MonoBehaviour, ISaveable {
    

//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
///////////////////////////
///
///     USER OPTIONS
///
///////////////////////////
    
///////////////////
///
///     START OPTIONS
///
///////////////////

    
    public bool setWidth;
    public float width;
    
    public bool setScale;
    public Vector3 scale;
    
    public bool setRotation;
    public Vector3 startRotation;
    
    
///////////////////
///
///     WAVE OPTIONS
///
///////////////////

    
    public LineRenderer lineRenderer;
    
    public bool animate;

    public float waveModulate;
    public int lengthOfLine;
    
    public float amplitude;
    public float wavelength;
    public float waveSpeed;
    
    
///////////////////
///
///     DEBUG OPTIONS
///
///////////////////
    

    public bool useGUI;
    
    
///////////////////////////
///
///     AUTO
///
///////////////////////////
    
    
    private AnimationCurve curve;
    
    public float curWidth;
    public int curLengthOfLine;
    
    public float curAmplitude;
    public float curWavelength;
    public float curWaveSpeed;
    
    private float x;
    private float y;
    private float k;
    private float w;
    
    public int tabs;
    
    public bool startOpts;
    public bool waveOpts;
    public bool debugOpts;
    
    public int guiInt;
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    
    
    void Awake(){
    
        curAmplitude = amplitude;
        curWavelength = wavelength;
        curWaveSpeed = waveSpeed;
        
        curWidth = width;
        curLengthOfLine = lengthOfLine;
        
    }//Awake
    
    void Start() {
        
        StartInit();
        
    }//Start
    
    public void StartInit(){
        
        if(setScale){
        
            this.transform.localScale = scale;
        
        }//setScale
        
        if(setRotation){
        
            this.transform.localEulerAngles = startRotation;
        
        }//setRotation
        
        if(setWidth){
            
            curve = new AnimationCurve();
            
            curve.AddKey(0.0f, curWidth);

            lineRenderer.widthCurve = curve;
        
        }//setWidth
        
    }//StartInit
    
    
//////////////////////////////////////
///
///     UPDATE ACTIONS
///
///////////////////////////////////////
    
    
    void Update(){
        
        if(useGUI){
            
            curWidth = width;
            curLengthOfLine = lengthOfLine;

            curAmplitude = amplitude;
            curWavelength = wavelength;
            curWaveSpeed = waveSpeed;
            
            this.transform.localScale = scale;
            
            if(setWidth){

                curve = new AnimationCurve();

                curve.AddKey(0.0f, curWidth);

                lineRenderer.widthCurve = curve;

            }//setWidth
        
        }//useGUI
        
        if(animate){
        
            DrawSineWave(new Vector3(0, 0, 0), curAmplitude, curWavelength, curWaveSpeed);
        
        }//animate
        
    }//Update
    

//////////////////////////////////////
///
///     DRAW ACTIONS
///
///////////////////////////////////////
    
    
    private void DrawSineWave(Vector3 startPoint, float amplitude, float wavelength, float waveSpeed){

        x = 0f;
        k = 2 * Mathf.PI / wavelength;
        w = k * waveSpeed;
        
        lineRenderer.positionCount = curLengthOfLine;
        
        for(int i = 0; i < lineRenderer.positionCount; i++){
        
            x += i * waveModulate;
            y = amplitude * Mathf.Sin(k * x + w * Time.time);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0) + startPoint);
            
        }//for i lineRenderer
        
    }//DrawSineWave
    
    
//////////////////////////////////////
///
///     ANIMATE ACTIONS
///
///////////////////////////////////////
    
    
    public void Animate_State(bool state){
        
        animate = state;
        
    }//Animate_State
    
    
//////////////////////////////////////
///
///     WIDTH ACTIONS
///
///////////////////////////////////////
    
    
    public void Width_Set(float newValue){
        
        curWidth = newValue;
        
    }//Width_Set
    
    public void Width_Reset(){
        
        curWidth = width;
        
    }//Width_Reset
    
    
//////////////////////////////////////
///
///     LENGTH OF LINE ACTIONS
///
///////////////////////////////////////
    
    
    public void LengthOfLine_Set(int newValue){
        
        curLengthOfLine = newValue;
        
    }//LengthOfLine_Set
    
    public void LengthOfLine_Reset(){
        
        curLengthOfLine = lengthOfLine;
        
    }//LengthOfLine_Reset
    
    
//////////////////////////////////////
///
///     AMPLITUDE ACTIONS
///
///////////////////////////////////////
    
    
    public void Amplitude_Set(float newValue){
        
        curAmplitude = newValue;
        
    }//Amplitude_Set
    
    public void Amplitude_Reset(){
        
        curAmplitude = amplitude;
        
    }//Amplitude_Reset
    
    
//////////////////////////////////////
///
///     WAVE LENGTH ACTIONS
///
///////////////////////////////////////
    
    
    public void WaveLength_Set(float newValue){
        
        curWavelength = newValue;
        
    }//WaveLength_Set
    
    public void WaveLength_Reset(){
        
        curWavelength = wavelength;
        
    }//WaveLength_Reset
    
    
//////////////////////////////////////
///
///     WAVE LENGTH ACTIONS
///
///////////////////////////////////////
    
    
    public void WaveSpeed_Set(float newValue){
        
        curWaveSpeed = newValue;
        
    }//WaveSpeed_Set
    
    public void WaveSpeed_Reset(){
        
        curWaveSpeed = waveSpeed;
        
    }//WaveSpeed_Reset
    
    
//////////////////////////////////////
///
///     SAVE/LOAD ACTIONS
///
///////////////////////////////////////
    
    
    public Dictionary<string, object> OnSave() {
        
        return new Dictionary<string, object> {
            
            {"animate", animate},
            {"curAmplitude", curAmplitude },
            {"curWavelength", curWavelength },
            {"curWaveSpeed", curWaveSpeed }
        
        };//Dictionary
        
    }//OnSave

    public void OnLoad(JToken token) {

        animate = (bool)token["animate"];
        curAmplitude = (float)token["curAmplitude"];
        curWavelength = (float)token["curWavelength"];
        curWaveSpeed = (float)token["curWaveSpeed"];
        
    }//OnLoad
    

}//Puzzler_Wave
