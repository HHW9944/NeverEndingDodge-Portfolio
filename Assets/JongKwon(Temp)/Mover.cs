using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 15f; // 이동 속도

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 월드 좌표에서 x 값이 20 이상일 경우 오브젝트 파괴
        if (transform.position.x >= 20f)
        {
            Destroy(gameObject);
        }
    }
}
