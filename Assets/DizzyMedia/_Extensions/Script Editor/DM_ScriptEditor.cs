#if UNITY_EDITOR

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.IO;

public class DM_ScriptEditor : EditorWindow {

    [Serializable]
    public class Content_Saved {
        
        public string name;
        public string editName;
        public string path;
        public string content;
        public bool present;
        
    }//Content_Saved
    
    private static DM_ScriptEditor window;
    private static Vector2 windowsSize = new Vector2(400, 550);
    
    public DM_ScriptEdit_Library editLibrary;
    
    public string scriptPath;
    public string scriptContent;
    
    private static DM_Version dmVersion;
    private static string versionName = "ScriptEditor Version";
    private static string verNumb = "";
    private static bool versionCheckStatic = false;
    
    public static DM_InternEnums.Language language;
    private static DM_MenusLocData dmMenusLocData;
    private static string menusLocDataName = "DM_M_Data";
    private static int menusLocDataSlot;
    private static bool languageLock = false; 
    
    public List<Content_Saved> saved = new List<Content_Saved>();
    
    Vector2 scrollPos;
    public bool editsDone;
    public bool savedChecked;
    private bool barShowing = false;
    
    [MenuItem("Tools/Dizzy Media/Extensions/Script Editor", false , 31)]
    private static void OpenWizard() {

        if(dmVersion == null){
        
            versionCheckStatic = false;
            Version_FindStatic();
        
        //dmVersion == null
        } else {
        
            verNumb = dmVersion.version;

            window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
            window.maxSize = window.minSize = windowsSize;
        
        }//dmVersion == null
        
        if(dmMenusLocData == null){
            
            languageLock = false;
            DM_LocDataFind();
        
        //dmMenusLocData = null
        } else {
        
            language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;
        
        }//dmMenusLocData = null
        
    }//OpenWizard
    
    public void OpenWizard_Single(){
        
        OpenWizard();
        
    }//OpenWizard_Single

    private void OnGUI() {
        
        ScriptEdit_Screen();
            
    }//OnGUI
    
    
//////////////////////////////////////
///
///     EDITOR DISPLAY
///
///////////////////////////////////////

    
    private void ScriptEdit_Screen(){
        
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            
        Texture t0 = (Texture)Resources.Load("EditorContent/ScriptEditor/ScriptEditor_Header");
        
        var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            
        GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(64));
            
        EditorGUI.BeginChangeCheck();
        
        ScriptableObject target = this;
        SerializedObject soTar = new SerializedObject(target);
        
        SerializedProperty editLibraryRef = soTar.FindProperty("editLibrary");
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        
        language = (DM_InternEnums.Language)EditorGUILayout.EnumPopup("Language", language); 
        
