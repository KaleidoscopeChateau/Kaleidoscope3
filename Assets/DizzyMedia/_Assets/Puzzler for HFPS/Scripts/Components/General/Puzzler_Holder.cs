using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Dizzy Media/Puzzler/Components/General/Puzzler Holder")]
public class Puzzler_Holder : MonoBehaviour {
    
    
//////////////////////////////////////
///
///     VALUES
///
///////////////////////////////////////
    
///////////////////////////
///
///     REFERENCES
///
///////////////////////////
    
    
    public Collider trigger;
    public Rigidbody rigid;

    
///////////////////////////
///
///     AUTO
///
///////////////////////////
    
    
    public Puzzler_Handler puzzlerHand;
    
    public int slot;
    public int secondSlot;
    public float weight;
    
    public int tabs;
    
    
//////////////////////////////////////
///
///     START ACTIONS
///
///////////////////////////////////////
    
    
    void Start(){}
    
    
//////////////////////////////////////
///
///     SLOT ACTIONS
///
///////////////////////////////////////
    
    
    public void Slot_Empty(){
        
        if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.MultiItems){
            
            puzzlerHand.multiSlots[slot - 1].filled = false;
            puzzlerHand.multiSlots[slot - 1].active = false;
        
            puzzlerHand.multiSlots[slot - 1].onReset.Invoke();
        
        }//puzzleType = multi items
        
        if(puzzlerHand.puzzleType == Puzzler_Handler.Puzzle_Type.Weight){
        
            puzzlerHand.weightModules[slot - 1].weightSlots[secondSlot - 1].filled = false;
            
            puzzlerHand.weightModules[slot - 1].curWeight -= weight;
        
            puzzlerHand.weightModules[slot - 1].weightSlots[secondSlot - 1].onReset.Invoke();
            
            puzzlerHand.weightModulesTemp[slot - 1].weightSlots[secondSlot - 1] = 0;
            
            puzzlerHand.Weight_Check();
            puzzlerHand.CompleteCheck();
        
        }//puzzleType = weight
        
        puzzlerHand.puzzlerHolders.Remove(this);
        
    }//Slot_Empty
    
    
//////////////////////////////////////
///
///     STATE ACTIONS
///
///////////////////////////////////////
    
    
    public void ActiveState(bool state){
            
        trigger.enabled = state;
        
        if(rigid != null){
        
            if(state){

                rigid.isKinematic = false;
                rigid.useGravity = true;

            //state
            } else {

                rigid.isKinematic = true;
                rigid.useGravity = false;

            }//state
        
        }//rigidbody != null
        
    }//ActiveState
    
}
