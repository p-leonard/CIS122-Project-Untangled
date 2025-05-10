using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RestartButton()
    {
        SceneLoader.LoadGameplayScene();
    }

    public void ExitButton()
    {
        SceneLoader.LoadMainMenu();
    }
}