        if(dmMenusLocData != null){
                
            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[0].local)) {

                Language_Save();

            }//Button
        
        }//dmMenusLocData != null

        EditorGUILayout.EndHorizontal();
        
        if(dmMenusLocData != null){
        
            if(verNumb == "Unknown"){

                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[0].texts[0].text, MessageType.Error);

            //verNumb == "Unknown"
            } else {
                
                EditorGUILayout.Space();

                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Info);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(editLibraryRef, true);

                EditorGUILayout.Space();

                if(editLibrary != null){

                    EditorGUILayout.HelpBox("\n" + editLibrary.content.name + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[2].local + "\n", MessageType.Info);

                //editLibrary != null
                } else {

                    EditorGUILayout.HelpBox("\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local + "\n", MessageType.Error);

                    if(saved.Count > 0){

                        Notifications_Clear();

                    }//saved.Count > 0

                }//editLibrary != null

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                if(saved.Count > 0){

                    if(!savedChecked){

                        for(int s = 0; s < saved.Count; ++s ) {

                            if(!saved[s].present){

                                EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[3].local + "\n", MessageType.Info);

                            //!present
                            } else {

                                EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[4].local + "\n", MessageType.Info);

                            }//!present

                        }//for s saved

                    //!savedChecked
                    } else {

                        for(int s = 0; s < saved.Count; ++s ) {

                            if(!saved[s].present){

                                EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[5].local + "\n", MessageType.Error);

                            //!present
                            } else {

                                EditorGUILayout.HelpBox("\n" + saved[s].name + " > " + saved[s].editName + " |" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[6].local + "\n", MessageType.Info);

                            }//!present

                        }//for s saved

                    }//!savedChecked

                //saved.Count > 0
                } else {

                    EditorGUILayout.HelpBox("\n" + dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[1].local + "\n", MessageType.Warning);

                }//saved.Count > 0

                EditorGUILayout.EndScrollView();

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                EditorGUILayout.BeginHorizontal();

                if(editLibrary != null){

                    if(editsDone){

                        GUI.enabled = false;

                    //editsDone
                    } else {

                        GUI.enabled = true;

                    }//editsDone

                //editLibrary != null
                } else {

                    GUI.enabled = false;

                }//editLibrary != null

                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                    if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].prompts[0].buttons[1].local)){

                        Scipts_Update();

                    }//DisplayDialog

                }//Button

                if(saved.Count > 0){

                    if(editsDone){

                        GUI.enabled = true;

                    //editsDone
                    } else {

                        GUI.enabled = false;

                    }//editsDone

                //saved.Count > 0
                } else {

                    GUI.enabled = false;

                }//saved.Count > 0

                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                    Scripts_Check();

                }//Button

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();

                if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[3].local)){

                    Notifications_Clear();

                }//Button

                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space();
                
            }//verNumb == "Unknown"
        
        //dmMenusLocData != null 
        } else {
        
            if(!languageLock){
                
                DM_LocDataFind();
            
            }//!languageLock 
        
        }//dmMenusLocData != null
        
        if(!EditorApplication.isCompiling){
    
            if(editsDone && barShowing){

                barShowing = false;
                EditorUtility.ClearProgressBar();

            }//defineChanged & barShowing

        }//isCompiling
        
        if(EditorGUI.EndChangeCheck()){

            soTar.ApplyModifiedProperties();

        }//EndChangeCheck
        
    }//ScriptEdit_Screen
    
    
