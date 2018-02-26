using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    public float frequency = 0.5f;

    public int FramesPerSec { get; protected set; }

    public Text textScript;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            //gameObject.guiText.text = FramesPerSec.ToString() + " fps";
            textScript.text = FramesPerSec.ToString() + " fps";
        }
    }
}
