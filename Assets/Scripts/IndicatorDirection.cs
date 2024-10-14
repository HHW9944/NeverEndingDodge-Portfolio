using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorDirection : MonoBehaviour
{
    public Transform indicatorArea;      // indicatorArea 오브젝트 (원뿔이 배치될 빈 오브젝트)
    public Transform middlePoint;    // MiddlePoint 오브젝트
    public Transform indicator;           // 원뿔 오브젝트
    public float distanceFromMiddle;

    void Update()
    {
        distanceFromMiddle = GameManager.distanceFromMiddle;

        // distanceFromMiddle에 따라 indicatorArea와 indicator 활성화/비활성화
        if (distanceFromMiddle < 10f || distanceFromMiddle > 120f)
        {
            indicator.gameObject.SetActive(false);
        }
        else
        {
            indicator.gameObject.SetActive(true);
        }

        // indicatorArea 또는 indicator이 null일 경우 함수 종료
        if (indicatorArea == null || indicator == null) return;
        //
        Vector3 directionToMiddlePoint = (middlePoint.position - indicatorArea.position).normalized;

        SphereCollider sphereCollider = indicatorArea.GetComponent<SphereCollider>();
        float sphereRadius = sphereCollider != null ? sphereCollider.radius * indicatorArea.lossyScale.x : 1f;

        Vector3 indicatorPosition = indicatorArea.position + directionToMiddlePoint * sphereRadius;

        indicator.position = indicatorPosition;

        Quaternion lookAtMiddleRotation = Quaternion.LookRotation(directionToMiddlePoint);

        indicator.rotation = lookAtMiddleRotation * Quaternion.Euler(90, 0, 0);
    }
}
