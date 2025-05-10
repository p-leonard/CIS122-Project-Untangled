using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Text pointsText;
    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + "POINTS";
    }

    // Update is called once per frame
    public void RestartButton()
    {
        SceneLoader.LoadGameplayScene();
        
    }

    public void ExitButton()
    {
        SceneLoader.LoadMainMenu();
    }
}
