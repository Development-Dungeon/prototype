using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    public float flickerRate = 0.1f;         // Base rate of flickering (in seconds)
    public float flickerRandomness = 0.1f;   // Range to add randomness to flicker timing
    public float minIntensity = 0.5f;        // Minimum light intensity for flicker
    public float maxIntensity = 1.5f;        // Maximum light intensity for flicker

    private Light spotlight;
    private float nextFlickerTime;

    void Start()
    {
        spotlight = GetComponent<Light>();
        nextFlickerTime = Time.time + Random.Range(flickerRate - flickerRandomness, flickerRate + flickerRandomness);
    }

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            // Randomly adjust light intensity between min and max values
            spotlight.intensity = Random.Range(minIntensity, maxIntensity);

            // Set the next flicker time with some randomness
            nextFlickerTime = Time.time + Random.Range(flickerRate - flickerRandomness, flickerRate + flickerRandomness);
        }
    }
}
