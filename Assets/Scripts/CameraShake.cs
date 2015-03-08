using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    
    // How long the object should shake for.
    public float shake = 3;
    
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    public bool shakeRandomly = true;
    public Vector3 shakeDirection = Vector2.up;

    private float maxTime = 0;
    
    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }//if
    }//Awake
    
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
        maxTime = shake;
    }

    void Update()
    {
        if (shake > 0)
        {
            Vector3 pos = Vector3.zero;

            if(shakeRandomly)
            {
                pos = originalPos + Random.insideUnitSphere * shakeAmount;
            }
            else
            {
                pos = originalPos + Bump(shakeDirection) * shakeAmount;
            }//else

            camTransform.localPosition = pos;
            
            shake -= Time.deltaTime * decreaseFactor;
        }//if
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
            Destroy(this);
        }//else
    }//Update

    Vector3 Bump(Vector3 bumpDirection)
    {
        return bumpDirection * Mathf.Lerp(1,0, shake / maxTime);
    }//Bump

    public static CameraShake Shake(Camera camera, float time, float intensity, float damping, Vector2 direction)
    {
        if(camera == null)
            return null;

        CameraShake cs = camera.gameObject.AddComponent<CameraShake>();

        
        cs.shake = time;
        cs.shakeRandomly = (direction == Vector2.zero);
        cs.shakeDirection = direction;
        cs.shakeAmount = intensity;
        cs.decreaseFactor = damping;

        return cs;
    }//if

}//CameraShake