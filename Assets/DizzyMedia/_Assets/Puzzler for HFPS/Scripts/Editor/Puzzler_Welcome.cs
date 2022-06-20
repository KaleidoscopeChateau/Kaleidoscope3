using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;
using System.IO;

[InitializeOnLoad]
public class Puzzler_Welcome : EditorWindow {


//////////////////////////////////////
///
///     INTERNAL VALUES
///
///////////////////////////////////////

        
    private static Puzzler_Welcome window;
    private static Vector2 windowsSize = new Vector2(530, 520f);
    
    private const string isShowAtStartEditorPrefs = "Puzzler_WelcomeStart";
    public static bool showOnStart = true;
    private static bool isInited;
    
    private static DM_Version dmVersion;
    private static string versionName = "Puzzler Version";
    private static string verNumb = "";
    private static bool versionCheckStatic = false;
    
    public static DM_InternEnums.Language language;
    private static DM_MenusLocData dmMenusLocData;
    private static string menusLocDataName = "DM_M_Data";
    private static int menusLocDataSlot;
    private static bool languageLock = false; 
    
    private string fileDocs = "Puzzler Documentation";
    
    private int puzzlerTabs;
    
    private string currBuildSettings = "";
    private string defineSymb = "PUZZLER_PRESENT";
    private bool defineChanged = false;
    private bool barShowing = false;
    
    Vector2 scrollPos;
    Vector2 scrollPos2;
    
    
//////////////////////////////////////
///
///     SHOW AT START CHECKS
///
///////////////////////////////////////

    
	static Puzzler_Welcome() {
        
		EditorApplication.update -= GetShowAtStart;
		EditorApplication.update += GetShowAtStart;
	
    }//WelcomeScreen
    
	private static void GetShowAtStart() {
        
		EditorApplication.update -= GetShowAtStart;
		
        if(EditorPrefs.HasKey(isShowAtStartEditorPrefs)){
        
            showOnStart = EditorPrefs.GetBool(isShowAtStartEditorPrefs);
        
        //HasKey
        } else {
        
            showOnStart = true;
            EditorPrefs.SetBool(isShowAtStartEditorPrefs, showOnStart);
            
        }//HasKey

		if(showOnStart) {
            
			EditorApplication.update -= OpenAtStartup;
			EditorApplication.update += OpenAtStartup;
		
        }//showOnStart
        
	}//GetShowAtStart

	private static void OpenAtStartup() {
        
        OpenWizard();
        EditorApplication.update -= OpenAtStartup;

	}//OpenAtStartup
    
    
//////////////////////////////////////
///
///     EDITOR WINDOW
///
///////////////////////////////////////


    [MenuItem("Tools/Dizzy Media/Puzzler/Review Asset", false , 13)]
    public static void OpenReview() {
            
        Application.OpenURL("https://u3d.as/2Hn8#reviews");
        
    }//OpenReview
    
    [MenuItem("Tools/Dizzy Media/Puzzler/Puzzler Welcome", false , 12)]
    public static void OpenWizard() {

        if(dmVersion == null){
        
            versionCheckStatic = false;
            Version_FindStatic();
        
        //dmVersion == null
        } else {
        
            verNumb = dmVersion.version;

            window = GetWindow<Puzzler_Welcome>(false, "Puzzler" + " v" + verNumb, true);
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

    private void OnGUI() {
        
        Puzzler_WelcomeScreen();
            
    }//OnGUI


//////////////////////////////////////
///
///     EDITOR DISPLAY
///
///////////////////////////////////////

    
    public void Puzzler_WelcomeScreen(){

        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            
        Texture t0 = (Texture)Resources.Load("EditorContent/Puzzler-Logo");
        
        var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            
        GUILayout.Box(t0, style, GUILayout.ExpandWidth(true), GUILayout.Height(200));

        GUILayout.Space(10);
        
        if(dmMenusLocData != null){

            showOnStart = EditorGUILayout.Toggle(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].singleValues[0].local, showOnStart);
        
        }//dmMenusLocData != null
        
        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        
        language = (DM_InternEnums.Language)EditorGUILayout.EnumPopup("Language", language); 
        
        if(dmMenusLocData != null){
                
            if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[0].local)) {

                Language_Save();

            }//Button
        
        }//dmMenusLocData != null

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        if(dmMenusLocData != null){
        
            if(verNumb == "Unknown"){

                EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[0].texts[0].text, MessageType.Info);

            //verNumb == "Unknown"
            } else {

                puzzlerTabs = GUILayout.SelectionGrid(puzzlerTabs, new string[] { dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].local}, 2);

