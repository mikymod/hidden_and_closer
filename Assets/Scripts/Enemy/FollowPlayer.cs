using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public Transform Player;

    private void Update() => transform.position = Player.position;
}
