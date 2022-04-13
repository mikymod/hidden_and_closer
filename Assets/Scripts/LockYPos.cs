using UnityEngine;

public class LockYPos : MonoBehaviour {
    public float Position = 0.7f;

    private void Update() => transform.position = new Vector3(transform.position.x, Position, transform.position.z);
}
