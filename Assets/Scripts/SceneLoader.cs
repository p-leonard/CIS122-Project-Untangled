using UnityEngine;
using UnityEngine.SceneManagement;
// Make class static, remove inheritance from MonoBehavior.
// We make this class static because a) It's stateless, and b) we don't want to allow instantiation anyway.

// So instead of:
// SceneLoader loader = new SceneLoader();
// loader.LoadWinScene();

// We can do:
// SceneLoader.LoadWinScene();

public static class SceneLoader
{
    // We need this object to remain the same across scenes.
    // To make that easy, we'll have scene names be defined in code, as constants.
    private const string GameplayScene = "GameplayScene";
    private const string MainMenuScene = "Menu";
    private const string WinScene = "WinScreen";
    private const string LoseScene = "DeathScreen";
    // Restart game from menu or retry
    public static void LoadGameplayScene() => SceneManager.LoadScene(GameplayScene);
    // Return to main menu
    public static void LoadMainMenu() => SceneManager.LoadScene(MainMenuScene);
    // Player wins
    public static void LoadWinScene() => SceneManager.LoadScene(WinScene);
    // Player loses
    public static void LoadGameOverScene() => SceneManager.LoadScene(LoseScene);
    // Exit application
    public static void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}