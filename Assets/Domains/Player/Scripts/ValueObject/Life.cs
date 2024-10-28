using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [Tooltip("초기 값 설정")]
    public float InitValue;
    public UnityEvent onDeath;
<<<<<<< Updated upstream
=======
    public UIManager uiManager;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
            
            if (Value == 0)
            {
                onDeath.Invoke();
            }
=======
>>>>>>> Stashed changes
        }
    }

    void Start()
    {
        _value = InitValue;

        // 생명이 0 이하일 때 GameOver 이벤트를 트리거
        onDeath.AddListener(() =>
        {
            if (_value <= 0)
            {
                GameManager.instance.GameOver();
            }
        });
    }

    public void DecreaseLife()
    {
<<<<<<< Updated upstream
        Value--;

        if (IsDebug)
            Debug.Log("Life : " + Value);
=======
        if (IsDebug && Value > 1)
        {
            Value--;
            uiManager.life[Value].color = new Color(1, 1, 1, 0.1f);
            Debug.Log("Life : " + Value);
        }
        else
        {
            Value--;
            uiManager.life[0].color = new Color(1, 1, 1, 0.1f);
            onDeath.Invoke();
        }
    }

    // 새로운 초기화 메서드 추가
    public void ResetLife()
    {
        
        _value = InitValue; // 초기 값으로 생명 복원
        Debug.Log("Life 초기화됨 : " + _value);

        // UI 초기화
        for (int i = 0; i < uiManager.life.Length; i++)
        {
            uiManager.life[i].color = new Color(1, 1, 1, 1f); // 생명 UI를 초기 상태로 복원
        }
>>>>>>> Stashed changes
    }
}
