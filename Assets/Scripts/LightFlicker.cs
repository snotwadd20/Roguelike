using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour 
{
    public enum WaveFunctionTypes {sin, triangle, square, sawTooth, invertedSaw, noise};
    // Properties
    public WaveFunctionTypes waveFunction = WaveFunctionTypes.sin; // possible values: sin, triangle, square, sawTooth, invertedSaw, noise (random)

    public bool changeColor = true;
    public bool changeIntensity = false;
    public bool changeAngle = false;

    public float baseValue = 0.0f; // start
    public float amplitude = 1.0f; // amplitude of the wave
    public float phase = 0.0f; // start point inside on wave cycle
    public float frequency = 0.5f; // cycle frequency per second
    
    // Keep a copy of the original color
    private Color originalColor;
    private float originalAngle;
    private float originalIntensity;
    private Light theLight;
    
    // Store the original color
    void Start () 
    {
        if(theLight == null)
            theLight = GetComponent<Light>();

        originalColor = theLight.color;
        originalAngle = theLight.spotAngle;
        originalIntensity = theLight.intensity;
    }
    
    void Update () 
    {
        if(changeColor)
            theLight.color = originalColor * EvalWave();
        else
            theLight.color = originalColor;

        if(changeAngle)
            theLight.spotAngle = originalAngle * EvalWave();
        else
            theLight.spotAngle = originalAngle;

        if(changeIntensity)
            theLight.intensity = originalIntensity * EvalWave();
        else
            theLight.intensity = originalIntensity;
    }
    
    private float EvalWave () 
    {

        float x = (Time.time + phase)*frequency;
        float y;
        
        x = x - Mathf.Floor(x); // normalized value (0..1)
        
        if (waveFunction == WaveFunctionTypes.sin) {
            y = Mathf.Sin(x*2*Mathf.PI);
        }
        else if (waveFunction == WaveFunctionTypes.triangle) {
            if (x < 0.5)
                y = 4.0f * x - 1.0f;
            else
                y = -4.0f * x + 3.0f;  
        }    
        else if (waveFunction == WaveFunctionTypes.square) {
            if (x < 0.5f)
                y = 1.0f;
            else
                y = -1.0f;  
        }    
        else if (waveFunction == WaveFunctionTypes.sawTooth) {
            y = x;
        }    
        else if (waveFunction == WaveFunctionTypes.invertedSaw) {
            y = 1.0f - x;
        }    
        else if (waveFunction == WaveFunctionTypes.noise) {
            y = 1 - (Random.value*2);
        }
        else {
            y = 1.0f;
        }        
        return (y*amplitude)+baseValue;     
    }
}
