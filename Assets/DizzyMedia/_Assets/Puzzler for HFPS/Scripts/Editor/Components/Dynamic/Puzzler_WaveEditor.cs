using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(Puzzler_Wave))]
public class Puzzler_WaveEditor : Editor {


//////////////////////////
//
//      EDITOR DISPLAY
//
//////////////////////////
    
    
    Puzzler_Wave puzzlerWave;
    GUISkin oldSkin;
    
    public bool showTips;
    
    private void OnEnable() {
        
        puzzlerWave = (Puzzler_Wave)target;
        
    }//OnEnable

    public override void OnInspectorGUI() { 
    
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
    
        GUILayout.Space(15);
        
        var style = new GUIStyle(EditorStyles.largeLabel) {alignment = TextAnchor.MiddleCenter};
        
        if(oldSkin == null){
        
            if(oldSkin != Resources.Load("EditorContent/Puzzler Skin") as GUISkin){

                oldSkin = GUI.skin;

                //Debug.Log("Old Skin Name " + GUI.skin.name);

            }//oldSkin != IWC Skin
            
        }//oldSkin == null
        
        GUI.skin = Resources.Load("EditorContent/Puzzler Skin") as GUISkin;
        
        Texture2D t = (Texture2D)Resources.Load("EditorContent/Puzzler-Editor-Icon");
        Texture2D t2 = (Texture2D)Resources.Load("EditorContent/DM_InfoIcon");
        Texture2D t3 = (Texture2D)Resources.Load("EditorContent/DM_InfoIconActive");
        
        GUILayout.BeginHorizontal("Puzzler Wave", "headerText");
        
        GUILayout.Label(t, "headerIcon");
        
        GUILayout.FlexibleSpace();
        
        if(!showTips){
        
            if(GUILayout.Button(t2, "infoIcon")){

                ShowTips_Check();

            }//Button
        
        }//!showTips
        
        if(showTips){
        
            if(GUILayout.Button(t3, "infoIcon")){

                ShowTips_Check();

            }//Button
        
        }//showTips
        
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        
        GUI.skin = oldSkin;

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginVertical();
        
        puzzlerWave.tabs = GUILayout.SelectionGrid(puzzlerWave.tabs, new string[] { "User Options", "Auto"}, 2);
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        
        SerializedProperty lineRenderer = serializedObject.FindProperty("lineRenderer");
        
        SerializedProperty scale = serializedObject.FindProperty("scale");
        SerializedProperty startRotation = serializedObject.FindProperty("startRotation");
        
        if(puzzlerWave.tabs == 0){
        
            EditorGUILayout.Space();
            
            puzzlerWave.startOpts = GUILayout.Toggle(puzzlerWave.startOpts, "Start Options", GUI.skin.button);
            
            if(puzzlerWave.startOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Sets the line renderers width on start if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
            
                puzzlerWave.setWidth = EditorGUILayout.Toggle("Set Width?", puzzlerWave.setWidth);
                
                if(puzzlerWave.setWidth){
                
                    if(showTips){
                    
                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Width of the line to be set." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    puzzlerWave.width = EditorGUILayout.FloatField("Width", puzzlerWave.width);
            
                }//setWidth
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Sets the GameObject scale on start if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.setScale = EditorGUILayout.Toggle("Set Scale?", puzzlerWave.setScale);
                
                if(puzzlerWave.setScale){
                
                    if(showTips){
                    
                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Scale of the GameObject to be set." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(scale, true);
                
                }//setScale
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Sets the GameObject rotation on start if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.setRotation = EditorGUILayout.Toggle("Set Rotation?", puzzlerWave.setRotation);
                
                if(puzzlerWave.setRotation){
                
                    if(showTips){
                    
                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Rotation of the GameObject to be set." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(startRotation, true);
                
                }//setRotation
                
            }//startOpts
            
            EditorGUILayout.Space();

            puzzlerWave.waveOpts = GUILayout.Toggle(puzzlerWave.waveOpts, "Wave Options", GUI.skin.button);

            if(puzzlerWave.waveOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Line renderer used to draw the wave." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(lineRenderer, true);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Animates the wave if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.animate = EditorGUILayout.Toggle("Animate?", puzzlerWave.animate);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Total length of the wave." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.lengthOfLine = EditorGUILayout.IntField("Length Of Line", puzzlerWave.lengthOfLine);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Modulation of the wave." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.waveModulate = EditorGUILayout.FloatField("Wave Modulate", puzzlerWave.waveModulate);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Amplitude of the wave." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.amplitude = EditorGUILayout.FloatField("Amplitude", puzzlerWave.amplitude);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Wave length of the wave." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.wavelength = EditorGUILayout.FloatField("Wave Length", puzzlerWave.wavelength);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Speed of the wave." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerWave.waveSpeed = EditorGUILayout.FloatField("Wave Speed", puzzlerWave.waveSpeed);
                    
            }//waveOpts
            
            EditorGUILayout.Space();
            
            puzzlerWave.debugOpts = GUILayout.Toggle(puzzlerWave.debugOpts, "Debug Options", GUI.skin.button);
            
            if(puzzlerWave.debugOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "If ON the wave will update according to the editor UI, allowing for real time tweaking." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.LabelField("Use Editor UI values?", EditorStyles.centeredGreyMiniLabel);

                EditorGUILayout.Space();
                
                puzzlerWave.guiInt = GUILayout.Toolbar(puzzlerWave.guiInt, new string[] { "OFF", "ON" });

                if(puzzlerWave.guiInt == 0){

                    puzzlerWave.useGUI = false;

                }//guiInt == 0

                if(puzzlerWave.guiInt == 1){

                    puzzlerWave.useGUI = true;

                }//guiInt == 1
            
            }//debugOpts
             
        }//tabs = user options
        
        if(puzzlerWave.tabs == 1){
        
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.Space();
            
            if(showTips){
                
                EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);
                    
                EditorGUILayout.Space();
                
            }//showTips

            puzzlerWave.curWidth = EditorGUILayout.FloatField("Current Width", puzzlerWave.curWidth);
            puzzlerWave.curLengthOfLine = EditorGUILayout.IntField("Cur Length Of Line", puzzlerWave.curLengthOfLine);
            
            EditorGUILayout.Space();
            
            puzzlerWave.curAmplitude = EditorGUILayout.FloatField("Cur Amplitude", puzzlerWave.curAmplitude);
            puzzlerWave.curWavelength = EditorGUILayout.FloatField("Cur Wave Length", puzzlerWave.curWavelength);
            puzzlerWave.curWaveSpeed = EditorGUILayout.FloatField("Cur Wave Speed", puzzlerWave.curWaveSpeed);
        
        }//tabs = auto
        
        EditorGUILayout.Space();
        
        if(EditorGUI.EndChangeCheck()){

            serializedObject.ApplyModifiedProperties();

        }//EndChangeCheck
  
        if(GUI.changed){
            
            EditorUtility.SetDirty(puzzlerWave);
            
            if(!EditorApplication.isPlaying){
            
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            
            }//!isPlaying
        
        }//changed
        
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndVertical();

    }//OnInspectorGUI
    
    
//////////////////////////
//
//      TIPS ACTIONS
//
//////////////////////////


    public void ShowTips_Check(){
    
        if(showTips){
            
            showTips = false;
        
        //showTips
        } else {
        
            showTips = true;
            
        }//showTips
        
    }//ShowTips_Check

}//Puzzler_WaveEditor
