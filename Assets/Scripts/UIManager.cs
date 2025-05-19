using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using SgLib;

public class UIManager : MonoBehaviour
{
    // Referências aos elementos da UI
    [Header("Object References")]
    public PlayerController playerController;        // Controlador do jogador
    public GameObject mainCanvas;                    // Canvas principal
    public GameObject header;                        // Cabeçalho da tela
    public GameObject title;                         // Título do jogo
    public Text coinText;                           // Texto da quantidade de moedas
    public GameObject playBtn;                      // Botão de jogar
    public GameObject restartBtn;                   // Botão de reiniciar
    public GameObject menuButtons;                  // Grupo de botões do menu
    public GameObject settingsUI;                   // Interface de configurações
    public GameObject soundOnBtn;                   // Botão de som ligado
    public GameObject soundOffBtn;                  // Botão de som desligado
    public GameObject musicOnBtn;                   // Botão de música ligada
    public GameObject musicOffBtn;                  // Botão de música desligada
    public GameObject velocityBoard;                // Painel de velocidade
    public Text velocityText;                       // Texto da velocidade
    public GameObject velocityNote;                 // Nota de velocidade

    // Variáveis privadas
    float timeCount;                                // Contador de tempo
    float maxSpeed;                                 // Velocidade máxima atingida

    // Inscrição nos eventos do jogo
    void OnEnable()
    {
        GameManager.GameStateChanged += GameManager_GameStateChanged;
    }

    // Cancelamento da inscrição nos eventos
    void OnDisable()
    {
        GameManager.GameStateChanged -= GameManager_GameStateChanged;
    }

    // Inicialização
    void Start()
    {
        Reset();
        ShowStartUI();
    }

    // Atualização a cada frame
    void Update()
    {
        // Atualiza a velocidade durante o jogo
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 0.25f)
            {
                velocityText.text = ((int)playerController.currentSpeed).ToString();
                if (maxSpeed < playerController.currentSpeed)
                    maxSpeed = playerController.currentSpeed;
                timeCount = 0;
            }
        }

        // Atualiza os textos da UI
        coinText.text = CoinManager.Instance.Coins.ToString();

        // Atualiza os botões de som e música se a tela de configurações estiver ativa
        if (settingsUI.activeSelf)
        {
            UpdateSoundButtons();
            UpdateMusicButtons();
        }
    }

    // Manipulador de mudança de estado do jogo
    void GameManager_GameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Playing)
        {
            ShowGameUI();
        }
        else if (newState == GameState.PreGameOver)
        {
            // Antes do game over, quando o jogo ainda pode ser recuperado
        }
        else if (newState == GameState.GameOver)
        {
            Invoke("ShowGameOverUI", 1.2f);
        }
    }

    // Reseta todos os elementos da UI para o estado inicial
    void Reset()
    {
        mainCanvas.SetActive(true);
        header.SetActive(false);
        title.SetActive(false);
        playBtn.SetActive(false);
        menuButtons.SetActive(false);
        velocityBoard.SetActive(false);
        settingsUI.SetActive(false);
    }

    // Inicia o jogo
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    // Finaliza o jogo
    public void EndGame()
    {
        GameManager.Instance.GameOver();
    }

    // Reinicia o jogo
    public void RestartGame()
    {
        GameManager.Instance.RestartGame(0.2f);
    }

    // Mostra a tela inicial
    public void ShowStartUI()
    {
        settingsUI.SetActive(false);      
        header.SetActive(true);
        title.SetActive(true);
        playBtn.SetActive(true);
        restartBtn.SetActive(false);
        menuButtons.SetActive(true);
        velocityBoard.SetActive(false);
    }

    // Mostra a UI durante o jogo
    public void ShowGameUI()
    {
        header.SetActive(true);
        title.SetActive(false);
        playBtn.SetActive(false);
        menuButtons.SetActive(false);
        velocityNote.SetActive(false);
        velocityBoard.SetActive(true);
    }

    // Mostra a UI de game over
    public void ShowGameOverUI()
    {
        header.SetActive(true);
        title.SetActive(false);
        playBtn.SetActive(false);
        restartBtn.SetActive(true);
        menuButtons.SetActive(true);
        settingsUI.SetActive(false);
        velocityText.text = ((int)maxSpeed).ToString();
        velocityNote.SetActive(true);
    }

    // Mostra a tela de configurações
    public void ShowSettingsUI()
    {
        settingsUI.SetActive(true);
    }

    // Esconde a tela de configurações
    public void HideSettingsUI()
    {
        settingsUI.SetActive(false);
    }

    // Alterna o som
    public void ToggleSound()
    {
        SoundManager.Instance.ToggleSound();
    }

    // Alterna a música
    public void ToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();
    }

    // Abre a página de avaliação do app
    public void RateApp()
    {
        Utilities.RateApp();
    }

    // Abre a página do Twitter
    public void OpenTwitterPage()
    {
        Utilities.OpenTwitterPage();
    }

    // Abre a página do Facebook
    public void OpenFacebookPage()
    {
        Utilities.OpenFacebookPage();
    }

    // Toca o som de clique do botão
    public void ButtonClickSound()
    {
        Utilities.ButtonClickSound();
    }

    // Atualiza os botões de som
    void UpdateSoundButtons()
    {
        if (SoundManager.Instance.IsSoundOff())
        {
            soundOnBtn.gameObject.SetActive(false);
            soundOffBtn.gameObject.SetActive(true);
        }
        else
        {
            soundOnBtn.gameObject.SetActive(true);
            soundOffBtn.gameObject.SetActive(false);
        }
    }

    // Atualiza os botões de música
    void UpdateMusicButtons()
    {
        if (SoundManager.Instance.IsMusicOff())
        {
            musicOffBtn.gameObject.SetActive(true);
            musicOnBtn.gameObject.SetActive(false);
        }
        else
        {
            musicOffBtn.gameObject.SetActive(false);
            musicOnBtn.gameObject.SetActive(true);
        }
    }
}
