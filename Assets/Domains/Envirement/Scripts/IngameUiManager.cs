using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class IngameUiManager : MonoBehaviour
{
    private int timer = 2000;
    public Text timerText;
    public Image[] life;

    public GameObject[] UI;

    void Start()
    {
        StartCoroutine(TimerCountDown());
    }

    IEnumerator TimerCountDown()
    {
        while (timer > 0)
        {
            timer--;
            int minute = timer / 60;
            int second = timer % 60;
            timerText.text = string.Format("{0:00} : {1:00}", minute, second);
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("타임아웃. 생존하셨습니다.");
        /*UI[1].SetActive(true);*/

    }

    public void OnDie()
    {
        /*UI[0].SetActive(true);*/
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("게임이 다시 시작됩니다.");
    }

    /*public void OnDamage(int currentLife, int damage)
    {
        for (int life = currentLife; life < currentLife + damage; life++)
        {
            this.life[life].color = new Color(1, 1, 1, 0.1f);
        }
    }*/
}
