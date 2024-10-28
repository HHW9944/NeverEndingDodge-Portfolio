using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Tooltip("초기 값 설정")]
    public int InitValue;
    public UnityEvent onDeath;
    public IngameUiManager ingameUiManager;

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


        }
    }

    void Start()
    {
        _value = InitValue;
    }

    public void DecreaseLife()
    {


        if (IsDebug && Value > 1)
        {
            Value--;
            ingameUiManager.life[Value].color = new Color(1, 1, 1, 0.1f);
            Debug.Log("Life : " + Value);

        }
        else
        {
            Value--;
            ingameUiManager.life[0].color = new Color(1, 1, 1, 0.1f);
            onDeath.Invoke();
        }

    }
}
