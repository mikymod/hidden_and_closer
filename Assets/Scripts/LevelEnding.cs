using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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
                StartCoroutine(ChangeLevel());
            }
        }

        private IEnumerator ChangeLevel()
        {
            UIManager.TransitionSceneFadeOut?.Invoke();
            SaveSystem.LevelFinished?.Invoke(SceneManager.GetActiveScene());
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
