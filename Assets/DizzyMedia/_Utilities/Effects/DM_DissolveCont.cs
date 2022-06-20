using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Dizzy Media/Utilities/Effects/Dissolve Controller")]
public class DM_DissolveCont : MonoBehaviour {

    
//////////////////////////
//
//      VALUES
//
//////////////////////////
    
///////////////
//
//   REFERENCES
//
///////////////
    
    [Space]
    
    [Header("References")]
    
    [Space]
    
    public MeshRenderer meshRenderer;
    

///////////////
//
//   SPEED OPTIONS
//
///////////////
    
    
    [Space]
    
    [Header("User Options")]
    
    [Space]
    
    public float speed = 0.5f;
    
    
///////////////
//
//   AUTO
//
///////////////

    [Space]
    
    [Header("Auto")]
    
    [Space]
    
    public Material[] mats;
    
    public float amount = 0f;
    
    public bool dissolveOut;
    public bool dissolveIn;
    
    
//////////////////////////
//
//      START ACTIONS
//
//////////////////////////
    

    void Start(){
        
        if(meshRenderer != null){
            
            if(mats.Length < 1){
                
                mats = meshRenderer.materials;
        
            }//mats.Length < 1
            
        }//meshRenderer != null
        
    }//Start
    
    
//////////////////////////
//
//      UPDATE ACTIONS
//
//////////////////////////
    
    
    void Update(){

        if(dissolveIn){

            if(mats.Length > 0){

                if(amount > 0){

                    amount -= Time.deltaTime;
                    
                    mats[0].SetFloat("_Cutoff", Mathf.Sin(amount * speed));

                //amount > 0
                } else {

                    dissolveIn = false;
                    amount = 0;

                    mats[0].SetFloat("_Cutoff", 0);

                }//amount > 0

            }//mats.Length > 0
            
        }//dissolveIn
        
        if(dissolveOut){

            if(mats.Length > 0){

                if(amount < 2){

                    amount += Time.deltaTime;
                    
                    mats[0].SetFloat("_Cutoff", Mathf.Sin(amount * speed));

                //amount < 2
                } else {

                    dissolveOut = false;
                    amount = 2;
                    
                    mats[0].SetFloat("_Cutoff", 1);
                    
                }//amount < 2

            }//mats.Length > 0
            
        }//dissolveOut
        
    }//Update
    
    
//////////////////////////
//
//      DISSOLVE ACTIONS
//
//////////////////////////
    
    
    public void Dissolve_In(){
        
        amount = 2;
            
        dissolveIn = true;
        dissolveOut = false;
        
    }//Dissolve_In
    
    public void Dissolve_Out(){
        
        amount = 0;
            
        dissolveIn = false;
        dissolveOut = true;
        
    }//Dissolve_Out
    
    public void Dissolve(bool state){
        
        if(state){
            
            amount = 0;
            
            dissolveIn = false;
            dissolveOut = true;
            
        //state
        } else {
            
            amount = 1;
            
            dissolveIn = true;
            dissolveOut = false;
            
        }//state
        
    }//Dissolve
    
    public void DissolveQuick_In(){
    
        mats[0].SetFloat("_Cutoff", 0);
        
    }//DissolveQuick_In
    
    public void DissolveQuick_Out(){
    
        mats[0].SetFloat("_Cutoff", 1);
        
    }//DissolveQuick_Out
    

}
