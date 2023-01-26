using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Progression;

public class LightResponder : MonoBehaviour
{
    public Rooms connectedRoom = Rooms.None;
    public int requiredLightAmount = 1;
    public bool setsActiveWhenRequirementsMet = true;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        bool isAvailable = ProgressionManager.Instance.HasMinLightAmount(connectedRoom, requiredLightAmount);
        if (isAvailable)
        {
            gameObject.SetActive(setsActiveWhenRequirementsMet);
        }
        else
        {
            gameObject.SetActive(setsActiveWhenRequirementsMet);
        }

    }
        
}
