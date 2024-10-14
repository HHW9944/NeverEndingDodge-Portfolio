using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorDirection : MonoBehaviour
{
    public Transform indicatorArea;      // indicatorArea ������Ʈ (������ ��ġ�� �� ������Ʈ)
    public Transform middlePoint;    // MiddlePoint ������Ʈ
    public Transform indicator;           // ���� ������Ʈ
    public float distanceFromMiddle;

    void Update()
    {
        distanceFromMiddle = GameManager.distanceFromMiddle;

        // distanceFromMiddle�� ���� indicatorArea�� indicator Ȱ��ȭ/��Ȱ��ȭ
        if (distanceFromMiddle < 10f || distanceFromMiddle > 120f)
        {
            indicator.gameObject.SetActive(false);
        }
        else
        {
            indicator.gameObject.SetActive(true);
        }

        // indicatorArea �Ǵ� indicator�� null�� ��� �Լ� ����
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
