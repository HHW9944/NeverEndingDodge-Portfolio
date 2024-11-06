using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ValueReachedEvent<T>
{
    [Tooltip("특정 값에 도달했을 때 호출되는 이벤트")]
    public UnityEvent OnTrigger;
    
    [Tooltip("이벤트가 발생할 특정 값")]
    public T TriggerValue;
}