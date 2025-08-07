using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : SingleTonMonoBehaviour<AudioMgr>
{
    public List<AudioSource> audioSourcePool = new List<AudioSource>();
    private AudioSource bgAudioSource;

    public float Vol;
    public float MusicVol;

    public void Init()
    {
        this.audioSourcePool = new List<AudioSource>();
        for (int index = 0; index < 5; index++)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            this.SetAudiioSourceDefaultValue(audioSource);
            this.audioSourcePool.Add(audioSource);
        }

        bgAudioSource = gameObject.AddComponent<AudioSource>();
        this.SetAudiioSourceDefaultValue(bgAudioSource);
    }

    public void SetAudiioSourceDefaultValue(AudioSource audioSource)
    {
        audioSource.pitch = 1.0f;
        audioSource.volume = 1.0f;
        audioSource.loop = false;
        audioSource.clip = null; 
        //audioSource.reverbZoneMix = 0f
    }

    public AudioSource getSourceFromPool()
    {
        foreach (var audioSource in this.audioSourcePool)
        {
            if (!audioSource.isPlaying)
            {
                this.SetAudiioSourceDefaultValue(audioSource);
                return audioSource;
            }
        }

        var newAudioSource = gameObject.AddComponent<AudioSource>();
        this.SetAudiioSourceDefaultValue(newAudioSource);
        audioSourcePool.Add(newAudioSource);
        PrintTool.Log("audioSource： ", audioSourcePool.Count);
        return newAudioSource;
    }

    private AudioClip GetAudioClip(string audioName)
    {
        return ResCenter.Instance.mBundleGameAllRes.FindAudioClip(audioName);
    }

    public AudioSource PlaySound(int audioName, bool loop = false, int delay = 0)
    {
        bool bMute = DataCenter.Instance.bMute;
        AudioClip obj = this.GetAudioClip(audioName.ToString());
        if (obj)
        {
            AudioSource mAudioSource = this.getSourceFromPool();
            mAudioSource.clip = obj;
            mAudioSource.loop = loop;
            mAudioSource.volume = 1;
            mAudioSource.mute = bMute;
            mAudioSource.PlayDelayed(delay);
            return mAudioSource;
        }
        else
        {
            PrintTool.Log("Not Exist audioName: ", audioName);
        }
        return null;
    }

    public void StopSound(int audioName)
    {
        foreach (var audioSource in this.audioSourcePool)
        {
            if(audioSource.isPlaying && audioSource.clip.name.EndsWith(audioName.ToString()))
            {
                audioSource.Stop();
            }
        }
    }

    public void UpdateSoundState()
    {
        bool bMute = DataCenter.Instance.bMute;
        foreach (var audioSource in this.audioSourcePool)
        {
            audioSource.mute = bMute;
        }
    }

    public void playMusic(string musicName, int musicIndex)
    {
        AudioSource mAudioSource = bgAudioSource;
        mAudioSource.Stop();
        mAudioSource.clip = null;

        AudioClip obj = GetAudioClip(musicName);
        if(obj)
        {
            mAudioSource.clip = obj;
            mAudioSource.loop = true;
            if(musicIndex == 3)
            {
                mAudioSource.volume = 0.3f;
            }else if(musicIndex == 4)
            {
                mAudioSource.volume = 0.6f;
            }else if(musicIndex == 5)
            {
                mAudioSource.volume = 0.22f;
            }else if(musicIndex == 6)
            {
                mAudioSource.volume = 0.2f;
            }else if(musicIndex == 7)
            {
                mAudioSource.volume = 0.4f;
            }
            else
            {
                mAudioSource.volume = 0.2f;
            }
            
            mAudioSource.Play();
        }
    }

    public void StopTweenMusic()
    {
        AudioSource mAudioSource = bgAudioSource;
        if(mAudioSource.clip == null) return;
        
        float oriVolume = mAudioSource.volume;
        LeanTween.value(oriVolume, 0f, 0.5f).setOnUpdate((value)=>
        {
            mAudioSource.volume = value;
        })
        .setOnComplete(()=>
        {
            mAudioSource.Stop();
        });
    }

}


