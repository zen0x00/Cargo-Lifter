using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject diffSelectMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject analyticsStatsTab;
    [SerializeField] private GameObject analyticsGraphTab;
    [SerializeField] private GameObject leaderBoard;
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] public CraneRotate crane;
    [SerializeField] public Hook hook;
    [SerializeField] private GameManager gameManager;

    bool leaderboardShown = false;

    public static bool isRestart = false;
    public static GameManager.diffLevel restartLevel;

    private void Start()
    {
        if (isRestart)
        {
            isRestart = false;

            startMenu.SetActive(false);
            diffSelectMenu.SetActive(false);
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(false);
            analyticsStatsTab.SetActive(false);
            analyticsGraphTab.SetActive(false);
            leaderBoard.SetActive(false);
            gameOverScreen.SetActive(false);

            switch (restartLevel)
            {
                case GameManager.diffLevel.Begginer:
                    LoadBeginnerLevel();
                    break;

                case GameManager.diffLevel.Intermidiate:
                    LoadIntermediateLevel();
                    break;

                case GameManager.diffLevel.Expert:
                    LoadExpertLevel();
                    break;
            }

            return;
        }

        GoToStart();

        startMenu.SetActive(true);
        diffSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        analyticsStatsTab.SetActive(false);
        analyticsGraphTab.SetActive(false);
        leaderBoard.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if (gameManager.sessionEnded && !leaderboardShown)
        {
            Invoke(nameof(ShowLeaderBoard), 6f);
            leaderboardShown = true;
        }

        if (leaderboardShown)
        {
            crane.StopRotation();
        }
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        diffSelectMenu.SetActive(true);

        analyticsStatsTab.SetActive(false);
        analyticsGraphTab.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadBeginnerLevel()
    {
        restartLevel = GameManager.diffLevel.Begginer;

        diffSelectMenu.SetActive(false);
        pauseMenu.SetActive(false);

        crane.StartRotation();
        hook.isGameStarted = true;

        crane.rotationSpeed = 15f;
        hook.ropeSpeed = 18f;

        gameManager.Level = GameManager.diffLevel.Begginer;
    }

    public void LoadIntermediateLevel()
    {
        restartLevel = GameManager.diffLevel.Intermidiate;

        diffSelectMenu.SetActive(false);
        pauseMenu.SetActive(false);

        crane.StartRotation();
        hook.isGameStarted = true;

        crane.rotationSpeed = 25f;
        hook.ropeSpeed = 12f;

        gameManager.Level = GameManager.diffLevel.Intermidiate;
    }

    public void LoadExpertLevel()
    {
        restartLevel = GameManager.diffLevel.Expert;

        diffSelectMenu.SetActive(false);
        pauseMenu.SetActive(false);

        crane.StartRotation();
        hook.isGameStarted = true;

        crane.rotationSpeed = 40f;
        hook.ropeSpeed = 8f;

        gameManager.Level = GameManager.diffLevel.Expert;
    }

    public void GoToStart()
    {
        startMenu.SetActive(true);

        diffSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        analyticsStatsTab.SetActive(false);
        analyticsGraphTab.SetActive(false);
        leaderBoard.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        Time.timeScale = 0f;

    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;

    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        isRestart = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        isRestart = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowLeaderBoard()
    {
        gameOverScreen.SetActive(false);
        leaderBoard.SetActive(true);
    }

    public void ShowAnalyticsStatsTab()
    {
        leaderBoard.SetActive(false);
        analyticsStatsTab.SetActive(true);
    }

    public void ShowAnalyticsGraphTab()
    {
        analyticsStatsTab.SetActive(false);
        analyticsGraphTab.SetActive(true);
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameManager.sessionEnded = true;
    }
}