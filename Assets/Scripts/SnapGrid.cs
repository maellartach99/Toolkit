using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGrid : MonoBehaviour
{
    [SerializeField] public Vector3 size = new Vector3(0.5f, 0.5f, 0.5f);

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying && transform.hasChanged)
            Snap();
    }

    private void Snap()
    {
        float x = Mathf.Round(transform.position.x / size.x) * size.x;
        float y = Mathf.Round(transform.position.y / size.y) * size.y;
        float z = Mathf.Round(transform.position.z / size.z) * size.z;
        transform.position = new Vector3(x, y, z);
    }
}
