using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private Material newMaterial;
    private Material oldMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var renderer = other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            oldMaterial = renderer.material;
            renderer.material = newMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var renderer = other.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            renderer.material = oldMaterial;
        }
    }
}
