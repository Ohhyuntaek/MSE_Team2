using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUsage : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBackgroundMusicByName("lobbyBgm");
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

    public void OnUnitDestory()
    {
        AudioManager.Instance.PlaySFX("Destroy");
    }
}
