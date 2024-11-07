using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Distance : MonoBehaviour
{
    [Tooltip("Target과의 거리")]
    public Transform Target;

    [Header("Life Ratio Reached Events")]
    [Tooltip("특정 비율에 도달했을 때 호출되는 이벤트. TriggerValue는 0~1 사이의 값이어야 함.")]
    public List<ValueReachedEvent<float>> ReachedEvents = new List<ValueReachedEvent<float>>();

    [Tooltip("Value 값이 설정값보다 크거나 같은 경우를 따지는가?")]
    public bool IsGreaterThan = true;

    [Tooltip("특정 비율에 도달했을 때 호출되는 이벤트가 발생한 후 재발생되기까지의 시간")]
    public float ReachedCooldown = 30f;
    private HashSet<float> _cooldownActive = new HashSet<float>();

    [Header("Debug")]
    [Tooltip("디버그 모드 On/Off")]
    public bool IsDebug = false;

    private float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        private set
        {
            float previous_value = _value;
            _value = Math.Max(value, 0.0f);

            foreach (var reachedEvent in ReachedEvents)
            {
                float triggerValue = reachedEvent.TriggerValue;

                if (_cooldownActive.Contains(triggerValue))
                {
                    continue;
                }

                if (!HasIntegerPartChanged(previous_value, _value))
                {
                    continue;
                }

                if (IsGreaterThan)
                {
                    if (_value >= triggerValue && previous_value < triggerValue)
                    {
                        reachedEvent.OnTrigger?.Invoke();
                        StartCoroutine(CooldownCoroutine(triggerValue));
                    }
                }
                else
                {
                    if (_value <= triggerValue && previous_value > triggerValue)
                    {
                        reachedEvent.OnTrigger?.Invoke();
                        StartCoroutine(CooldownCoroutine(triggerValue));
                    }
                }
            }

            if (IsDebug)
            {
                Debug.Log("Time : " + Value);
            }
        }
    }

    void Start()
    {
        _cooldownActive.Clear();
    }

    void Update()
    {
        if (Target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, Target.position);
        Value = distance;
    }

    private bool HasIntegerPartChanged(float prev, float current)
    {
        int prevInt = (int)prev;
        int currentInt = (int)current;

        return prevInt != currentInt;
    }

    private IEnumerator CooldownCoroutine(float triggerValue)
    {
        _cooldownActive.Add(triggerValue);
        yield return new WaitForSeconds(ReachedCooldown);
        _cooldownActive.Remove(triggerValue);
    }
}
