using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_CameraFocus : MonoBehaviour {

    public float halfXBounds;
    public float halfYBounds;
    public float halfZBounds;

    [HideInInspector]
    public Bounds focusBounds;

    void Update () {
        Vector3 pos = transform.position;
        Bounds bounds = new Bounds();
        bounds.Encapsulate(new Vector3(pos.x - halfXBounds, pos.y - halfYBounds, pos.z - halfZBounds));
        bounds.Encapsulate(new Vector3(pos.x + halfXBounds, pos.y + halfYBounds, pos.z + halfZBounds));
        focusBounds = bounds;
    }
}
