using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoComponent : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float radius;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
