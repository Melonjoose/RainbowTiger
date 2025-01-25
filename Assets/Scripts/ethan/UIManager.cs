using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject menuMain;
    public GameObject menuInstructions;
    public GameObject menuCredits;
    public GameObject gameUI;

    public static bool isPaused = false;
    public static bool isPlaying = false;

    //ON DEATH WHAT HAPPENS

    void Start()
    {
        menuPause.SetActive(false);
        menuMain.SetActive(true);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPlaying)
            {
                if (isPaused)
                {
                    GameResume();
                }

                else
                {
                    GamePause();
                }
            }    
        }   
    }

    public void GameStart()
    {
        isPlaying = true;
        menuMain.SetActive(false);
        gameUI.SetActive(true );
    }

    public void GamePause()
    {
        AudioManager.Instance.PlaySFX("SFX_UIClick");
        menuPause.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void GameResume()
    {
        AudioManager.Instance.PlaySFX("SFX_UIClick");
        menuPause.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void MenuMain()
    {
        AudioManager.Instance.PlaySFX("SFX_UIClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        menuInstructions.SetActive(false);
        menuCredits.SetActive(false);
    }

    public void ShowCredits()
    {
        AudioManager.Instance.PlaySFX("SFX_UIClick");
        if (menuCredits.activeSelf)
        {
            menuCredits.SetActive(false);
        }
        else
        {
            menuCredits.SetActive(true);
        }

    }

        public void ShowInstructions()
        {
            AudioManager.Instance.PlaySFX("SFX_UIClick");
            if (menuInstructions.activeSelf)
            {
                menuInstructions.SetActive(false);
            }
            else
            {
                menuInstructions.SetActive(true);
            }
        }

    public void MuteOST()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.MuteOST();
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found");
        }
    }

    public void MuteSFX()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.MuteSFX();
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found");
        }
    }
}