                EditorGUILayout.Space();

                if(puzzlerTabs == 0){
                
                    scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                    #if PUZZLER_PRESENT
                    
                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[1].text, MessageType.Info);
                    
                    #else
                    
                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[0].text, MessageType.Error);
                    
                    #endif

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].texts[2].text, MessageType.Info);
                    
                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    
                    EditorGUILayout.BeginHorizontal();

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[1].local)){

                        File_Find(fileDocs);

                    }//Button

                    GUILayout.Space(5);

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[1].buttons[2].local)){

                        OpenReview();

                    }//Button
                    
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.Space();

                }//puzzlerTabs = Welcome

                if(puzzlerTabs == 1){
                
                    scrollPos2 = GUILayout.BeginScrollView(scrollPos2, false, true, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                    
                    #if PUZZLER_PRESENT
                    
                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[1].text, MessageType.Info);
                    
                    #else
                    
                        EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[0].text, MessageType.Error);
                    
                    #endif

                    EditorGUILayout.HelpBox(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].texts[2].text, MessageType.Info);
                    
                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[0].local)){

                        Launch_ScriptEditor();

                    }//Button
                    
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    
                    #if PUZZLER_PRESENT
                    
                        GUI.enabled = false;
                    
                    #else
                    
                        if(defineChanged){
                        
                            GUI.enabled = false;
                    
                        //defineChanged
                        } else {
                        
                            GUI.enabled = true;
                        
                        }//defineChanged
                        
                    #endif

                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[1].local)) {

                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[0].buttons[1].local)){
                            
                            Symbol_Add(defineSymb);

                        }//DisplayDialog

                    }//Button
                    
                    GUILayout.Space(5);
                    
                    GUI.enabled = true;
                    
                    if(GUILayout.Button(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].buttons[2].local)){
                    
                        if(EditorUtility.DisplayDialog(dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].header, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].message, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].buttons[0].local, dmMenusLocData.dictionary[menusLocDataSlot].menuLoc.localization.languages[(int)language].windows[0].sections[2].prompts[1].buttons[1].local)){
                        
                            Gizmos_Move();
                        
                        }//DisplayDialog
                    
                    }//Button

                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.Space();

                }//puzzlerTabs = Setup

            }//verNumb == "Unknown"
        
        //dmMenusLocData != null 
        } else {
        
            if(!languageLock){
                
                DM_LocDataFind();
            
            }//!languageLock 
        
        }//dmMenusLocData != null
        
        if(!EditorApplication.isCompiling){
    
            if(defineChanged && barShowing){

                barShowing = false;
                EditorUtility.ClearProgressBar();

            }//defineChanged & barShowing

        }//isCompiling
    
    }//WelcomeScreen
    
    
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
                
                if(dmMenusLocData.dictionary[d].asset == "Puzzler"){
                    
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
                                
                            window = GetWindow<Puzzler_Welcome>(false, "Puzzler" + " v" + verNumb, true);
                            window.maxSize = window.minSize = windowsSize;
                                
                            //Debug.Log("Puzzler Version found");

                        //tempVersion != null
                        } else {
                            
                            if(verNumb == ""){
                            
                                verNumb = "Unknown";

                            }//verNumb = null
                            
                            window = GetWindow<Puzzler_Welcome>(false, "Puzzler " + verNumb, true);
                            window.maxSize = window.minSize = windowsSize;
                                
                            //Debug.Log("Puzzler Version NOT found");
                                
                        }//tempVersion != null
                            
                    //Exists
                    } else {

                        //Debug.Log("Puzzler Version NOT found"); 

                    }//Exists

                }//foreach guid

            //results.Length > 0
            } else {
                            
                verNumb = "Unknown";
                            
                window = GetWindow<Puzzler_Welcome>(false, "Puzzler " + verNumb, true);
                window.maxSize = window.minSize = windowsSize;
                
            }//results.Length > 0
            
        }//!versionCheckStatic
        
    }//Version_FindStatic
    
    
