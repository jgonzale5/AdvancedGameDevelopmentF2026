using UnityEngine;
//Add the FMOD Unity library
using FMODUnity;

public class FootstepSoundTest : MonoBehaviour
{
    [SerializeField]
    private EventReference footstepSound;

    //Tell the audio manager to play a footstep sound
    public void PlayFootstep()
    {
        FMODAudioManager.instance.PlayOneShot(footstepSound, transform.position);
    }
}
