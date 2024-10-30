using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Tooltip("초기 값 설정")]
    public int InitValue;
    public UnityEvent onDeath;

    [Tooltip("데미지를 입었을 때 호출되는 이벤트, currentLife, damage")]
    public UnityEvent<int, int> OnDamaged;

    [Tooltip("디버그 모드 On/Off")]
    public bool IsDebug = false;

    private int _value;
    public int Value
    {
        get
        {
            return _value;
        }
        private set
        {
            _value = Math.Max(value, 0);

            if (IsDebug)
            {
                Debug.Log("Life : " + Value);
            }
        }
    }

    public int MaxValue
    {
        get
        {
            return InitValue;
        }
    }

    void Start()
    {
        _value = InitValue;
    }

    public void TakeDamage(int damage)
    {
        Value -= damage;
        OnDamaged?.Invoke(Value, damage);
    }
}