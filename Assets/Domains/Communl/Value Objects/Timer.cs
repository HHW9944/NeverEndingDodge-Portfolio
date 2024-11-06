using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Tooltip("초기 값 설정")]
    public int InitValue;
    [Tooltip("Timer 생성 시 자동으로 시작할지 여부")]
    public bool PlayOnAwake = true;

    [Header("Events")]
    [Tooltip("Time Out 시 호출되는 이벤트")]
    public UnityEvent onTimeout;

    [Tooltip("Time Second가 변경될 때마다 호출되는 이벤트, previous, current")]
    public UnityEvent<int, int> OnChanged;

    [Header("Timed Events")]
    public List<ValueReachedEvent<int>> TimedEvents = new List<ValueReachedEvent<int>>();

    [Header("Debug")]
    [Tooltip("디버그 모드 On/Off")]
    public bool IsDebug = false;

    private HashSet<int> _triggeredTimes = new HashSet<int>();
    private bool _isStopped = false;
    private float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        private set
        {
            if (_isStopped)
            {
                return;
            }
            
            float previous_value = _value;
            _value = Math.Max(value, 0);

            // 매 초마다 발생되는 로직
            if (HasIntegerPartChanged(previous_value, _value))
            {
                OnChanged?.Invoke((int) previous_value, (int) _value);

                foreach (var timedEvent in TimedEvents)
                {
                    if (_value <= timedEvent.TriggerValue && !_triggeredTimes.Contains(timedEvent.TriggerValue))
                    {
                        timedEvent.OnTrigger?.Invoke();
                        _triggeredTimes.Add(timedEvent.TriggerValue);
                    }
                }
            }

            if (_value <= 0)
            {
                onTimeout?.Invoke();
            }

            if (IsDebug)
            {
                Debug.Log("Time : " + Value);
            }
        }
    }

    void Start()
    {
        _value = InitValue;
        _triggeredTimes.Clear();

        if (!PlayOnAwake)
        {
            Stop();
        }
    }

    void Update()
    {
        if (_isStopped || Value <= 0)
        {
            return;
        }

        Value -= Time.deltaTime;
    }

    public void Reset()
    {
        _value = InitValue;
        _triggeredTimes.Clear();
    }

    public void Stop()
    {
        _isStopped = true;
    }

    public void Resume()
    {
        _isStopped = false;
    }

    private bool HasIntegerPartChanged(float prev, float current)
    {
        int prevInt = (int)prev;
        int currentInt = (int)current;

        return prevInt != currentInt;
    }
}
