using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyRenderInEditor : MonoBehaviour
{
    // Cylinder mesh reference
    private Mesh cylinderMesh;

    // Radius and height of the cylinder
    public float radius = 0.1f;
    public float height = 0.5f;

    // Gizmo color
    public Color gizmoColor = Color.green;

    void OnDrawGizmos() // Runs in Edit mode
    {
        if (cylinderMesh == null)
        {
            // Create a temporary cylinder GameObject and extract its mesh
            GameObject tempCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinderMesh = tempCylinder.GetComponent<MeshFilter>().sharedMesh;

            // Destroy the temporary GameObject to avoid it appearing in the scene
            DestroyImmediate(tempCylinder);  // Use DestroyImmediate in Editor mode
        }

        Gizmos.color = gizmoColor;

        // Draw the cylinder mesh using Gizmos
        Gizmos.DrawMesh(
            cylinderMesh,
            transform.position,
            transform.rotation,
            new Vector3(radius, height / 2f, radius) // Adjust scale based on radius and height
        );
    }
}
