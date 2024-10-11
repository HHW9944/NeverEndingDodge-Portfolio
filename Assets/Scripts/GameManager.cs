using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴으로 모든 씬에서 GamaManager에 쉽게 접근할 수 있도록 함.
    public static GameManager instance;

    private void Awake()
    {
        //싱글톤 패턴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //씬이 바뀌어도 GameManager가 파괴되지 않도록..

        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    //씬 전환 함수
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
