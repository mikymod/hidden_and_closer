using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SaveSystem.LevelStarted?.Invoke(SceneManager.GetActiveScene(), transform);
        }
    }
}
