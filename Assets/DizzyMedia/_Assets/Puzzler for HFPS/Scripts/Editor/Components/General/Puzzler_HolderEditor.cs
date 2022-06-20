using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(Puzzler_Holder))]
public class Puzzler_HolderEditor : Editor {


//////////////////////////
//
//      EDITOR DISPLAY
//
//////////////////////////
    
    
    Puzzler_Holder puzzlerHold;
    GUISkin oldSkin;
    
    public bool showTips;
    
    private void OnEnable() {
        
        puzzlerHold = (Puzzler_Holder)target;
        
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
        
        GUILayout.BeginHorizontal("Puzzler Holder", "headerText_Small");
        
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
        
        puzzlerHold.tabs = GUILayout.SelectionGrid(puzzlerHold.tabs, new string[] { "References", "Auto"}, 2);
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        
        SerializedProperty trigger = serializedObject.FindProperty("trigger");
        SerializedProperty rigid = serializedObject.FindProperty("rigid");
        
        SerializedProperty puzzlerHand = serializedObject.FindProperty("puzzlerHand");
        
        if(puzzlerHold.tabs == 0){
        
            EditorGUILayout.Space();
            
            if(showTips){
                
                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("\n" + "The collider/trigger used for detecting player interaction." + "\n", MessageType.Info);

                EditorGUILayout.Space();

            }//showTips
            
            EditorGUILayout.PropertyField(trigger, true);
            
            if(showTips){
                
                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("\n" + "The rigidbody used for interactable items." + "\n", MessageType.Info);

                EditorGUILayout.Space();

            }//showTips
            
            EditorGUILayout.PropertyField(rigid, new GUIContent("Rigidbody"), true);
             
        }//tabs = user options
        
        if(puzzlerHold.tabs == 1){
        
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.Space();
            
            if(showTips){
                
                EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);
                    
                EditorGUILayout.Space();
                
            }//showTips

            puzzlerHold.slot = EditorGUILayout.IntField("Slot", puzzlerHold.slot);
            puzzlerHold.secondSlot = EditorGUILayout.IntField("Second Slot", puzzlerHold.secondSlot);
            puzzlerHold.weight = EditorGUILayout.FloatField("Weight", puzzlerHold.weight);
            EditorGUILayout.PropertyField(puzzlerHand, true);
        
        }//tabs = auto
        
        EditorGUILayout.Space();
        
        if(EditorGUI.EndChangeCheck()){

            serializedObject.ApplyModifiedProperties();

        }//EndChangeCheck
  
        if(GUI.changed){
            
            EditorUtility.SetDirty(puzzlerHold);
            
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
