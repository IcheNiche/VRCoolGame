using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSAndNetworkCounter : MonoBehaviour {

    public float frequency = 0.5f;

    public int FramesPerSec { get; protected set; }

    public Text fpsText;
    public Text roundTripTimeText;

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
            fpsText.text = FramesPerSec.ToString() + " fps";

            roundTripTimeText.text = PhotonNetwork.networkingPeer.RoundTripTime + " ms";
        }
    }
}
