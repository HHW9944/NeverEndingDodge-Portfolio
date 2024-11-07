using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Tooltip("초기 값 설정")]
    public int InitValue;
    public UnityEvent onDeath;

    [Tooltip("데미지를 입었을 때 호출되는 이벤트, currentLife, damage")]
    public UnityEvent<int, int> OnDamaged;

    [Tooltip("Life 값이 변경될 때 호출되는 이벤트, previous, current")]
    public UnityEvent<int, int> OnChanged;

    [Header("Life Ratio Reached Events")]
    [Tooltip("특정 비율에 도달했을 때 호출되는 이벤트. TriggerValue는 0~1 사이의 값이어야 함.")]
    public List<ValueReachedEvent<float>> ReachedEvents = new List<ValueReachedEvent<float>>();

    [Tooltip("특정 비율에 도달했을 때 호출되는 이벤트가 발생한 후 재발생되기까지의 시간")]
    public float ReachedCooldown = 30f;
    private HashSet<float> _cooldownActive = new HashSet<float>();

    [Header("Debug")]
    [Tooltip("디버그 모드 On/Off")]
    public bool IsDebug = false;

    private bool _isDead = false;
    private int _value;
    public int Value
    {
        get
        {
            return _value;
        }
        private set
        {
            int previous_value = _value;
            _value = Math.Max(value, 0);
            OnChanged?.Invoke(previous_value, _value);

            foreach (var reachedEvent in ReachedEvents)
            {
                float triggerValue = reachedEvent.TriggerValue;

                if ((float) _value / MaxValue <= triggerValue && !_cooldownActive.Contains(triggerValue))
                {
                    reachedEvent.OnTrigger?.Invoke();
                    StartCoroutine(CooldownCoroutine(triggerValue));
                }
            }

            if (_value <= 0 && !_isDead)
            {
                _isDead = true;
                onDeath?.Invoke();
            }

            if (IsDebug)
            {
                Debug.Log("Life : " + _value);
                Debug.Log("Life Ratio : " + (float) _value / MaxValue);
            }
        }
    }

    public int MaxValue => InitValue;

    void Start()
    {
        _value = InitValue;
    }

    public void TakeDamage(int damage)
    {
        Value -= damage;
        OnDamaged?.Invoke(Value, damage);
    }

    public void Reset()
    {
        _value = InitValue;
        _cooldownActive.Clear();
    }

    private IEnumerator CooldownCoroutine(float triggerValue)
    {
        _cooldownActive.Add(triggerValue);
        yield return new WaitForSeconds(ReachedCooldown);
        _cooldownActive.Remove(triggerValue);
    }
}