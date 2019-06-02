using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredObjectCullingLight : MonoBehaviour
{
    private LayeredObjectCulling CullingSystem { get; set; }
    private void OnEnable() {
        CullingSystem = FindObjectOfType<LayeredObjectCulling>();
        if(CullingSystem == null) {
            Debug.LogWarning("No LayeredObjectCulling system found!");
            return;
        }
        CullingSystem.RegisterLight(GetComponent<Light>());
    }
    private void OnDisable() {
        if (CullingSystem == null) {
            Debug.LogWarning("No LayeredObjectCulling system found!");
            return;
        }
        CullingSystem.UnRegisterLight(GetComponent<Light>());
    }
}
