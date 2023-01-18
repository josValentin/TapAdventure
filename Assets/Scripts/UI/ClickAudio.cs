using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour
{
    public static ClickAudio Instance;
    private AudioSource m_AudioSource;
    private AudioSource MenuGameMusic_AudioSource;
    private ManagerVars vars;

    private int SelectAudio = 0;

    
    private void Awake()
    {
        Instance = this;
        m_AudioSource = GetComponent<AudioSource>();
        MenuGameMusic_AudioSource = GameManager.Instance.MenuMusic.GetComponent<AudioSource>();

        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.PlayClikAudio, PlayAudio);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.AddListener<bool>(EventDefine.IsMainGameMusicOn, IsMenuGameMusicOn);

        EventCenter.AddListener<float>(EventDefine.UpdateSliderBarSound, SetSoundValue);
        EventCenter.AddListener<float>(EventDefine.UpdateSliderBarMusic, SetMusicValue);

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayClikAudio, PlayAudio);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.RemoveListener<bool>(EventDefine.IsMainGameMusicOn, IsMenuGameMusicOn);

        EventCenter.RemoveListener<float>(EventDefine.UpdateSliderBarSound, SetSoundValue);
        EventCenter.RemoveListener<float>(EventDefine.UpdateSliderBarMusic, SetMusicValue);
    }
    private void PlayAudio()
    {
        if(SelectAudio == 0)
        {
            m_AudioSource.PlayOneShot(vars.buttonClip);
        }
        else if(SelectAudio == 1)
        {
            m_AudioSource.PlayOneShot(vars.errorClip);
        }
        else if (SelectAudio == 2)
        {
            m_AudioSource.PlayOneShot(vars.rewardClip);
        }






        SelectAudio = 0;
    }

    public void SetSoundValue(float value)
    {
        m_AudioSource.volume = value;
    }

    public void SetMusicValue(float value)
    {
        MenuGameMusic_AudioSource.volume = value;
    }

    public void setSelectAudio(int audioID)
    {
        SelectAudio = audioID;
    }
    /// <summary>
    /// 音效是否开启
    /// </summary>
    /// <param name="value"></param>
    private void IsMusicOn(bool value)
    {
        m_AudioSource.mute = !value;
    }

    private void IsMenuGameMusicOn(bool value)
    {
        MenuGameMusic_AudioSource.mute = !value;
    }
}
