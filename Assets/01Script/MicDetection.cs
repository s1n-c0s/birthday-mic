using UnityEngine;

public class MicDetection : MonoBehaviour
{
    [SerializeField] private ParticleSystem candleLight;
    public float threshold = 0.2f;  // Fixed threshold for detecting a blow (adjust manually in Inspector)
    AudioClip micInput;
    bool micActive = false;
    int sampleRate = 44100;
    bool lightOn = true;  // Track the light's current state
    int bufferLength = 1;  // Buffer length in seconds for mic input

    void Start()
    {
        StartMic();
    }

    void Update()
    {
        if (micActive && micInput != null && lightOn)
        {
            float loudness = GetMicLoudness() * 10f;

            // Debug: Log the loudness level every frame
            // Debug.Log("Mic Loudness: " + loudness);

            // Check if the loudness exceeds the threshold to trigger the blow event
            if (loudness > threshold)
            {
                Debug.Log("Blow detected! Turning off the light.");
                TurnOffLight();
            }
        }
    }

    // Start recording from the microphone with a shorter buffer length
    void StartMic()
    {
        if (Microphone.devices.Length > 0)
        {
            micInput = Microphone.Start(null, true, bufferLength, sampleRate);  // Shorter buffer length for lower latency
            micActive = true;
            Debug.Log("Microphone started.");
        }
        else
        {
            Debug.Log("No microphone detected.");
            micActive = false;
        }
    }

    // Stop microphone recording
    void StopMic()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            micActive = false;
            Debug.Log("Microphone stopped.");
        }
    }

    // Get the loudness of the sound from the microphone
    float GetMicLoudness()
    {
        float[] samples = new float[64];  // Use a smaller sample size to reduce delay
        micInput.GetData(samples, 0);  // Get data from the microphone's AudioClip
        float sum = 0f;

        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        return sum / samples.Length;  // Return the average loudness
    }

    // Turn off the candle light when a blow is detected
    void TurnOffLight()
    {
        if (candleLight != null)
        {
            candleLight.Stop();  // Turn off the Light (Particle System)
            lightOn = false;     // Update the light state
            Debug.Log("Candle light turned off.");
        }
    }

    void OnDestroy()
    {
        StopMic();  // Clean up the microphone when the object is destroyed
    }

    void OnDisable()
    {
        StopMic();  // Clean up the microphone when the script is disabled
    }
}
