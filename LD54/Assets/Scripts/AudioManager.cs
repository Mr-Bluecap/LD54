using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField]
    AudioSource createWallAudio;
    
    [SerializeField]
    AudioSource destroyWallAudio;

    [SerializeField]
    AudioSource paintAudio;

    void Awake()
    {
        Instance = this;
    }

    public void PlayCreateWall()
    {
        createWallAudio.Stop();
        createWallAudio.Play();
    }
    
    public void PlayDestroyWall()
    {
        destroyWallAudio.Stop();
        destroyWallAudio.Play();
    }
    
    public void PlayPaint()
    {
        paintAudio.Stop();
        paintAudio.Play();
    }
}