//////////////////////////////////////
///
///     EDITOR ACTIONS
///
///////////////////////////////////////
    
    
    private void Scipts_Update(){
        
        int countLib = 0;
        int countTemp = 0;
        int countSave = 0;
        
        saved = new List<Content_Saved>();
        editsDone = false;
        
        if(editLibrary != null){
            
            if(editLibrary.content.library.Count > 0){
                
                for(int l = 0; l < editLibrary.content.library.Count; ++l ) {
                    
                    countLib += 1;
                    countTemp = 0;
                    
                    if(editLibrary.content.library[l].template != null){
                        
                        scriptPath = File_Find(editLibrary.content.library[l].name);
                        
                        if(scriptPath != ""){
                        
                            scriptContent = File.ReadAllText(scriptPath);

                            for(int c = 0; c < editLibrary.content.library[l].template.content.Count; ++c ) {

                                countTemp += 1;

                                if(!scriptContent.Contains(editLibrary.content.library[l].template.content[c].edit)){

                                    if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].original)){

                                        scriptContent = scriptContent.Replace(editLibrary.content.library[l].template.content[c].original, editLibrary.content.library[l].template.content[c].edit);

                                        Content_Saved tempSave = new Content_Saved();

                                        tempSave.name = editLibrary.content.library[l].name;
                                        tempSave.editName = editLibrary.content.library[l].template.content[c].name;
                                        tempSave.path = scriptPath;
                                        tempSave.content = scriptContent;

                                        if(!saved.Contains(tempSave)){

                                            saved.Add(tempSave);

                                        }//!Contains

                                        editsDone = true;

                                    }//contains original
                                    
                                //!contains edit
                                } else {
                                    
                                    Content_Saved tempSave = new Content_Saved();
                                    
                                    tempSave.name = editLibrary.content.library[l].name;
                                    tempSave.editName = editLibrary.content.library[l].template.content[c].name;
                                    tempSave.present = true;

                                    if(!saved.Contains(tempSave)){

                                        saved.Add(tempSave);

                                    }//!Contains
                                    
                                }//!contains edit

                                //Debug.Log("countLib = " + countLib);
                                //Debug.Log("countTemp = " + countTemp);

                                if(countLib == editLibrary.content.library.Count && countTemp == editLibrary.content.library[l].template.content.Count){

                                    for(int s = 0; s < saved.Count; ++s ) {

                                        if(!saved[s].present){
                                            
                                            File.WriteAllText(saved[s].path, saved[s].content);

                                        }//!present
                                        
                                        countSave += 1;

                                        if(countSave == saved.Count){
                                        
                                            EditorUtility.DisplayProgressBar("Compiling", "Updating Scripts...", 0.5f);

                                            barShowing = true;
                                            editsDone = true;

                                            AssetDatabase.Refresh();

                                        }//countSave = saved.Count

                                    }//for s saved

                                }//count = content.Count

                            }//for c content
                            
                        }//scriptPath != ""
                        
                    //template != null
                    } else {
                     
                        Debug.Log("Template missing = " + editLibrary.content.library[l].name);
                        
                    }//template != null
                    
                }//for l library
                
            }//library.Count > 0
            
        }//editLibrary != null
        
    }//Scipts_Update
    
    private void Scripts_Check(){
        
        if(editLibrary != null){
            
            if(editLibrary.content.library.Count > 0){
                
                for(int l = 0; l < editLibrary.content.library.Count; ++l ) {
                    
                    if(editLibrary.content.library[l].template != null){
                     
                        scriptPath = File_Find(editLibrary.content.library[l].name);
                        
                        if(scriptPath != ""){
                        
                            scriptContent = File.ReadAllText(scriptPath);
                            
                            for(int c = 0; c < editLibrary.content.library[l].template.content.Count; ++c ) {
                                
                                if(scriptContent.Contains(editLibrary.content.library[l].template.content[c].edit)){
                                    
                                    for(int s = 0; s < saved.Count; ++s ) {

                                        if(saved[s].name == editLibrary.content.library[l].name){

                                            saved[s].present = true;

                                        }//name = name

                                    }//for s saved
                                    
                                //contains edit
                                } else {
                                    
                                    for(int s = 0; s < saved.Count; ++s ) {

                                        if(saved[s].name == editLibrary.content.library[l].name){

                                            saved[s].present = false;

                                        }//name = name

                                    }//for s saved
                                    
                                }//contains edit
                                
                            }//for c content
                            
                        }//scriptPath != ""
                        
                    }//template != null
                    
                }//for l library
                
            }//library.Count > 0
            
        }//editLibrary != null
        
        savedChecked = true;
        
    }//Scripts_Check
    
    private void Notifications_Clear(){
        
        saved = new List<Content_Saved>();    
        editsDone = false;
        savedChecked = false;
        
    }//Notifications_Clear
    
    
//////////////////////////////////////
///
///     LANGUAGE ACTIONS
///
//////////////////////////////////////

    
    public static void DM_LocDataFind(){
    
        if(dmMenusLocData == null){
        
            //Debug.Log("Find Start");
        
            //AssetDatabase.Refresh();

            string[] results;
            DM_MenusLocData tempMenusLocData = ScriptableObject.CreateInstance<DM_MenusLocData>();

            results = AssetDatabase.FindAssets(menusLocDataName);

            if(results.Length > 0){

                foreach(string guid in results){

                    if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                        tempMenusLocData = AssetDatabase.LoadAssetAtPath<DM_MenusLocData>(AssetDatabase.GUIDToAssetPath(guid));
                        
                        if(tempMenusLocData != null){
                        
                            dmMenusLocData = tempMenusLocData;
                            
                            if(dmMenusLocData != null){

                                if(!languageLock){

                                    languageLock = true;

                                    Language_Check();

                                }//!languageLock

                            }//dmMenusLocData != null

                        }//tempMenusLocData != null

                        //Debug.Log("Menus Loc Data Found");

                    }//file.exists

                }//foreach guid

            }//results.Length > 0
        
        //dmMenusLocData = null
        } else {
        
            if(!languageLock){
        
                languageLock = true;
        
                language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;
            
            }//!languageLock
        
        }//dmMenusLocData = null
    
    }//DM_LocDataFind
    
    public static void Language_Check(){
    
        if(dmMenusLocData != null){

            for(int d = 0; d < dmMenusLocData.dictionary.Count; d++){
                
                if(dmMenusLocData.dictionary[d].asset == "Script Editor"){
                    
                    menusLocDataSlot = d;
                        
                    //Debug.Log("Loc Data Slot = " + menusLocDataSlot);
                        
                }//asset = IWC
                
            }//for d dictionary
            
            language = (DM_InternEnums.Language)(int)dmMenusLocData.currentLanguage;
            
        }//dmMenusLocData != null
            
    }//Language_Check
    
    public void Language_Save(){
    
        if(dmMenusLocData != null){

            if((int)dmMenusLocData.currentLanguage != (int)language){

                dmMenusLocData.currentLanguage = (DM_InternEnums.Language)(int)language;

            }//currentLanguage != language

        }//dmMenusLocData != null
        
        Debug.Log("Language Saved");
    
    }//Language_Save


