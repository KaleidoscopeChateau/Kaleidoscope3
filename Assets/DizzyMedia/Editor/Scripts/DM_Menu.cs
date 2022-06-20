using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DM_Menu : EditorWindow {
    
    
//////////////////////////////////////
///
///     MENU BUTTONS
///
///////////////////////////////////////
    
////////////////////////////////
///
///     UTILITIES CREATE
///
////////////////////////////////
    
////////////////////
///
///     EFFECTS
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Utilities/Effects/Dissolve Controller", false , 11)]
    public static void Create_DissolveCont() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<DM_DissolveCont>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_DissolveCont
    
    
////////////////////
///
///     GIZMOS
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Utilities/Gizmos/Simple Icon", false , 11)]
    public static void Create_SimpIcon() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<SimpleIcon>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_SimpIcon
    
    [MenuItem("Tools/Dizzy Media/Utilities/Gizmos/Transform Indicator", false , 11)]
    public static void Create_TransInd() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<TransInd>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_TransInd   
    
    
////////////////////
///
///     HFPS
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Utilities/HFPS/Scare Handler", false , 11)]
    public static void Create_ScareHand() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<ScareHand>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_ScareHand 
    
    
}
