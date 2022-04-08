using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour, Interactable
{
    private bool _interacted = false;

    public void Interact()
    {
        if (_interacted)
        {
            return;
        }

        _interacted = true;

        StartCoroutine(Interaction());
    }

    private IEnumerator Interaction()
    {
        SaveSystem.PlayerSave?.Invoke(SceneManager.GetActiveScene(), transform, true);

        yield return new WaitForSeconds(2);

        _interacted = false;
    }
}
