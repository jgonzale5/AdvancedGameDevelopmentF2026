using UnityEngine;
//Add the FMOD Unity library
using FMODUnity;
//For lists
using System.Collections.Generic;
//For event instances
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    //A singleton reference to this component
    public static FMODAudioManager instance 
    { 
        get; 
        private set; 
    }//This lets us force this parameter to be read-only for other scripts

    //The list of event instances currently running
    private List<EventInstance> activeEvents = new List<EventInstance>();

    //On awake we set this as the instance
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("There's two audio managers!");
        }
    }

    //Play a sound once
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    //This creates an instance of an event with the specified sound
    public EventInstance CreateEventInstance(EventReference sound)
    {
        //Create an instance with the specified sound
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        //Add the instance to the list of active instances
        activeEvents.Add(instance); 
        //Return the instance to the script that called this function
        return instance; 
    }

    //We use this function to clean up any instances still running. Mainly when changing scenes.
    private void CleanUp()
    {
        foreach (var EI in  activeEvents)
        {
            EI.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            EI.clearHandle();
        }
    }
}