//////////////////////////////////////
///
///     VERSION ACTIONS
///
//////////////////////////////////////

    
    public static void Version_FindStatic(){
        
        if(!versionCheckStatic){

            versionCheckStatic = true;

            AssetDatabase.Refresh();

            string[] results;
            DM_Version tempVersion = ScriptableObject.CreateInstance<DM_Version>();

            results = AssetDatabase.FindAssets(versionName);

            if(results.Length > 0){

                foreach(string guid in results){

                    if(File.Exists(AssetDatabase.GUIDToAssetPath(guid))){

                        tempVersion = AssetDatabase.LoadAssetAtPath<DM_Version>(AssetDatabase.GUIDToAssetPath(guid));

                        if(tempVersion != null){
                                
                            dmVersion = tempVersion;
                            verNumb = dmVersion.version;
                                
                            window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
                            window.maxSize = window.minSize = windowsSize;
                                
                            //Debug.Log("Script Editor Version found");

                        //tempVersion != null
                        } else {
                            
                            if(verNumb == ""){
                            
                                verNumb = "Unknown";

                            }//verNumb = null
                            
                            window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
                            window.maxSize = window.minSize = windowsSize;
                                
                            //Debug.Log("Puzzler Version NOT found");
                                
                        }//tempVersion != null
                            
                    //Exists
                    } else {

                        //Debug.Log("Script Editor Version NOT found"); 

                    }//Exists

                }//foreach guid

            //results.Length > 0
            } else {
                            
                verNumb = "Unknown";
                            
                window = GetWindow<DM_ScriptEditor>(false, "Script Editor" + " v" + verNumb, true);
                window.maxSize = window.minSize = windowsSize;
                
            }//results.Length > 0
            
        }//!versionCheckStatic
        
    }//Version_FindStatic
    
    
//////////////////////////////////////
///
///     EXTRA ACTIONS
///
///////////////////////////////////////
    
    
    public string File_Find(string fileName){
        
        string[] results = new string[0];
        
        results = AssetDatabase.FindAssets(fileName + " t:script");
        
        if(results.Length > 0){
            
            UnityEngine.Object[] scripts = new UnityEngine.Object[results.Length];
            
            for(int r = 0; r < results.Length; r++) {
                    
                scripts[r] = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(results[r]), typeof(UnityEngine.Object));
            
            }//for r results
            
            if(scripts.Length > 0){
                
                for(int s = 0; s < scripts.Length; s++) {
                 
                    if(scripts[s].name == fileName){
                        
                        //Debug.Log(scripts[s].name + " Found!");
                        
                        return AssetDatabase.GUIDToAssetPath(results[s]);
                        
                    //name = tempName
                    } else {
                        
                        //Debug.Log(scripts[s].name + " Not Correct File Found!");
                        
                    }//name = tempName
                    
                }//for s scripts
                
            }//scripts.Length > 0
            
        //results > 0
        } else {
        
            Debug.Log(fileName + " Not Found!");
        
        }//results > 0
        
        return "";
        
    }//File_Find
    
	private void OnDestroy() {
        
        window = null;
        verNumb = "";
        
        if(barShowing){
        
            barShowing = false;
            EditorUtility.ClearProgressBar();
            
        }//barShowing
	
    }//OnDestroy

}//DM_ScriptEditor

#endif
