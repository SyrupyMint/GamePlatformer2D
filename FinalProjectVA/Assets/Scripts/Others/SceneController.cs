using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    //[SerializeField] private Animator transAnim;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //IEnumerator LoadLevel()
    //{
    //    transAnim.SetTrigger("End");
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    //    transAnim.SetTrigger("Start");
    //}
}
