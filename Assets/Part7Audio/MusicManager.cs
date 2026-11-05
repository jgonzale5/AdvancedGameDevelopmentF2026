using UnityEngine;
//Add this library to use event instances
using FMOD.Studio;
//Add this library to use event references
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    //The music track we'll be playing
    [SerializeField]
    private EventReference musicTrack;
    //The instance of the music playing
    private EventInstance musicInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Create the instance of the music playing and keep a reference to it
        musicInstance = FMODAudioManager.instance.CreateEventInstance(musicTrack);
        //
        musicInstance.start();
    }

    //Call this function to set the volume of the music
    public void SetVolume(float volume)
    {
        //Changes the parameter "music_volume" of the track
        musicInstance.setParameterByName("music_volume", volume);
    }
}
