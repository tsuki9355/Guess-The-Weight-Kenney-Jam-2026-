using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CenterMeshPivot : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Center Mesh On This Transform")]
    void CenterOnSelf()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("No renderers found under " + name);
            return;
        }

        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
            bounds.Encapsulate(r.bounds);

        Vector3 offset = transform.position - bounds.center;

        foreach (Transform child in transform)
        {
            Undo.RecordObject(child, "Center Mesh Pivot");
            child.position += offset;
        }

        Debug.Log($"Shifted children of {name} by {offset} to center the mesh on this transform's origin.");
    }
#endif
}
