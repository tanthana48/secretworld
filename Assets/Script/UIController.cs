using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    
    public Button startButton;
    public Button exitButton;
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("start-button");
        exitButton = root.Q<Button>("exit-button");
        

        startButton.clicked += StartButtonPressed;
        exitButton.clicked += QuitApplicaion;
    }

    void StartButtonPressed()
    {
        SceneManager.LoadScene("ChooseScene");
    }

    void QuitApplicaion()
    {
        Application.Quit();
    }

}
