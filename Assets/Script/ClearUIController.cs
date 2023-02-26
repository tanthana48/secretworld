using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ClearUIController : MonoBehaviour
{
    
    public Button backButton;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        backButton = root.Q<Button>("back-button");


        backButton.clicked += BackButtonPressed;
    }

    void BackButtonPressed()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
