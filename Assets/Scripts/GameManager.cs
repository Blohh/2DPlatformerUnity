using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameState
{
    GS_PAUSEMENU,
    GS_GAME,
    GS_GAME_OVER,
    GS_LEVELCOMPLETED
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;
    public static GameManager instance;
    public Canvas MenuCanvas, PauseCanvas;
    public Text coinsText, enemyText, clockText;
    private int coins, keys, lives, enemys;
    private float time;
    public Image[] keysTab, livesTab;

    private void Awake()
    {
        instance = this;
        this.coins = 0;
        this.keys = 0;
        this.lives = 3;
        this.enemys = 0;
        this.time = 0.0f;
        for (int i=keys; i<this.keysTab.Length; i++)
        {
            this.keysTab[i].color = Color.grey;
        }
        for(int i=lives; i < this.livesTab.Length; i++)
        {
            this.livesTab[i].enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.InGame();

    }

    // Update is called once per frame
    void Update()
    {
        if(currentGameState == GameState.GS_GAME)
        {
            this.updateTimer();
        }
        if (Input.GetKey(KeyCode.Escape) && currentGameState == GameState.GS_PAUSEMENU)
        {
            this.InGame();
        }
        if (Input.GetKey(KeyCode.Escape) && currentGameState == GameState.GS_GAME)
        {
            this.PauseMenu();
        }
    }
    void SetGameState(GameState newGameState)
    {
        this.currentGameState = newGameState;
        if (newGameState == GameState.GS_GAME)
            this.MenuCanvas.enabled = true;
        else
            this.MenuCanvas.enabled = false;

        if (newGameState == GameState.GS_PAUSEMENU)
            this.PauseCanvas.enabled = true;
        else
            this.PauseCanvas.enabled = false;
    }
   
    public void InGame()
    {
        this.SetGameState(GameState.GS_GAME);
    }
    public void GameOver()
    {
        this.SetGameState(GameState.GS_GAME_OVER);
    }
    public void PauseMenu()
    {
        this.SetGameState(GameState.GS_PAUSEMENU);
    }
    public void LevelCompleted()
    {
        this.SetGameState(GameState.GS_LEVELCOMPLETED);
    }
    public void AddCoins()
    {
        this.coins += 1;
        coinsText.text = coins.ToString();
    }
    public void AddKeys()
    {
        this.keysTab[keys].color = Color.white;
        this.keys += 1;
    }
    public void AddLives()
    {
        this.livesTab[lives].enabled = true;
        this.lives += 1;
    }
    public void DeleteLives()
    {
        if (lives > 0)
        {
            this.lives -= 1;
            this.livesTab[lives].enabled = false;
        }
        else
        {
            this.GameOver();
            Debug.Log("Game over");
        }
    }
    public int GetCurrentKeysCount()
    {
        return keys;
    }
    public void AddEnemys()
    {
        this.enemys += 1;
        enemyText.text = enemys.ToString();
    }
    public void updateTimer()
    {
        this.time += Time.deltaTime;
        var ts = System.TimeSpan.FromSeconds(time);
        this.clockText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

    }

    public void onResumeButtonPressed()
    {
        this.InGame();
    }

    public void onRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onExitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
