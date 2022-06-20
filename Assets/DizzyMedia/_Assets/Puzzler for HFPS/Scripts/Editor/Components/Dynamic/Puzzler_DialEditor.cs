using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(Puzzler_Dial))]
public class Puzzler_DialEditor : Editor {

    
//////////////////////////
//
//      EDITOR DISPLAY
//
//////////////////////////
    
    
    Puzzler_Dial puzzlerDial;
    GUISkin oldSkin;
    
    public bool showTips;
    
    private void OnEnable() {
        
        puzzlerDial = (Puzzler_Dial)target;
        
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
        
        GUILayout.BeginHorizontal("Puzzler Dial", "headerText");
        
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
        
        puzzlerDial.tabs = GUILayout.SelectionGrid(puzzlerDial.tabs, new string[] { "User Options", "Events", "Auto"}, 3);
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        
        SerializedProperty useInput = serializedObject.FindProperty("useInput");
        
        SerializedProperty rotateTypeRef = serializedObject.FindProperty("rotateType");
        SerializedProperty turnAxis = serializedObject.FindProperty("turnAxis");
        SerializedProperty startRotation = serializedObject.FindProperty("startRotation");
        SerializedProperty rotationSlots = serializedObject.FindProperty("rotationSlots");
        
        SerializedProperty saveState = serializedObject.FindProperty("saveState");
        
        SerializedProperty dialTurnSounds = serializedObject.FindProperty("dialTurnSounds");
        
        SerializedProperty onInteract = serializedObject.FindProperty("onInteract");
        SerializedProperty onInteractLate = serializedObject.FindProperty("onInteractLate");
        
        if(puzzlerDial.tabs == 0){
        
            EditorGUILayout.Space();
            
            puzzlerDial.startOpts = GUILayout.Toggle(puzzlerDial.startOpts, "Start Options", GUI.skin.button);
            
            if(puzzlerDial.startOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Sets this objects rotation on start if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerDial.setRotation = EditorGUILayout.Toggle("Set Rotation?", puzzlerDial.setRotation);
                
                if(puzzlerDial.setRotation){
                
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The rotation set when start is called." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    EditorGUILayout.PropertyField(startRotation, true);
                
                }//setRotation
                
            }//startOpts
            
            EditorGUILayout.Space();
            
            puzzlerDial.genOpts = GUILayout.Toggle(puzzlerDial.genOpts, "General Options", GUI.skin.button);
            
            if(puzzlerDial.genOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Saves/loads the current state of the dial." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(saveState, true);
                
            }//genOpts
        
            EditorGUILayout.Space();
            
            puzzlerDial.inputOpts = GUILayout.Toggle(puzzlerDial.inputOpts, "Input Options", GUI.skin.button);
            
            if(puzzlerDial.inputOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Name of the input used for interaction." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(useInput, true);
                
            }//inputOpts
            
            EditorGUILayout.Space();

            puzzlerDial.rotateOpts = GUILayout.Toggle(puzzlerDial.rotateOpts, "Rotate Options", GUI.skin.button);

            if(puzzlerDial.rotateOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Type of rotation to use when interacted with." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                EditorGUILayout.PropertyField(rotateTypeRef, true);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "The axis in which the rotation will occur on." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(turnAxis, true);
                
                if((int)puzzlerDial.rotateType == 0){
                
                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The speed in which the dial will rotate at." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    puzzlerDial.dialTurnSpeed = EditorGUILayout.FloatField("Turn Speed", puzzlerDial.dialTurnSpeed);

                }//rotateType = continuous

                if((int)puzzlerDial.rotateType == 1){
                
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Slot used for start action/rotation." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    puzzlerDial.startSlot = EditorGUILayout.IntField("Start Slot", puzzlerDial.startSlot);
                    
                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Slots used for rotating the dial and calling events per slot." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(rotationSlots, true);

                }//rotateType = slots
                    
            }//rotateOpts
            
            EditorGUILayout.Space();
            
            puzzlerDial.soundOpts = GUILayout.Toggle(puzzlerDial.soundOpts, "Sound Options", GUI.skin.button);
            
            if(puzzlerDial.soundOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Plays sounds when sound actions are called if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerDial.useSounds = EditorGUILayout.Toggle("Use Sounds?", puzzlerDial.useSounds);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "The wait time in between sound plays." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerDial.dialSoundAfter = EditorGUILayout.FloatField("Sound After", puzzlerDial.dialSoundAfter);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "The volume used when playing sounds." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerDial.m_Volume = EditorGUILayout.FloatField("Volume", puzzlerDial.m_Volume);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Sounds used when interacting with the dial." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(dialTurnSounds, true);
            
            }//soundOpts
             
        }//tabs = user options
        
        if(puzzlerDial.tabs == 1){
        
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(onInteract, true);
            
            EditorGUILayout.Space();
            
            puzzlerDial.lateDelay = EditorGUILayout.FloatField("Late Delay", puzzlerDial.lateDelay);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(onInteractLate, true);
        
        }//tabs = events
        
        if(puzzlerDial.tabs == 2){
        
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.Space();
            
            if(showTips){
                
                EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);
                    
                EditorGUILayout.Space();
                
            }//showTips
            
            if((int)puzzlerDial.rotateType == 0){
            
                puzzlerDial.canRotate = EditorGUILayout.Toggle("Can Rotate?", puzzlerDial.canRotate);
                puzzlerDial.useKeyPressed = EditorGUILayout.Toggle("UseKey Pressed?", puzzlerDial.useKeyPressed);
                puzzlerDial.isHolding = EditorGUILayout.Toggle("Is Holding?", puzzlerDial.isHolding);
                puzzlerDial.turnSound = EditorGUILayout.Toggle("TurnSounds?", puzzlerDial.turnSound);
                
                EditorGUILayout.Space();
            
            }//rotateType = continuous
            
            if((int)puzzlerDial.rotateType == 1){
            
                EditorGUILayout.Space();
            
                puzzlerDial.curSlot = EditorGUILayout.IntField("Current Slot", puzzlerDial.curSlot);
            
            }//rotateType = slots
            
            puzzlerDial.locked = EditorGUILayout.Toggle("Locked?", puzzlerDial.locked);
        
        }//tabs = auto
        
        EditorGUILayout.Space();
        
        if(EditorGUI.EndChangeCheck()){

            serializedObject.ApplyModifiedProperties();

        }//EndChangeCheck
  
        if(GUI.changed){
            
            EditorUtility.SetDirty(puzzlerDial);
            
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
    
}
