using UnityEngine;
using UnityEngine.Events;

public class ClickToEvent : MonoBehaviour
{
    public UnityEvent OnClick;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick?.Invoke();
        }
    }
}
