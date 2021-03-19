using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;


    public void StartSlowMotion(float timeFactor)
    {
        Time.timeScale = timeFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

}
