using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public string SceneName;
    
    public void Move()
    {
        SceneManager.LoadScene(SceneName);
    }
}
