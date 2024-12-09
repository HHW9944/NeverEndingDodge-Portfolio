using UnityEngine;

public class TutorialEnemyAI : MonoBehaviour
{
    private GameObject player;
    public bool isAlwaysRecognizePlayer = true;
    public float recognitionDistance;
    public float rotationSpeed = 10.0f;
    public GameObject redMissile;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 플레이어 방향을 계산하여 적이 생성될 때 플레이어를 바로 바라보도록 설정
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}