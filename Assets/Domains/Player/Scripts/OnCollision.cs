using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    [Tooltip("충돌을 감지할 레이어들을 선택하세요.")]
    public LayerMask EnemyMask;

    public UnityEvent<Collision> OnCollide;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, EnemyMask))
        {
            OnCollide?.Invoke(collision);
        }
    }

    // 충돌한 오브젝트가 지정된 레이어에 속하는지 확인하는 함수
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}
