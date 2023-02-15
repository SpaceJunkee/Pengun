using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;
    public float slowDownPitch = 0.5f;
    public AudioSource[] allAudioSources;
    float[] allAudioPitches;

    private void Start()
    {
        allAudioSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        allAudioPitches = new float[allAudioSources.Length];

        for (int i = 0; i < allAudioPitches.Length; i++)
        {
            allAudioPitches[i] = allAudioSources[i].pitch;
        }
    }

    public void StartSlowMotion(float timeFactor)
    {
        LowerAllAudioPitches();
        Time.timeScale = timeFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void StopSlowMotion()
    {
        ChangeToOriginalPitch();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void InvokeStopSlowMotion(float delay)
    {
        Invoke("StopSlowMotion", delay);
    }

    //Lower pitch of all audio pitches except null and tagged with DontChangeAudioPitch
    void LowerAllAudioPitches()
    {
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            if(allAudioSources[i] != null && allAudioSources[i].tag != "DontChangeAudioPitch")
            {
                allAudioSources[i].pitch = slowDownPitch;
            }                       
        }
    }

    void ChangeToOriginalPitch()
    {
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            if (allAudioSources[i] != null)
            {
                allAudioSources[i].pitch = allAudioPitches[i];
            }               
        }
    }

}
