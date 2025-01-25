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
        if (menuCredits.activeSelf) // Check if menuCredits is currently active
        {
            menuCredits.SetActive(false); // Deactivate it
        }
        else
        {
            menuCredits.SetActive(true); // Activate it
        }
    }

    public void ShowInstructions()
    {
        AudioManager.Instance.PlaySFX("SFX_UIClick");
        menuInstructions.SetActive(true);
    }
}
