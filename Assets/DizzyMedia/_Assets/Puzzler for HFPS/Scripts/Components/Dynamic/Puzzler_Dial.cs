using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using ThunderWire.Helpers;
using ThunderWire.Input;
using HFPS.Systems;

[AddComponentMenu("Dizzy Media/Puzzler/Components/Dynamic/Puzzler Dial")]
public class Puzzler_Dial : MonoBehaviour, ISaveable {
    
    
//////////////////////////////////////
///
///     CLASSES
///
///////////////////////////////////////
    
    
    [System.Serializable]
    public class Rotation_Slots {
        
        [Space]
        
        public Vector3 rotation;
        
        [Space]
        
        public UnityEvent onStart;
        public UnityEvent onRotate;
        
    }//Rotation_Slots
    
    
//////////////////////////////////////
///
///     ENUMS
///
///////////////////////////////////////
    
    
    public enum Type_Axis {
        
        AxisX,
        AxisY,
        AxisZ
    
    }//Type_Axis
    
    public enum RotateType {
        
        Continuous = 0,
        Slots = 1,
        
    }//Rotate_Type
    
    public enum SaveState {
    
        NotActive = 0,
        Active = 1,
        
    }//SaveState
    
    
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

////////////////////
///
///     START OPTIONS
///
////////////////////

    
    public bool setRotation;
    public Vector3 startRotation;
    
    
////////////////////
///
///     GENERAL OPTIONS
///
////////////////////

    
    public SaveState saveState;

    
////////////////////
///
///     INPUT OPTIONS
///
////////////////////
    
    
    public string useInput;
    
    
////////////////////
///
///     ROTATE OPTIONS
///
////////////////////
    
    
    public RotateType rotateType;
    public Type_Axis turnAxis = Type_Axis.AxisX;
    
    public float dialTurnSpeed;
    
    public int startSlot;
    public List<Rotation_Slots> rotationSlots;
    
    
////////////////////
///
///     SOUND OPTIONS
///
////////////////////
    

    public bool useSounds;
    public AudioClip[] dialTurnSounds;
    public float dialSoundAfter;
    public float m_Volume = 1;
    
    
///////////////////////////
///
///     EVENT
///
///////////////////////////


    public UnityEvent onInteract;
    
    public float lateDelay;
    public UnityEvent onInteractLate;
    
    
///////////////////////////
///
///     AUTO
///
///////////////////////////
    
    
    public bool canRotate;
    public bool useKeyPressed;
    public bool isHolding;
    public bool turnSound;
    
    public Vector3 curRotation;
    
    public int curSlot;
    
    public bool locked;
    
    public int tabs;
    
    public bool startOpts;
    public bool genOpts;
    public bool inputOpts;
    public bool rotateOpts;
    public bool soundOpts;
    
    private readonly RandomHelper rand = new RandomHelper();
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    

    void Start() {
        
        StartInit();
        
    }//Start
    
    public void StartInit(){

        if(rotateType == RotateType.Continuous){
            
            this.transform.localEulerAngles = startRotation;
        
        }//rotateType = continuous
        
        if(rotateType == RotateType.Slots){
            
            curSlot = startSlot;

            if(rotationSlots.Count > 0){

                this.transform.localEulerAngles = rotationSlots[curSlot].rotation;
                
                rotationSlots[curSlot].onStart.Invoke();

            }//rotationSlots.Count > 0
        
        }//rotateType = slots
        
        Rotate_State(false);
        
        isHolding = false;
        useKeyPressed = false;
        locked = false;

    }//StartInit

    
//////////////////////////////////////
///
///     UPDATE ACTIONS
///
///////////////////////////////////////
    

