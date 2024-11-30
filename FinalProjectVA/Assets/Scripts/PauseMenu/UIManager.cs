using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private PlayerAimAndShoot playerAimAndShoot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If pause screen already active unpause and viceversa
            bool isPaused = !pauseScreen.activeInHierarchy;
            PauseGame(isPaused);
        }
    }

    public void PauseGame(bool status)
    {
        //If status == true pause | if status == false unpause
        pauseScreen.SetActive(status);

        //When pause status is true change timescale to 0 (time stops)
        //when it's false change it back to 1 (time goes by normally)
        Time.timeScale = status ? 0 : 1;
        playerAimAndShoot.SetPauseState(status);
    }

    public void MainMenu()
    {
        Time.timeScale = 1; // Ensure time scale is set to normal
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            // Stop play mode in the Unity Editor
            EditorApplication.isPlaying = false;
        #else
            // Quit the application in the build
            Application.Quit();
        #endif
    }
}
