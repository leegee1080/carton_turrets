using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!!!!!!!!!!HEY DUMMY Next time you use this: put each sound type in its own list (instead of Sounds being the only list)

public static class Sound_Events
{
    #region play sound event
    public static event System.Action<string> play_sound_event;
    public static void Play_Sound(string sound_name)
    {
        play_sound_event?.Invoke(sound_name);
    }
    #endregion
    #region delay play sound event
    public static event System.Action<string, float> delay_play_sound_event;
    public static void Delay_Play_Sound(string sound_name, float delay)
    {
        delay_play_sound_event?.Invoke(sound_name, delay);
    }
    #endregion
    #region stop sound event
    public static event System.Action<string> stop_sound_event;
    public static void Stop_Sound(string sound_name)
    {
        stop_sound_event?.Invoke(sound_name);
    }
    #endregion
    #region change volume sound event
    public static event System.Action<float, Sound_Type_Tags> change_volume_event;
    public static void Change_Volume(float new_volume, Sound_Type_Tags tag)
    {
        change_volume_event?.Invoke(new_volume, tag);
    }
    #endregion
}

public enum Sound_Type_Tags
{
    music,
    fx,
    menu
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public Sound_Type_Tags Tag;

    public bool loop;
    public bool randomPitch;
    [Range(0, 1)]
    public float Maxvolume;
    [Range(0, 1)]
    public float volume;
    [Range(0.1f, 3)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}

public class AudioController : MonoBehaviour
{
    [Range(0, 1)]
    public float currentGameVolumeLevel;
    [Range(0, 1)]
    public float currentMusicVolumeLevel;
    [Range(0, 1)]
    public float currentMenuVolumeLevel;
    public Sound[] Sounds;
    public Dictionary<string, AudioSource> MusicAS = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> FXAS = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> MenuAS = new Dictionary<string, AudioSource>();
    public static AudioController singleton;
    public AudioSource selectedSound;

    private IEnumerator _IfadeIn;
    private IEnumerator _IfadeOut;

    private void Awake()
    {
        //Checks to make sure there is only one SM
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        currentGameVolumeLevel = PlayerPrefs.GetFloat("gameVolume", 0.5f);
        currentMusicVolumeLevel = PlayerPrefs.GetFloat("musicVolume", 0.5f);

        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.Maxvolume;
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            switch (s.Tag)
            {
                case Sound_Type_Tags.music:
                    s.source.volume = currentMusicVolumeLevel;
                    MusicAS[s.name] = s.source;
                    break;
                case Sound_Type_Tags.fx:
                    s.source.volume = currentGameVolumeLevel;
                    FXAS[s.name] = s.source;
                    break;
                case Sound_Type_Tags.menu:
                    s.source.volume = currentMenuVolumeLevel;
                    MenuAS[s.name] = s.source;
                    break;
                default:
                    break;
            }
            s.source.playOnAwake = false;
            s.source.pitch = s.pitch;
        }

        Sound_Events.play_sound_event += PlaySound;
        Sound_Events.delay_play_sound_event += DelayPlaySound;
        Sound_Events.stop_sound_event += StopSound;
        Sound_Events.change_volume_event += ChangeVolume;

    }

    public void ChangeVolume(float newVolume, Sound_Type_Tags tag)
    {
        if(tag == Sound_Type_Tags.music)
        {
            GlobalDataStorage.singleton.musicVolume = newVolume;
        }
        else
        {
            GlobalDataStorage.singleton.gameVolume = newVolume;
        }

        foreach (Sound s in Sounds)
        {
            if (s.Tag == tag)
            {
                s.source.volume = s.Maxvolume * newVolume;
            }
        }
        switch (tag)
        {
            case Sound_Type_Tags.music:
                currentMusicVolumeLevel = newVolume;
                break;
            case Sound_Type_Tags.fx:
                currentGameVolumeLevel = newVolume;
                break;
            case Sound_Type_Tags.menu:
                currentMenuVolumeLevel = newVolume;
                break;
            default:
                print("incorrect volume change tag sent");
                break;
        }

    }


    public void PlaySound(string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log(soundName + " sound not found!");
            return;
        }
        if (s.randomPitch)
        {
            s.source.pitch = Random.Range(s.pitch - 0.2f, s.pitch + 0.2f);
        }
        s.source.Play();
    }

    public void DelayPlaySound(string soundName, float delayTime)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log(soundName + " sound not found!");
            return;
        }
        if (s.randomPitch)
        {
            s.source.pitch = Random.Range(s.pitch - 0.2f, s.pitch + 0.2f);
        }

        IEnumerator DelayPlay()
        {
            yield return new WaitForSeconds(delayTime);
            s.source.Play();
        }
        StartCoroutine(DelayPlay());
        
    }

    public void StopSound(string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log(soundName + " sound not found!");
            return;
        }
        s.source.Stop();
    }

    public AudioSource SelectSound(string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.name == soundName);
        selectedSound = s.source;
        return s.source;
    }

    public void ChangeSelectedSoundVolume(float newVolume)
    {
        selectedSound.volume = newVolume;
    }

    public void FadeSoundIn(float speed, string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.source == SelectSound(soundName));
        Sound_Type_Tags t = System.Array.Find(Sounds, sound => sound.source == SelectSound(soundName)).Tag;
        float maxVolume = 0;
        switch (t)
        {
            case Sound_Type_Tags.music:
                maxVolume = currentMusicVolumeLevel;
                break;
            case Sound_Type_Tags.fx:
                maxVolume = currentGameVolumeLevel;
                break;
            case Sound_Type_Tags.menu:
                maxVolume = currentMenuVolumeLevel;
                break;
            default:
                maxVolume = 1;
                break;
        }
        s.source.volume = 0f;

        PlaySound(soundName);


        StartCoroutine(IFadeIn(s, speed, maxVolume));
    }
    IEnumerator IFadeIn(Sound s, float speed, float maxVolume)
    {
        
        int timer = 100; //failsafe timer
        while(s.source.volume < maxVolume && timer > 0)
        {
            timer -= 1;
            s.source.volume += speed;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public void FadeSoundOut(float speed, string soundName)
    {
        Sound s = System.Array.Find(Sounds, sound => sound.source == SelectSound(soundName));

        StartCoroutine(IFadeOut(s, speed));
    }

    IEnumerator IFadeOut(Sound s, float speed)
    {
        int timer = 100; //failsafe timer
        while(s.source.volume > 0 && timer > 0)
        {
            timer -= 1;
            s.source.volume -= speed;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        s.source.Stop();
    }

    private void OnDestroy()
    {
        if(_IfadeIn != null){StopCoroutine(_IfadeIn);}
        if(_IfadeOut != null){StopCoroutine(_IfadeOut);}

        Sound_Events.play_sound_event -= PlaySound;
        Sound_Events.delay_play_sound_event -= DelayPlaySound;
        Sound_Events.stop_sound_event -= StopSound;
        Sound_Events.change_volume_event -= ChangeVolume;
    }
}

