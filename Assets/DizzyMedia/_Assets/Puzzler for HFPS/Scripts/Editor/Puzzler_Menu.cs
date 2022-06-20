using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Puzzler_Menu : EditorWindow {


//////////////////////////////////////
///
///     MENU BUTTONS
///
///////////////////////////////////////
    
////////////////////////////////
///
///     COMPONENTS CREATE
///
////////////////////////////////
    
////////////////////
///
///     DYNAMIC
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Puzzler/Components/Dynamic/Puzzler Dial", false , 0)]
    public static void Create_PuzzDial() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<Puzzler_Dial>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_PuzzDial
    
    [MenuItem("Tools/Dizzy Media/Puzzler/Components/Dynamic/Puzzler Wave", false , 0)]
    public static void Create_PuzzWave() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<Puzzler_Wave>();
            
            if(Selection.gameObjects[0].GetComponent<LineRenderer>() != null){
                
                Selection.gameObjects[0].GetComponent<Puzzler_Wave>().lineRenderer = Selection.gameObjects[0].GetComponent<LineRenderer>();
        
            }//LineRenderer != null
            
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_PuzzWave
    
    
////////////////////
///
///     GENERAL
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Puzzler/Components/General/Puzzler Holder", false , 0)]
    public static void Create_PuzzHold() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<Puzzler_Holder>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_PuzzHold
    
    
////////////////////
///
///     SYSTEM
///
////////////////////
    
    
    [MenuItem("Tools/Dizzy Media/Puzzler/Components/Systems/Puzzler Handler", false , 0)]
    public static void Create_PuzzHand() {
        
        if(Selection.gameObjects.Length > 0){
            
            Selection.gameObjects[0].AddComponent<Puzzler_Handler>();
        
        //Selection > 0
        } else {
            
            if(EditorUtility.DisplayDialog("Error", "You must select an object to add the component to.", "Ok")){}
            
        }//Selection > 0
        
    }//Create_PuzzHand

}
