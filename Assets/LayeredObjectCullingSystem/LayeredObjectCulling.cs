using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

public class LayeredObjectCulling : MonoBehaviour {
    #region classes
    [System.Serializable]
    public class CullingLayer {
        [Range(0, 31)]
        public int layerId;
        [Space]
        public float layerBaseDistance;
        public float layerShadowDistance;
    }
    #endregion

    #region fields
    // publics
    [SerializeField]
#if ODIN_INSPECTOR
    [Title("General")]
#else
    [Header("General")]
#endif
    private bool cullSpherically = false;
    [SerializeField]
    private bool multiplierIsLodBias = false;

#if ODIN_INSPECTOR
    [HideIf("multiplierIsLodBias", false)]
    [Indent(2)]
#endif
    [SerializeField]
    private float multiplier = 1f;


#if ODIN_INSPECTOR
    [PropertySpace]
    [Title("Layers")]
#else
    [Header("Layers")]
#endif
    [SerializeField]
    private List<CullingLayer> layerList;

    // privates
    private List<Camera> cameraInstances;
    private List<Light> lightInstances;
    #endregion

    #region properties
    // publics
    public bool CullSpherically { get => cullSpherically; }
    public List<CullingLayer> LayerList { get => layerList; }
    public bool MultiplierIsLodBias {
        get => multiplierIsLodBias;
        set {
            multiplierIsLodBias = value;
            Refresh();
        }
    }
    public float Multiplier {
        get => multiplier;
        set {
            multiplier = value;
            Refresh();
        }
    }

    // privates
    public List<Camera> CameraInstances {
        get {
            if (cameraInstances == null) {
                cameraInstances = new List<Camera>();
            }
            return cameraInstances;
        }
        set => cameraInstances = value;
    }
    public List<Light> LightInstances {
        get {
            if (lightInstances == null) {
                lightInstances = new List<Light>();
            }
            return lightInstances;
        }
        set => lightInstances = value;
    }
    #endregion

    #region methods
    private void OnEnable() {
        StartCoroutine(CheckLodBias(QualitySettings.lodBias));
    }
    /// <summary>
    /// Sets the culling values to all the registered instances
    /// </summary>
    private void Refresh() {
        if(LayerList.Count == 0) {
            Debug.LogWarning("Failed to refresh LayeredObjectCulling system - No layers set");
            return;
        }

        // setup an empty float array
        var layerCullDistances = new float[32];
        var layerShadowCullDistances = new float[32];
        var multiplier = MultiplierIsLodBias ? QualitySettings.lodBias : Multiplier;
        
        // set culling on every layer
        foreach (CullingLayer lc in LayerList) {
            layerCullDistances[lc.layerId] = lc.layerBaseDistance * multiplier;
            layerShadowCullDistances[lc.layerId] = lc.layerShadowDistance * multiplier;
        }

        // apply to cameras
        foreach (Camera c in CameraInstances) {
            c.layerCullDistances = layerCullDistances;
            c.layerCullSpherical = CullSpherically;
        }
        // apply to lights
        foreach (Light l in LightInstances) {
            l.layerShadowCullDistances = layerShadowCullDistances;
        }
    }
    /// <summary>
    /// Checks for changes in LOD Bias 
    /// </summary>
    /// <param name="lastBias"></param>
    /// <returns></returns>
    private IEnumerator CheckLodBias(float lastBias) {
        // wait 1 second
        yield return new WaitForSeconds(1);

        // check if bias has changed
        if (MultiplierIsLodBias && lastBias != QualitySettings.lodBias) {
            Refresh();
        }

        // rinse and repeat
        if (enabled) {
            StartCoroutine(CheckLodBias(QualitySettings.lodBias));
        }
    }

    /// <summary>
    /// Register a camera instance to the layered culling system
    /// </summary>
    /// <param name="instance"></param>
    public void RegisterCamera(Camera instance) {
        // already added, nothing left to do here
        if (CameraInstances.Contains(instance))
            return;

        CameraInstances.Add(instance);

        //todo - only refresh the newly added camera (save performance)
        Refresh();
    }
    /// <summary>
    /// Register a light instance to the layered culling system
    /// </summary>
    /// <param name="instance"></param>
    public void RegisterLight(Light instance) {
        // already added, nothing left to do here
        if (LightInstances.Contains(instance))
            return;

        //todo - only refresh the newly added light (save performance)
        LightInstances.Add(instance);
        Refresh();
    }

    /// <summary>
    /// Unregister a camera instance from the layered culling system
    /// </summary>
    /// <param name="instance"></param>
    public void UnRegisterCamera(Camera instance) {
        // already removed, nothing left to do here
        if (!CameraInstances.Contains(instance))
            return;

        CameraInstances.Remove(instance);

        // reset culling distances to defaults
        instance.layerCullDistances = new float[32];
    }
    /// <summary>
    /// Unregister a light instance from the layered culling system
    /// </summary>
    /// <param name="instance"></param>
    public void UnRegisterLight(Light instance) {
        // already removed, nothing left to do here
        if (!LightInstances.Contains(instance))
            return;

        LightInstances.Remove(instance);

        // reset culling distances to defaults
        instance.layerShadowCullDistances = new float[32];
    }
    #endregion
}
