using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils : MonoBehaviour
{
    public static Utils Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ExecuteAfterDelay(float delay, Action action)
    {
        StartCoroutine(ExecuteAfterDelayCoroutine(delay, action));
    }

    private IEnumerator ExecuteAfterDelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
