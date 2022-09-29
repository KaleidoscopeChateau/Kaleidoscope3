using UnityEngine;
using System.Collections;

public class Sparkler : MonoBehaviour {

public GameObject sparklerFX;
public GameObject sparklerAnim;
public GameObject mainFX;
public GameObject sparklerMesh;
public ParticleSystem smokeParticles;
public GameObject burnGlowTop;
public GameObject burnGlowBottom;

private float offset = 0;
private int MatchLit = 0;
private float burnGlowScale = 0.5f;

  
void Start (){

    sparklerFX.SetActive(false);

	burnGlowTop.transform.localScale = Vector3.Scale(burnGlowTop.transform.localScale, new Vector3(1, 0.5f, 1));

}  
  
  
void Update (){
 
	if (Input.GetButtonDown("Fire1")) //check to see if the left mouse was pressed - lights fuse
    {

        if (MatchLit == 0)
        {
                  
		    StartCoroutine("Fuse");

        }
         
    }
            
 }
 
 
IEnumerator Fuse (){

    MatchLit = 1;

    // Start flame particle effects
    sparklerFX.SetActive(true);
    // SmokeParticlesEmitter.enabled = true;
	smokeParticles.Play();

    offset = 0.05f;
    sparklerMesh.GetComponent<Renderer>().material.SetTextureOffset ("_DetailAlbedoMap", new Vector2(offset,offset));



    sparklerAnim.GetComponent<Animation>().Play();

	yield return new  WaitForSeconds (0.4f);


           
	while (offset < 0.45f)
	{  

	// Continue offsetting the secondary match burnt texture over the match

    	offset += (Time.deltaTime * 0.017f);
    	sparklerMesh.GetComponent<Renderer>().material.SetTextureOffset ("_DetailAlbedoMap", new Vector2(offset,offset));


      // Trigger smoke particle effects as the burnt texture offsets over the match

    	if (offset > 0.45f)
    	{
    	    mainFX.SetActive(false);
    	}

    	if (offset > 0.06f)
    	{
    	    
    	    if  (burnGlowScale < 1.0f)
    	    {
    	         burnGlowScale += 0.01f;
    	    }
					
		    burnGlowTop.transform.localScale = new Vector3(1, burnGlowScale, 1);

    	}


     
     	yield return 0;

	}


   
     
    offset = 0;


}


}