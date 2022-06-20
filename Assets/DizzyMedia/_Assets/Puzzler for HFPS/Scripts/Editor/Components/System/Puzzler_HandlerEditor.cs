using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(Puzzler_Handler))]
public class Puzzler_HandlerEditor : Editor {


//////////////////////////
//
//      EDITOR DISPLAY
//
//////////////////////////
    
    
    Puzzler_Handler puzzlerHand;
    GUISkin oldSkin;
    
    public bool showTips;
    
    private void OnEnable() {
        
        puzzlerHand = (Puzzler_Handler)target;
        
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
        
        GUILayout.BeginHorizontal("Puzzler Handler", "headerText_Small");
        
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
        
        puzzlerHand.puzzlerTabs = GUILayout.SelectionGrid(puzzlerHand.puzzlerTabs, new string[] { "User Options", "Events", "Auto"}, 3);
        
        EditorGUILayout.Space();
        
        EditorGUI.BeginChangeCheck();
        
        SerializedProperty puzzleTypeRef = serializedObject.FindProperty("puzzleType");
        SerializedProperty resetType = serializedObject.FindProperty("resetType");
        SerializedProperty completeTypeRef = serializedObject.FindProperty("completeType");
        SerializedProperty lockType = serializedObject.FindProperty("lockType");
        SerializedProperty saveState = serializedObject.FindProperty("saveState");
        SerializedProperty rotateType = serializedObject.FindProperty("rotateType");
        
        SerializedProperty itemID = serializedObject.FindProperty("itemID");
        SerializedProperty interactSound = serializedObject.FindProperty("interactSound");
        
        SerializedProperty interactTypeRef = serializedObject.FindProperty("interactType");
        SerializedProperty itemUseType = serializedObject.FindProperty("itemUseType");
        
        SerializedProperty selectText = serializedObject.FindProperty("selectText");
        SerializedProperty emptyText = serializedObject.FindProperty("emptyText");
        SerializedProperty fullText = serializedObject.FindProperty("fullText");
        
        SerializedProperty soloPrefab = serializedObject.FindProperty("soloPrefab");
        SerializedProperty multiPrefabs = serializedObject.FindProperty("multiPrefabs");
        
        SerializedProperty soloSlots = serializedObject.FindProperty("soloSlots");
        SerializedProperty multiSlots = serializedObject.FindProperty("multiSlots");
        
        SerializedProperty rotateSlots = serializedObject.FindProperty("rotateSlots");
        SerializedProperty rotateModules = serializedObject.FindProperty("rotateModules");
        
        SerializedProperty weightItems = serializedObject.FindProperty("weightItems");
        SerializedProperty weightModules = serializedObject.FindProperty("weightModules");
        
        SerializedProperty sequenceOrder = serializedObject.FindProperty("sequenceOrder");
        SerializedProperty sequence = serializedObject.FindProperty("sequence");
        
        SerializedProperty lightsActive = serializedObject.FindProperty("lightsActive");
        SerializedProperty lights = serializedObject.FindProperty("lights");
        
        SerializedProperty switchesActive = serializedObject.FindProperty("switchesActive");
        SerializedProperty switches = serializedObject.FindProperty("switches");
        
        SerializedProperty moduleSlots = serializedObject.FindProperty("moduleSlots");
        
        SerializedProperty soundSource = serializedObject.FindProperty("soundSource");
        SerializedProperty soundLibrary = serializedObject.FindProperty("soundLibrary");
        
        SerializedProperty onPuzzleComplete = serializedObject.FindProperty("onPuzzleComplete");
        SerializedProperty onPuzzleCompleteDelayed = serializedObject.FindProperty("onPuzzleCompleteDelayed");
        SerializedProperty onPuzzleFail = serializedObject.FindProperty("onPuzzleFail");
        
        SerializedProperty onCompleteLoad = serializedObject.FindProperty("onCompleteLoad");
        
        SerializedProperty tempSelectText = serializedObject.FindProperty("tempSelectText");
        SerializedProperty tempEmptyText = serializedObject.FindProperty("tempEmptyText");
        
        SerializedProperty tempInts = serializedObject.FindProperty("tempInts");
        SerializedProperty tempBools = serializedObject.FindProperty("tempBools");
        SerializedProperty tempVects = serializedObject.FindProperty("tempVects");
        
        SerializedProperty rotateModulesTemp = serializedObject.FindProperty("rotateModulesTemp");
        SerializedProperty waveModulesTemp = serializedObject.FindProperty("waveModulesTemp");
        SerializedProperty weightModulesTemp = serializedObject.FindProperty("weightModulesTemp");
        
        SerializedProperty puzzlerHolders = serializedObject.FindProperty("puzzlerHolders");
        
        if(puzzlerHand.puzzlerTabs == 0){
        
            EditorGUILayout.Space();
            
            puzzlerHand.genOpts = GUILayout.Toggle(puzzlerHand.genOpts, "General Options", GUI.skin.button);
            
            if(puzzlerHand.genOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "The type of puzzle to be used." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(puzzleTypeRef, true);
                
                if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential){
                
                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Checks how the puzzle should be reset." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(resetType, true);
                
                }//puzzleType = sequential
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Checks how the puzzle should be solved/completed." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(completeTypeRef, true);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Checks how the puzzle should be locked when solved/complete." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(lockType, true);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Saves/loads the current state of the puzzle." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(saveState, true);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Uses a delay before calling complete if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerHand.useCompleteDelay = EditorGUILayout.Toggle("Use Complete Delay?", puzzlerHand.useCompleteDelay);
                
                if(puzzlerHand.useCompleteDelay){
                
                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The wait time before complete is called." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    puzzlerHand.completeDelay = EditorGUILayout.FloatField("Complete Delay", puzzlerHand.completeDelay);
                
                }//useCompleteDelay
                
            }//genOpts
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
            
                EditorGUILayout.Space();

                puzzlerHand.itemOpts = GUILayout.Toggle(puzzlerHand.itemOpts, "Item Options", GUI.skin.button);

                if(puzzlerHand.itemOpts){

                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem){

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "The item used for interacting with this puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(itemID, true);

                    }//puzzleType = solo
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Items are handled in the Slots Options area." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//puzzleType = solo
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Items are handled in the Weight Options area." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//puzzleType = weight
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced){

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Checks if an item is required to interact with the puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                        
                        puzzlerHand.requireItem = EditorGUILayout.Toggle("Require Item?", puzzlerHand.requireItem);
                        
                        if(puzzlerHand.requireItem){
                        
                            EditorGUILayout.Space();
                        
                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "The item used for interacting with this puzzle." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(itemID, true);
                        
                        }//requireItem

                    }//puzzleType = sequence

                }//itemOpts
            
            }//puzzleType = solo or sequence
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate){
            
                EditorGUILayout.Space();

                puzzlerHand.rotOpts = GUILayout.Toggle(puzzlerHand.rotOpts, "Rotate Options", GUI.skin.button);

                if(puzzlerHand.rotOpts){
                
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The amount the object will be rotated when interacted with." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    puzzlerHand.rotateAmount = EditorGUILayout.FloatField("Rotate Amount", puzzlerHand.rotateAmount);
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Plays a sound when object is rotated if TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    EditorGUILayout.PropertyField(rotateType, true);
                
                }//rotOpts

            }//puzzleType = rotate

            EditorGUILayout.Space();
            
            puzzlerHand.objOpts = GUILayout.Toggle(puzzlerHand.objOpts, "Objective Options", GUI.skin.button);
            
            if(puzzlerHand.objOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Pushes an update call to Objectives if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerHand.showObjectiveUpdate = EditorGUILayout.Toggle("Show Objective Updates?", puzzlerHand.showObjectiveUpdate);
                
                if(puzzlerHand.showObjectiveUpdate){
                
                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "The ID of the objective to call to." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    puzzlerHand.objectiveID = EditorGUILayout.IntField("Objective ID", puzzlerHand.objectiveID);
                    
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Uses a delay before objective update is pushed iF TRUE." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    puzzlerHand.objectiveUpdateDelay = EditorGUILayout.Toggle("Update Delayed?", puzzlerHand.objectiveUpdateDelay);
                    
                    if(puzzlerHand.objectiveUpdateDelay){
                    
                        if(showTips){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "The wait time before objective update is pushed." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                    
                        puzzlerHand.objectiveUpdateWait = EditorGUILayout.FloatField("Update Wait", puzzlerHand.objectiveUpdateWait);
                    
                    }//objectiveUpdateDelay
                
                }//showObjectiveUpdate
                
            }//objOpts
            
            EditorGUILayout.Space();

            puzzlerHand.interactOpts = GUILayout.Toggle(puzzlerHand.interactOpts, "Interaction Options", GUI.skin.button);

            if(puzzlerHand.interactOpts){
                
                if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
                        
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){

                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Checks how the player will interact with the puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(interactTypeRef, true);
                        
                        if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
                        
                            EditorGUILayout.PropertyField(itemUseType, true);

                        }//puzzleType = solo or sequence
                        
                        if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight && puzzlerHand.interactType == Puzzler_Handler.Interact_Type.AutoDetect){

                            EditorGUILayout.Space();

                            EditorGUILayout.HelpBox("\n" + "Auto Detect cannot be used with Multi-Item puzzle types." + "\n", MessageType.Error);

                            EditorGUILayout.Space();

                        //puzzleType = multi items & interactType = auto detect
                        } else {

                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Text to be used when displaying inventory select." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(selectText, true);

                            if(showTips){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Text to be used when player does not have the item(s) required." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(emptyText, true);
                            
                            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
                            
                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Text to be used when the weight module is full." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(fullText, true);
                            
                            }//puzzleType = weight

                        }//puzzleType = multi items & interactType = auto detect
                        
                    }//puzzleType = solo or multi item
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced){
                        
                        if(puzzlerHand.requireItem){
                        
                            EditorGUILayout.Space();

                            if(showTips){

                                EditorGUILayout.HelpBox("\n" + "Checks how the player will interact with the puzzle." + "\n", MessageType.Info);

                                EditorGUILayout.Space();

                            }//showTips

                            EditorGUILayout.PropertyField(interactTypeRef, true);
                            EditorGUILayout.PropertyField(itemUseType, true);

                            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems && puzzlerHand.interactType == Puzzler_Handler.Interact_Type.AutoDetect){

                                EditorGUILayout.Space();

                                EditorGUILayout.HelpBox("\n" + "Auto Detect cannot be used with Multi-Item puzzle types." + "\n", MessageType.Error);

                                EditorGUILayout.Space();

                            //puzzleType = multi items & interactType = auto detect
                            } else {

                                EditorGUILayout.Space();

                                if(showTips){

                                    EditorGUILayout.HelpBox("\n" + "Text to be used when displaying inventory select." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(selectText, true);

                                if(showTips){

                                    EditorGUILayout.Space();

                                    EditorGUILayout.HelpBox("\n" + "Text to be used when player does not have the item(s) required." + "\n", MessageType.Info);

                                    EditorGUILayout.Space();

                                }//showTips

                                EditorGUILayout.PropertyField(emptyText, true);

                            }//puzzleType = multi items & interactType = auto detect
                            
                        }//requireItem
                        
                    }//puzzleType = sequence or rotate
                    
                }//puzzleType = solo, multi items, sequence or rotate
                    
                EditorGUILayout.Space();
                    
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Plays a sound when the puzzle is interacted with if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips

                puzzlerHand.useInteractSound = EditorGUILayout.Toggle("Use Interact Sound?", puzzlerHand.useInteractSound);

                if(puzzlerHand.useInteractSound){
                    
                    if(showTips){

                        EditorGUILayout.Space();

                        EditorGUILayout.HelpBox("\n" + "Name of the sound in Sound Options > Sound Library" + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips

                    EditorGUILayout.PropertyField(interactSound, true);

                }//useInteractSound

            }//interactOpts
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Wave){
            
                EditorGUILayout.Space();

                puzzlerHand.slotsOpts = GUILayout.Toggle(puzzlerHand.slotsOpts, "Slots Options", GUI.skin.button);

                if(puzzlerHand.slotsOpts){

                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem){

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Prefab instantiated when item is used/placed." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(soloPrefab, true);            

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Slots for using/placing items." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(soloSlots, true);

                    }//puzzleType = solo

                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems){

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Prefabs of items which are allowed to be used with this puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(multiPrefabs, true);

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Slots for setting order, item, etc. used for puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(multiSlots, true);

                    }//puzzleType = multi
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate){

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Slots used for different layers of the puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                        
                        EditorGUILayout.PropertyField(rotateSlots, true);
                        
                    }//puzzleType = rotate
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced){

                        EditorGUILayout.Space();
                        
                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Slots used for different layers of the puzzle." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips
                        
                        EditorGUILayout.PropertyField(rotateModules, true);
                        
                    }//puzzleType = rotate
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Wave){
                    
                        EditorGUILayout.Space();

                        if(showTips){

                            EditorGUILayout.HelpBox("\n" + "Slots used for modules, waves, wave checks, etc. settings." + "\n", MessageType.Info);

                            EditorGUILayout.Space();

                        }//showTips

                        EditorGUILayout.PropertyField(moduleSlots, true);
                    
                    }//puzzleType == wave

                }//slotsOpts
            
            }//puzzleType = solo or multi items
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential){
            
                EditorGUILayout.Space();

                puzzlerHand.seqOpts = GUILayout.Toggle(puzzlerHand.seqOpts, "Sequence Options", GUI.skin.button);

                if(puzzlerHand.seqOpts){
                
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The correct sequence order for solving/completing the puzzle." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(sequenceOrder, true);
                    
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Slots used for sequence events, etc." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    EditorGUILayout.PropertyField(sequence, true);
                
                }//seqOpts
            
            }//puzzleType = sequential
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Lights){
            
                EditorGUILayout.Space();

                puzzlerHand.lightOpts = GUILayout.Toggle(puzzlerHand.lightOpts, "Light Options", GUI.skin.button);

                if(puzzlerHand.lightOpts){
                
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The correct lights active for solving/completing the puzzle." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(lightsActive, true);
                    
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Slots used for intearctive light checks, etc." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    EditorGUILayout.PropertyField(lights, true);
                
                }//lightOpts
            
            }//puzzleType = lights
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Switches){
            
                EditorGUILayout.Space();

                puzzlerHand.switchOpts = GUILayout.Toggle(puzzlerHand.switchOpts, "Switch Options", GUI.skin.button);

                if(puzzlerHand.switchOpts){
                
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The correct switches order for solving/completing the puzzle." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                
                    EditorGUILayout.PropertyField(switchesActive, true);
                    
                    EditorGUILayout.Space();
                    
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Slots used for dynamic object checks, etc." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                    
                    EditorGUILayout.PropertyField(switches, true);
                
                }//switchOpts
            
            }//puzzleType = switches
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
            
                EditorGUILayout.Space();

                puzzlerHand.weightOpts = GUILayout.Toggle(puzzlerHand.weightOpts, "Weight Options", GUI.skin.button);

                if(puzzlerHand.weightOpts){
                    
                    EditorGUILayout.Space();
                        
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "The items used for interacting with this puzzle." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                        
                    EditorGUILayout.PropertyField(weightItems, true);

                    EditorGUILayout.Space();
                        
                    if(showTips){

                        EditorGUILayout.HelpBox("\n" + "Slots used for weight settings per module." + "\n", MessageType.Info);

                        EditorGUILayout.Space();

                    }//showTips
                        
                    EditorGUILayout.PropertyField(weightModules, true);
                        
                }//weightOpts
                        
            }//puzzleType = weight
            
            EditorGUILayout.Space();
            
            puzzlerHand.soundOpts = GUILayout.Toggle(puzzlerHand.soundOpts, "Sound Options", GUI.skin.button);
            
            if(puzzlerHand.soundOpts){
            
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Plays sounds when sound actions are called if TRUE." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                puzzlerHand.useSounds = EditorGUILayout.Toggle("Use Sounds?", puzzlerHand.useSounds);
                
                EditorGUILayout.Space();
                
                if(showTips){

                    EditorGUILayout.HelpBox("\n" + "Source the sounds are played from." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(soundSource, true);
                
                if(showTips){
                
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("\n" + "Library of sounds and settings which can be called to at any time." + "\n", MessageType.Info);

                    EditorGUILayout.Space();

                }//showTips
                
                EditorGUILayout.PropertyField(soundLibrary, true);
                
            }//soundOpts
             
        }//puzzlerTabs = user options
        
        if(puzzlerHand.puzzlerTabs == 1){
        
            EditorGUILayout.Space();
            
            if(showTips){

                EditorGUILayout.HelpBox("\n" + "Events triggered on puzzle complete, fail, etc.." + "\n", MessageType.Info);

                EditorGUILayout.Space();

            }//showTips
            
            EditorGUILayout.PropertyField(onPuzzleComplete, true);
            
            EditorGUILayout.Space();
            
            puzzlerHand.useDelayedEvent = EditorGUILayout.Toggle("Use Delayed Event?", puzzlerHand.useDelayedEvent);
            puzzlerHand.delayEventWait = EditorGUILayout.FloatField("Delayed Event Wait", puzzlerHand.delayEventWait);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(onPuzzleCompleteDelayed, true);
            
            if(puzzlerHand.completeType == Puzzler_Handler.Complete_Type.Manual | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential){
            
                EditorGUILayout.Space();
            
                EditorGUILayout.PropertyField(onPuzzleFail, true);
        
            }//completeType = manual or puzzleType = sequential
            
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(onCompleteLoad, true);
        
        }//puzzlerTabs = events
        
        if(puzzlerHand.puzzlerTabs == 2){
        
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Automatic Values", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.Space();
            
            if(showTips){
                
                EditorGUILayout.HelpBox("\n" + "These values are automatically handled by the system." + "\n", MessageType.Info);
                    
                EditorGUILayout.Space();
                
            }//showTips
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
            
                if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems){
                
                    puzzlerHand.itemCount = EditorGUILayout.IntField("Item Count", puzzlerHand.itemCount);
                    puzzlerHand.tempItemCount = EditorGUILayout.IntField("Temp Item Count", puzzlerHand.tempItemCount);

                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(tempSelectText, true);
                    EditorGUILayout.PropertyField(tempEmptyText, true);

                    EditorGUILayout.Space();
                
                }//puzzleType = solo or multi
                
                if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
                
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate){
                    
                        if(puzzlerHand.requireItem){

                            puzzlerHand.tempItemCount = EditorGUILayout.IntField("Temp Item Count", puzzlerHand.tempItemCount);

                            EditorGUILayout.Space();

                            EditorGUILayout.PropertyField(tempSelectText, true);
                            EditorGUILayout.PropertyField(tempEmptyText, true);

                            EditorGUILayout.Space();

                        }//requireItem
                    
                    }//puzzleType = sequential or rotate
                    
                    if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
                    
                        puzzlerHand.tempItemCount = EditorGUILayout.IntField("Temp Item Count", puzzlerHand.tempItemCount);

                        EditorGUILayout.Space();

                        EditorGUILayout.PropertyField(tempSelectText, true);
                        EditorGUILayout.PropertyField(tempEmptyText, true);

                        EditorGUILayout.Space();
                    
                    }//puzzleType = weight
                
                }//puzzleType = sequential, rotate or weight

            }//puzzleType = solo or multi item
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential){
            
                EditorGUILayout.PropertyField(tempInts, true);
                
                EditorGUILayout.Space();
            
            }//puzzleType = multi items or sequential
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate){
            
                puzzlerHand.tempRot = EditorGUILayout.FloatField("Temp Rotation", puzzlerHand.tempRot);
                
                EditorGUILayout.PropertyField(tempVects, true);
                
                EditorGUILayout.Space();
            
            }//puzzleType = rotate
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced){
            
                puzzlerHand.tempRot = EditorGUILayout.FloatField("Temp Rotation", puzzlerHand.tempRot);
                
                EditorGUILayout.Space();
                
                EditorGUILayout.PropertyField(rotateModulesTemp, true);
                
                EditorGUILayout.Space();
            
                puzzlerHand.modulesCount = EditorGUILayout.IntField("Modules Count", puzzlerHand.modulesCount);
                puzzlerHand.modulesActive = EditorGUILayout.IntField("Modules Active", puzzlerHand.modulesActive);
                
                EditorGUILayout.Space();
            
            }//puzzleType = rotate advanced
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Lights | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Switches){
            
                EditorGUILayout.PropertyField(tempBools, true);
                
                EditorGUILayout.Space();
            
            }//puzzleType = solo item, lights or switches
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
            
                EditorGUILayout.PropertyField(weightModulesTemp, true);
                
                EditorGUILayout.Space();
            
            }//puzzleType = lights or switches
            
            if(puzzlerHand.puzzleType != Puzzler_Handler.Puzzle_Type.Wave){
            
                puzzlerHand.currentSlot = EditorGUILayout.IntField("Current Slot", puzzlerHand.currentSlot);
            
            }//puzzleType != wave
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Wave){

                EditorGUILayout.Space();
                
                puzzlerHand.moduleSlot = EditorGUILayout.IntField("Module Slot", puzzlerHand.moduleSlot);
                puzzlerHand.waveSlot = EditorGUILayout.IntField("Wave Slot", puzzlerHand.waveSlot);
                puzzlerHand.waveCheckSlot = EditorGUILayout.IntField("WaveCheck Slot", puzzlerHand.waveCheckSlot);
                
                EditorGUILayout.Space();
                
                EditorGUILayout.PropertyField(waveModulesTemp, true);
                
                EditorGUILayout.Space();
            
                puzzlerHand.modulesActive = EditorGUILayout.IntField("Modules Active", puzzlerHand.modulesActive);
                puzzlerHand.wavesActive = EditorGUILayout.IntField("Waves Active", puzzlerHand.wavesActive);    
            
            }//puzzleType = wave
            
            puzzlerHand.activeCount = EditorGUILayout.IntField("Active Count", puzzlerHand.activeCount);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(puzzlerHolders, true);
            
            EditorGUILayout.Space();
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.SoloItem |puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems |puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Rotate | puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.RotateAdvanced){
            
                puzzlerHand.present = EditorGUILayout.Toggle("Item Present?", puzzlerHand.present);
            
            }//puzzleType = solo, multi, sequential or rotate
            
            puzzlerHand.complete = EditorGUILayout.Toggle("Complete?", puzzlerHand.complete);
            
            if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Sequential){
            
                puzzlerHand.fail = EditorGUILayout.Toggle("Fail?", puzzlerHand.fail);
            
            }//puzzleType = sequential
            
            puzzlerHand.locked = EditorGUILayout.Toggle("Locked?", puzzlerHand.locked);
        
        }//puzzlerTabs = auto
        
        EditorGUILayout.Space();
        
        if(EditorGUI.EndChangeCheck()){

            serializedObject.ApplyModifiedProperties();

        }//EndChangeCheck
  
        if(GUI.changed){
            
            EditorUtility.SetDirty(puzzlerHand);
            
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
