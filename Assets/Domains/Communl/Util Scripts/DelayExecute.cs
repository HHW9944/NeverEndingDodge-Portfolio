using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayExecute : MonoBehaviour
{
    public float delayTime = 0f;
    public UnityEvent onDelayEnd;
    
    public void Execute()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(delayTime);
        onDelayEnd.Invoke();
    }
}
