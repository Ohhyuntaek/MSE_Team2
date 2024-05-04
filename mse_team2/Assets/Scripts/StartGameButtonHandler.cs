using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButtonHandler : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    private Sprite originalSprite;
    [SerializeField] private Button EasyModeButton;
    [SerializeField] private Button HardModeButton;

    private void Start() {
        startGameButton.onClick.AddListener(ChangeButtons);    
        originalSprite = startGameButton.gameObject.GetComponent<Image>().sprite;
    }

    private void ChangeButtons(){
        if (startGameButton.gameObject.GetComponent<Image>().sprite == originalSprite){
            EasyModeButton.gameObject.SetActive(true);
            HardModeButton.gameObject.SetActive(true);
            startGameButton.gameObject.GetComponent<Image>().sprite = null;
        }
        else {
            EasyModeButton.gameObject.SetActive(false);
            HardModeButton.gameObject.SetActive(false);
            startGameButton.gameObject.GetComponent<Image>().sprite = originalSprite;
        }
        
    }
}
