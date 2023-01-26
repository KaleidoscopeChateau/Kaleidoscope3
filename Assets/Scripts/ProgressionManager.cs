using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Progression;

namespace Progression
{
    public enum Rooms
    {
        None,
        Kitchen,
        LivingRoom,
        DiningRoom
    }

}

public class ProgressionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ProgressionManager Instance { get; private set; }

    Dictionary<Rooms, int> lightValues = new Dictionary<Rooms, int>();


    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void AddLight(Rooms room, int amount)
    {
        if (lightValues.ContainsKey(room))
        {
            lightValues[room] += amount;
        }
        else
        {
            lightValues[room] = amount;
        }
        Debug.Log("lightValue is now " + amount);
    }
    public void RemoveLight(Rooms room, int amount)
    {
        if (lightValues.ContainsKey(room))
        {
            lightValues[room] -= amount;
        }
        else
        {
            lightValues[room] = -amount;
        }
        Debug.Log("lightValue is now " + amount);
    }
    public bool HasMinLightAmount(Rooms room, int requiredAmount)
    {
        if (lightValues.ContainsKey(room))
        {
            return lightValues[room] >= requiredAmount;
        }
        else
        {
            return 0 >= requiredAmount;
        }
    }
}
