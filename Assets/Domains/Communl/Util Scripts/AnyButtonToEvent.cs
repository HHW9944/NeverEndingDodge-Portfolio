using UnityEngine;
using UnityEngine.Events;

public class AnyButtonToEvent : MonoBehaviour
{
    public UnityEvent OnClick;
    public bool IsLock = false;

    void Update()
    {
        if (Input.anyKeyDown && !IsLock)
        {
            OnClick?.Invoke();
        }
    }

    public void Unlock()
    {
        IsLock = false;
    }
}