using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredObjectCullingCamera : MonoBehaviour
{
    private LayeredObjectCulling CullingSystem { get; set; }
    private void OnEnable() {
        CullingSystem = FindObjectOfType<LayeredObjectCulling>();
        if(CullingSystem == null) {
            Debug.LogWarning("No LayeredObjectCulling system found!");
            return;
        }
        CullingSystem.RegisterCamera(GetComponent<Camera>());
    }
    private void OnDisable() {
        if (CullingSystem == null) {
            Debug.LogWarning("No LayeredObjectCulling system found!");
            return;
        }
        CullingSystem.UnRegisterCamera(GetComponent<Camera>());
    }
}
