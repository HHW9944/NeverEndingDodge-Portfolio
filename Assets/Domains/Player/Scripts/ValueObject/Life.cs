using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    public float InitValue;
    public UnityEvent onDeath;

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
    }
}
