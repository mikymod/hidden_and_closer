using UnityEngine;
using UnityEngine.SceneManagement;

namespace HNC
{
    public class LevelEnding : MonoBehaviour
    {
        [SerializeField] private SaveSystem saveSystem;
        [SerializeField] private string sceneToLoad;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SaveSystem.LevelFinished?.Invoke(SceneManager.GetActiveScene());
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
