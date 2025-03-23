using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerHelper : MonoBehaviour
{
    public void Awake()
    {
        if (!Debug.isDebugBuild)
            Destroy(gameObject);
    }
    public void OpenDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }
}
