using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cost : MonoBehaviour
{
    [Tooltip("초당 재생되는 Cost 양 (CPS)")]
    public float CostRegen = 1f;

    [Tooltip("Cost의 최대치")]
    public float MaxCost = 100f;

    [Tooltip("Cost가 재생되는지 여부")]
    public bool IsRegen = true;

    [Tooltip("시작 시 Cost가 최대치인지 여부")]
    public bool IsFullChargeAtStart = true;

    [Tooltip("Cost 값이 변경될 때 호출되는 이벤트, previous, current")]
    public UnityEvent<float, float> OnChanged;

    [Header("Cost Ratio Reached Events")]
    [Tooltip("특정 비율에 도달했을 때 호출되는 이벤트. TriggerValue는 0~1 사이의 값이어야 함.")]
    public List<ValueReachedEvent<float>> ReachedEvents = new List<ValueReachedEvent<float>>();

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
        set
        {
            float previous_value = _value;
            // _value를 0 이상 MaxCost 이하로 제한
            _value = Mathf.Clamp(value, 0f, MaxCost);
            OnChanged?.Invoke(previous_value, _value);

            foreach (var reachedEvent in ReachedEvents)
            {
                float triggerValue = reachedEvent.TriggerValue;

                if (_value / MaxCost <= triggerValue && !_cooldownActive.Contains(triggerValue))
                {
                    reachedEvent.OnTrigger?.Invoke();
                    StartCoroutine(CooldownCoroutine(triggerValue));
                }
            }

            if (IsDebug)
            {
                Debug.Log("Cost : " + Value);
                Debug.Log("Cost Ratio : " + _value / MaxCost);
            }
        }
    }

    void Start()
    {
        if (IsFullChargeAtStart)
        {
            _value = MaxCost;
        }
        else
        {
            _value = 0f;
        }
    }

    void Update()
    {
        if (IsRegen)
        {
            RegenerateCost();
        }
    }

    public void UseCost(float cost)
    {
        Value -= cost;
    }

    private void RegenerateCost()
    {
        if (Value >= MaxCost)
        {
            return;
        }

        Value += CostRegen * Time.deltaTime;
    }

    private IEnumerator CooldownCoroutine(float triggerValue)
    {
        // 쿨타임 시작
        _cooldownActive.Add(triggerValue);
        yield return new WaitForSeconds(ReachedCooldown);
        // 쿨타임 끝
        _cooldownActive.Remove(triggerValue);
    }
}