    void Update() {

        if(canRotate){
            
            if(InputHandler.InputIsInitialized) {
                
                useKeyPressed = InputHandler.ReadButton(useInput);
                
            }//InputIsInitialized
        
            if(useKeyPressed) {

                isHolding = true;

                curRotation = new Vector3(turnAxis == Type_Axis.AxisX ? -dialTurnSpeed : 0, turnAxis == Type_Axis.AxisY ? -dialTurnSpeed : 0, turnAxis == Type_Axis.AxisZ ? -dialTurnSpeed : 0);
                transform.Rotate(curRotation);

            //UseKey
            } else if(isHolding) {

                isHolding = false;

            }//isHolding
        
        }//canRotate 
            
        if(turnSound) {
            
            if(useSounds){
                
                StartCoroutine(DialSound_Buff());
            
            }//useSounds
            
            turnSound = false;
        
        }//turnSound
        
        if(!isHolding) {
            
            turnSound = true;
            Rotate_State(false);
        
        }//isHolding
        
    }//Update

    
//////////////////////////////////////
///
///     USE ACTIONS
///
///////////////////////////////////////
    
    
    public void UseObject() {
    
        onInteract.Invoke();
    
        if(rotateType == RotateType.Continuous){
         
            Rotate_State(true);
            
        }//rotateType = continuous
        
        if(rotateType == RotateType.Slots){
            
            Rotate_Check();
            
        }//rotateType = slots
        
        StopCoroutine("Interact_Delay");
        StartCoroutine("Interact_Delay");
        
    }//UseObject
    
    private IEnumerator Interact_Delay(){
    
        yield return new WaitForSeconds(lateDelay);
        
        onInteractLate.Invoke();
        
    }//Interact_Delay
    
    
//////////////////////////////////////
///
///     ROTATE ACTIONS
///
///////////////////////////////////////
    
    
    public void Rotate_State(bool state){
        
        canRotate = state;
        
    }//Rotate_State
    
    public void Rotate_Check(){
        
        if(useSounds){
            
            DialSound();
    
        }//useSounds
        
        if(rotationSlots.Count > 0){
            
            curSlot += 1;
            
            if(curSlot >= rotationSlots.Count){
                
                curSlot = 0;
                
            }//curSlot > rotationSlots.Count
            
            this.transform.localEulerAngles = rotationSlots[curSlot].rotation;
            
            rotationSlots[curSlot].onRotate.Invoke();
            
        }//rotationSlots.Count > 0
        
    }//Rotate_Check
    
    
//////////////////////////////////////
///
///     SLOT ACTIONS
///
///////////////////////////////////////
    
    
    public void Slot_Set(int slot){
        
        curSlot = slot;
        
    }//Slot_Set
    
    
//////////////////////////////////////
///
///     SOUND ACTIONS
///
///////////////////////////////////////
    
    
    private void DialSound() {
    
        if(dialTurnSounds.Length > 0){
                
            if(dialTurnSounds.Length > 1){

                int soundID = rand.Range(0, dialTurnSounds.Length);

                AudioSource.PlayClipAtPoint(dialTurnSounds[soundID], transform.position, m_Volume);

            //dialTurnSounds.Length > 1
            } else {

                AudioSource.PlayClipAtPoint(dialTurnSounds[0], transform.position, m_Volume);

            }//dialTurnSounds.Length > 1
        
        }//dialTurnSounds.Length > 0
        
    }//DialSound
    
    private IEnumerator DialSound_Buff() {
            
        if(dialTurnSounds.Length > 0){
        
            while(isHolding) {

                if(dialTurnSounds.Length > 1){

                    int soundID = rand.Range(0, dialTurnSounds.Length);

                    AudioSource.PlayClipAtPoint(dialTurnSounds[soundID], transform.position, m_Volume);

                //dialTurnSounds.Length > 1
                } else {

                    AudioSource.PlayClipAtPoint(dialTurnSounds[0], transform.position, m_Volume);

                }//dialTurnSounds.Length > 1

                yield return new WaitForSeconds(dialSoundAfter);

            }//isHolding
        
        }//dialTurnSounds.Length > 0

        yield return null;
        
    }//DialSound_Buff
    
    
//////////////////////////////////////
///
///     SAVE/LOAD ACTIONS
///
///////////////////////////////////////
    
    
    public Dictionary<string, object> OnSave() {
        
        return new Dictionary<string, object> {
            
            {"canRotate", canRotate},
            {"curRotation", curRotation},
            {"curSlot", curSlot},
            {"locked", locked }
        
        };//Dictionary
        
    }//OnSave

    public void OnLoad(JToken token) {
        
        canRotate = (bool)token["canRotate"];
        curRotation = token["curRotation"].ToObject<Vector3>();
        curSlot = (int)token["curSlot"];
        locked = (bool)token["locked"];
        
        if(saveState == SaveState.Active){
        
            if(rotationSlots.Count > 0){

                this.transform.localEulerAngles = rotationSlots[curSlot].rotation;

            }//rotationSlots.Count > 0
        
        }//saveState = active
        
    }//OnLoad
    
    
}
