using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController _instance = null;

    private GameObject MainMenuPanel,InGamePanel, OutGamePanel, LevelChoosePanel, GameInfoPanel, GameOverPanel ,WinPanel;

    private bool isPause = false;

    private bool isGameOver = false;

    public enum GameEventType
    {
        GAME_OVER,
        GAME_WIN,
        PAUSE,
        DANDELION_GET_PLAYER_WIND,
        DANDELION_GET_WIND
    }


    void Awake()
    {
        MainMenuPanel = transform.Find("MainMenuPanel").gameObject;
        if (MainMenuPanel == null) MainMenuPanel = Instantiate((GameObject)Resources.Load("Prefabs/UI/MainMenuPanel"), this.transform);
        InGamePanel = MainMenuPanel.transform.Find("InGamePanel").gameObject;
        OutGamePanel = MainMenuPanel.transform.Find("OutGamePanel").gameObject;
        LevelChoosePanel = MainMenuPanel.transform.Find("LevelChoosePanel").gameObject;
        GameInfoPanel = MainMenuPanel.transform.Find("GameInfoPanel").gameObject;
        GameOverPanel = MainMenuPanel.transform.Find("GameOverPanel").gameObject;
        WinPanel = MainMenuPanel.transform.Find("WinPanel").gameObject;

        if (MainMenuController._instance == null)
            _instance = this;
    }

    void Start()
    {
        OpenOutGamePanel();

        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.GAME_OVER, OnGameOver);
        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.PAUSE, OnGamePauseStateChange);
        GameEventDispatcher.GetInstance().AddEventListener(GameEventType.GAME_WIN, OnGameWin);

        if (!AudioController._instance.ChangeBGM("OutGameMusic"))
        {
            AudioController._instance.RegisterAudioClip("OutGameMusic", "Audio/Music/1ø™≥°“Ù¿÷");
            AudioController._instance.ChangeBGM("OutGameMusic");
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            GamePauseChange();
        }
    }

    void OnDestroy()
    {
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.PAUSE, OnGamePauseStateChange);
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.GAME_OVER, OnGameOver);
        GameEventDispatcher.GetInstance().RemoveEventListener(GameEventType.GAME_WIN, OnGameWin);
    }

    public void OpenInGamePanel()
    {
        HideAllSub();
        InGamePanel.SetActive(true);
    }

    public void OpenOutGamePanel()
    {
        HideAllSub();
        OutGamePanel.SetActive(true);
    }

    public void OpenChooseLevelPanel()
    {
        HideAllSub();
        LevelChoosePanel.SetActive(true);
    }

    public void OpenGameInfoPanel()
    {
        HideAllSub();
        GameInfoPanel.SetActive(true);
    }

    public void OpenGameOverPanel()
    {
        HideAllSub();
        GameOverPanel.SetActive(true);
    }

    public void OpenWinPanel()
    {
        HideAllSub();
        WinPanel.SetActive(true);
    }

    public void Restart()
    {
        isGameOver = false;
        Time.timeScale = 1;
        HideAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (!AudioController._instance.ChangeBGM("InGameMusic"))
        {
            AudioController._instance.RegisterAudioClip("InGameMusic", "Audio/Music/2”Œœ∑÷–“Ù¿÷");
            AudioController._instance.ChangeBGM("InGameMusic");
        }
    }

    public void LoadNextLevel()
    {
        isGameOver = false;
        Time.timeScale = 1;
        HideAll();
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        if (level > SceneManager.sceneCountInBuildSettings) level = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(level);

        if (!AudioController._instance.ChangeBGM("InGameMusic"))
        {
            AudioController._instance.RegisterAudioClip("InGameMusic", "Audio/Music/2”Œœ∑÷–“Ù¿÷");
            AudioController._instance.ChangeBGM("InGameMusic");
        }
    }

    public void BackToMainMenu()
    {
        UnityEngine.Cursor.visible = true;
        isGameOver = false;
        Time.timeScale = 1;
        OpenOutGamePanel();
        SceneManager.LoadScene(0);

        if (!AudioController._instance.ChangeBGM("OutGameMusic"))
        {
            AudioController._instance.RegisterAudioClip("OutGameMusic", "Audio/Music/1ø™≥°“Ù¿÷");
            AudioController._instance.ChangeBGM("OutGameMusic");
        }
    }

    public void GamePauseChange()
    {
        GameEventDispatcher.GetInstance().DispatchEvent(new BaseGameEvent(GameEventType.PAUSE,null,gameObject));
    }


    public void HideAllSub()
    {
        MainMenuPanel.SetActive(true);
        InGamePanel.SetActive(false);
        OutGamePanel.SetActive(false);
        LevelChoosePanel.SetActive(false);
        GameInfoPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    public void HideAll()
    {
        MainMenuPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnGameOver(BaseGameEvent gEvent)
    {
        isGameOver = true;
        OpenGameOverPanel();
        Time.timeScale = 0;


        if (!AudioController._instance.PlayAudioClip("GameOver"))
        {
            AudioController._instance.RegisterAudioClip("GameOver", "Audio/SoundFx/6 ß∞‹");
            AudioController._instance.PlayAudioClip("GameOver");
        }
    }

    private void OnGameWin(BaseGameEvent gEvent)
    {
        isGameOver = true;
        OpenWinPanel();
        Time.timeScale = 0;

        if (!AudioController._instance.PlayAudioClip("Win"))
        {
            AudioController._instance.RegisterAudioClip("Win", "Audio/SoundFx/7 §¿˚…˘“Ù");
            AudioController._instance.PlayAudioClip("Win");
        }
    }

    private void OnGamePauseStateChange(BaseGameEvent gEvent)
    {
        if (isPause)
        {
            UnityEngine.Cursor.visible = false;
            HideAll();
            Time.timeScale = 1;
        }
        else
        {
            UnityEngine.Cursor.visible = true;
            OpenInGamePanel();
            Time.timeScale = 0;
        }
        isPause = !isPause;
    }

}
