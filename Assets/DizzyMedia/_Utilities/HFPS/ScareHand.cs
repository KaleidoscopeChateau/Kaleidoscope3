using UnityEngine;
using ThunderWire.Utility;
using HFPS.Player;
using HFPS.Systems;

[AddComponentMenu("Dizzy Media/Utilities/HFPS/Scare Handler")]
public class ScareHand : MonoBehaviour {
    
	private JumpscareEffects jsEffects;

	[Header("Jumpscare Setup")]
	public AudioClip JumpscareSound;
	public AudioClip ScaredBreath;
    
	[Range(0, 5)] public float scareVolume = 0.5f;
	[Tooltip("Value sets how long will be player scared.")]
	public float scaredBreathTime = 33f;
	public bool enableEffects = true;

	[Header("Scare Effects")]
	public float chromaticAberrationAmount = 0.8f;
	public float vignetteAmount = 0.3f;
	public float effectsTime = 5f;

	[Header("Scare Shake")]
	public bool shakeByPreset = false;
	public float magnitude = 3f;
	public float roughness = 3f;
	public float startTime = 0.1f;
	public float durationTime = 3f;

	[Header("Scare Position Influence")]
	public Vector3 PositionInfluence = new Vector3(0.15f, 0.15f, 0f);
	public Vector3 RotationInfluence = Vector3.one;

    [Header("Auto")]
	public bool isPlayed;

	void Start() {
        
        if(jsEffects == null){
            
            jsEffects = ScriptManager.Instance.gameObject.GetComponent<JumpscareEffects>();
        
        }//jsEffects == null
        
	}//Start
    
    public void Scare_Init(){
    
		if (!isPlayed) {

			if(JumpscareSound) {
                
				Utilities.PlayOneShot2D(transform.position, JumpscareSound, scareVolume);
			
            }//JumpscareSound

			if(enableEffects) {
                
				if(shakeByPreset) {
                        
				    CameraShakeInstance shakeInstance = CameraShakePresets.Scare;
				    jsEffects.Scare(shakeInstance, chromaticAberrationAmount, vignetteAmount, scaredBreathTime, effectsTime, ScaredBreath);
					
                //shakeByPreset
                } else {
                        
				    CameraShakeInstance shakeInstance = new CameraShakeInstance(magnitude, roughness, startTime, durationTime);
				    shakeInstance.PositionInfluence = PositionInfluence;
				    shakeInstance.RotationInfluence = RotationInfluence;
				    jsEffects.Scare(shakeInstance, chromaticAberrationAmount, vignetteAmount, scaredBreathTime, effectsTime, ScaredBreath);
					
                }//shakeByPreset
			
            }//enableEffects

			isPlayed = true;
            
		}//!isPlayed
        
    }//Scare_Init
    
    public void Played_State(bool state){
        
        isPlayed = state;
        
    }//Played_State
    
    
}
