using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioUsage : MonoBehaviour
{
    public VisualEffectManager visualEffectManager;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "EasyMode")
        {
            AudioManager.Instance.PlayBackgroundMusicByName("roomMatchingBgm");
        }
        else
        {
            if (AudioManager.Instance.backgroundMusicSource.clip == null)
                AudioManager.Instance.PlayBackgroundMusicByName("lobbyBgm");
        }

        //if (visualEffectManager == null)
        //{
        //    visualEffectManager = Camera.main.GetComponent<VisualEffectManager>();
        //}
    }

    public void OnButtonClick()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
    }

    public void OnButtonHover()
    {
        AudioManager.Instance.PlaySFX("ButtonHover");
    }

    public void OnButtonConfirm()
    {
        AudioManager.Instance.PlaySFX("ButtonConfirm");
    }

    public void OnButtonPause()
    {
        AudioManager.Instance.PlaySFX("ButtonPause");
    }

    public void OnButtonUnpause()
    {
        AudioManager.Instance.PlaySFX("ButtonUnpause");
    }

    public void OnUnitClick()
    {
        AudioManager.Instance.PlaySFX("UnitClick");
    }

    public void OnMapClick()
    {
        AudioManager.Instance.PlaySFX("MapClick");
    }

    public void OnMove()
    {
        AudioManager.Instance.PlaySFX("Movement");
    }

    public void OnAttack()
    {
        AudioManager.Instance.PlaySFX("Attack");
    }

    public void OnUnitAdd()
    {
        AudioManager.Instance.PlaySFX("Add");
    }

    public void OnUnitDestory()
    {
        AudioManager.Instance.PlaySFX("Destroy");

        if (visualEffectManager != null)
            visualEffectManager.TriggerShake(0.5f, 0.3f); // Call screen jitter, set duration and jitter amplitude.
    }
}
