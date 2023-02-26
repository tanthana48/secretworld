using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ChooseUIController : MonoBehaviour
{
    public Button easy;
    public Button normal;
    public Button hard;

    private GameSession gameSession;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        easy = root.Q<Button>("easy");
        normal = root.Q<Button>("normal");
        hard = root.Q<Button>("hard");

        gameSession = FindObjectOfType<GameSession>();

        easy.clicked += EasyPressed;
        normal.clicked += NormalPressed;
        hard.clicked += HardPressed;

    }

    void EasyPressed()
    {
        gameSession.SetPlayerLives(6);
        gameSession.SetModeBoost(1);
        SceneManager.LoadScene("Lv 1");
    }

    void NormalPressed()
    {
        gameSession.SetPlayerLives(4);
        gameSession.SetModeBoost(2);
        SceneManager.LoadScene("Lv 1");
    }

    void HardPressed()
    {
        gameSession.SetPlayerLives(2);
        gameSession.SetModeBoost(3);
        SceneManager.LoadScene("Lv 1");
    }
}
