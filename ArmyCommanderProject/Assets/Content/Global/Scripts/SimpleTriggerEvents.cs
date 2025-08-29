using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SimpleTriggerEvents : MonoBehaviour
{
    [SerializeField] private LayerMask _readLayerMask;

    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerStayEvent;
    public UnityEvent OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject))
            OnTriggerEnterEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsInLayerMask(other.gameObject))
            OnTriggerStayEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject))
            OnTriggerExitEvent?.Invoke();
    }

    private bool IsInLayerMask(GameObject obj)
    {
        return (_readLayerMask.value & (1 << obj.layer)) != 0;
    }
}
