using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLookAt : MonoBehaviour
{
    public List<GameObject> Targets;
    public Vector2 RotationSpeedRange;
    public Vector2 SwitchDelayRange;

    private int _currentTargetIndex = 0;
    private float _rotationSpeed;
    private float _switchDelay;

    void Start()
    {
        UpdateRandomValues();
    }

    void Update()
    {
        if (Targets.Count > 0)
        {
            Vector3 targetDirection = Targets[_currentTargetIndex].transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }

    private void UpdateRandomValues()
    {
        _rotationSpeed = Random.Range(RotationSpeedRange.x, RotationSpeedRange.y);
        _switchDelay = Random.Range(SwitchDelayRange.x, SwitchDelayRange.y);
    }

    public void AddTarget(GameObject target)
    {
        Targets.Add(target);
    }

    public void SwitchTarget()
    {
        StartCoroutine(SwitchTargetCoroutine());
    }

    private IEnumerator SwitchTargetCoroutine()
    {
        yield return new WaitForSeconds(_switchDelay);
        _currentTargetIndex = (_currentTargetIndex + 1) % Targets.Count;
        UpdateRandomValues();
    }
}