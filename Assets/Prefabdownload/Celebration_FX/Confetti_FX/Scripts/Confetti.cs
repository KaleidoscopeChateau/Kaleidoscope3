using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{

    public GameObject confetti;
    private bool confettiActive = false;

    // Use this for initialization
    void Start()
    {

        confetti.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1")) //check to see if the left mouse was pushed.
        {

            if (confettiActive == false)
            {

                StartCoroutine("FireConfetti");

            }

        }

    }

    IEnumerator FireConfetti()
    {

        confettiActive = true;

        confetti.SetActive(true);
        yield return new WaitForSeconds(10);
        confetti.SetActive(false);

        confettiActive = false;

    }

}


