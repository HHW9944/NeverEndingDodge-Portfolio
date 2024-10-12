using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Tooltip("초기 값 설정")]
    public float InitValue;
    public UnityEvent onDeath;

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
            _value = Math.Max(value, 0);
            
            if (Value == 0)
            {
                onDeath.Invoke();
            }
        }
    }

    void Start()
    {
        _value = InitValue;
    }

    public void DecreaseLife()
    {
        Value--;

        if (IsDebug)
            Debug.Log("Life : " + Value);
    }
}
