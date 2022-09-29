// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class Cake_Firework : MonoBehaviour {

	public GameObject cakeFirework;


	void  Start (){

		cakeFirework.SetActive(false);

	}  


	void  Update (){

		if (Input.GetButtonDown("Fire1"))
		{
			LightFirework();

		}

	}


	void  LightFirework (){

		cakeFirework.SetActive(true);

	}


}