//////////////////////////////////////
///
///     LAUNCH ACTIONS
///
///////////////////////////////////////

    
    public void Launch_ScriptEditor(){

        DM_ScriptEditor window = (DM_ScriptEditor)EditorWindow.GetWindow<DM_ScriptEditor>(false, "Script Editor", true);
        window.OpenWizard_Single();

    }//Launch_ScriptEditor
    
    
//////////////////////////////////////
///
///     GIZMOS ACTIONS
///
///////////////////////////////////////

    
    public void Gizmos_Move(){
    
        if(!Directory.Exists("Assets/Gizmos/")){
                            
            AssetDatabase.CreateFolder("Assets", "Gizmos");

        }//!exists gizmos folder
                            
        FileUtil.MoveFileOrDirectory("Assets/DizzyMedia/Resources/Gizmos/Puzzler/Puzzler-Icon.png.meta", "Assets/Gizmos/Puzzler-Icon.png.meta");
        FileUtil.MoveFileOrDirectory("Assets/DizzyMedia/Resources/Gizmos/Puzzler/Puzzler-Icon.png", "Assets/Gizmos/Puzzler-Icon.png");

        AssetDatabase.Refresh();
                            
    }//Gizmos_Move
    
    
//////////////////////////////////////
///
///     SYMBOLS ACTIONS
///
///////////////////////////////////////

    
    private void Symbol_Add(string newSymbol){
    
        defineChanged = false;
        currBuildSettings = "";
        
        currBuildSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            
        if(!currBuildSettings.Contains(newSymbol)) {
            
            if(string.IsNullOrEmpty(currBuildSettings)) {
            
                currBuildSettings = newSymbol;
            
            //currBuildSettings IsNullOrEmpty
            } else {
                
                currBuildSettings += ";" + newSymbol;
                
            }//currBuildSettings IsNullOrEmpty
            
            defineChanged = true;
        
        //!Contains newSymbol
        } else {
            
            if(EditorUtility.DisplayDialog("Symbol Present", "The Define Symbol you are trying to add is already present. ", "Ok")){}
            
        }//!Contains newSymbol
            
        if(defineChanged) {
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currBuildSettings);
            
            EditorUtility.DisplayProgressBar("Compiling", "Activating Puzzler...", 0.5f);
            
            barShowing = true;
            
        }//defineChanged
        
    }//Symbol_Add


//////////////////////////////////////
///
///     EXTRAS
///
///////////////////////////////////////


    public void File_Find(string fileName){
        
        string[] results = new string[0];
        
        results = AssetDatabase.FindAssets(fileName);
        
        if(results.Length > 0){
            
            UnityEngine.Object[] objects = new UnityEngine.Object[results.Length];
            
            string[] paths = new string[results.Length];
            
            for(int i = 0; i < results.Length; i++) {
                
                paths[i] = AssetDatabase.GUIDToAssetPath(results[i]);
                
            }//for i results
            
            if(paths.Length > 0){
                
                for(int p = 0; p < paths.Length; p++) {
                    
                    objects[p] = AssetDatabase.LoadAssetAtPath(paths[p], typeof(UnityEngine.Object));
            
                }//for p paths
                
            }//paths.Length > 0
            
            if(objects.Length > 0){
                
                Selection.objects = objects;
                
                Debug.Log(fileName + " Found!");
            
            }//objects.Length > 0
            
        //results > 0
        } else {
        
            Debug.Log(fileName + " Not Found!");
        
        }//results > 0
        
    }//File_Find
    
    /*

    public void Puzzler_ImportPack(string packName){
        
        string[] results = new string[0];
        
        results = AssetDatabase.FindAssets(packName);
        
        if(results.Length > 0){
            
            foreach(string pack in results) {
                
                AssetDatabase.ImportPackage(AssetDatabase.GUIDToAssetPath(pack), true);
                
            }//foreach pack
            
        //results > 0
        } else {
        
            Debug.Log(packName + " Not Found!");
        
        }//results > 0
        
    }//Puzzler_ImportPack
    
    */
    
	private void OnDestroy() {
        
        window = null;
		EditorPrefs.SetBool(isShowAtStartEditorPrefs, showOnStart);
        
        verNumb = "";
        
        if(barShowing){
        
            barShowing = false;
            EditorUtility.ClearProgressBar();
            
        }//barShowing
	
    }//OnDestroy


}//Puzzler_Welcome
