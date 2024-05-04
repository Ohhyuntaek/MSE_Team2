using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditAccountButtonHandler : MonoBehaviour
{
    [SerializeField] private Button editAccountButton;
    [SerializeField] private GameObject editAccountInfos;

    private Sprite originalSprite;

    private void Start(){
        editAccountButton.onClick.AddListener(HandleEditing);
        originalSprite = editAccountButton.GetComponent<Image>().sprite;
    }

    private void HandleEditing(){
        if (editAccountButton.gameObject.GetComponent<Image>().sprite == originalSprite){
            editAccountInfos.gameObject.SetActive(true);
            editAccountButton.gameObject.GetComponent<Image>().sprite = null;
        }
        else {
            editAccountInfos.gameObject.SetActive(false);
            editAccountButton.gameObject.GetComponent<Image>().sprite = originalSprite;
        }
    }
}
