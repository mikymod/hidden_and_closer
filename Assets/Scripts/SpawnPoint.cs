using UnityEngine;
using UnityEngine.SceneManagement;

namespace HNC
{
    public class SpawnPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SaveSystem.LevelStarted?.Invoke(SceneManager.GetActiveScene(), transform);
            }
        }
    }
}
