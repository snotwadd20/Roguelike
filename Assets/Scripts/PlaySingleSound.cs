using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlaySingleSound : MonoBehaviour 
{
    private AudioSource audioSource = null;
    public float delay = 0.0f;
    public AudioClip clipToPlay = null;

    public float volume = 1.0f;
    public float pitch = 1.0f;

    protected bool pooled = false;

	// Use this for initialization
	void Init () 
    {
        if(clipToPlay == null)
        {
            print ("PlaySingleSound.cs - No AudioClip specified");
            Destroy(gameObject);
            return;
        }//if

        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();

        }//if

        audioSource.clip = clipToPlay;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.playOnAwake = false;

        if(delay > 0)
        {
            audioSource.PlayDelayed(delay);

        }//if
        else
        {
            audioSource.Play();
        }//else
        TimerCallback.createTimer(delay + clipToPlay.length + 0.1f, done, "PlaySingleSoundTimer", true);
    }//Init
    	
	// Update is called once per frame
	public void done()
    {
        if(pooled)
        {
            gameObject.SetActive(false);

            if(pool == null)
                pool = new List<PlaySingleSound>();

            pool.Add(this);
        }//if
        else
            Destroy(gameObject);
    }//done

    void OnDestroy()
    {
        if(pool == null)
            return;

        foreach(PlaySingleSound sound in pool)
        {
            Destroy(sound.gameObject);
        }//foreach
        pool = null;
    }//onDestroy

    protected static List<PlaySingleSound> pool = null;

    public static GameObject Spawn(AudioClip clipToPlay, float delay, float volume = 1.0f, float pitch = 1.0f)
    {
        if(pool == null)
            pool = new List<PlaySingleSound>();

        GameObject obj = null;
        PlaySingleSound pss = null;
        if(pool.Count <= 0)
        {
            obj = new GameObject();
            obj.name = "PlaySingleSound";
            pss = obj.AddComponent<PlaySingleSound>();

        }//if
        else
        {
            pss = pool[0];
            pool.RemoveAt(0);
            pss.gameObject.SetActive(true);
        }//else

        pss.clipToPlay = clipToPlay;
        pss.delay = delay;
        pss.volume = volume;
        pss.pooled = true;
        pss.pitch = pitch;
        pss.Init();

        return obj;
    }//Spawn
}
