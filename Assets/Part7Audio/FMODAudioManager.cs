using UnityEngine;
//Add the FMOD Unity library
using FMODUnity;

public class FMODAudioManager : MonoBehaviour
{
    //A singleton reference to this component
    public static FMODAudioManager instance 
    { 
        get; 
        private set; 
    }//This lets us force this parameter to be read-only for other scripts

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

}
