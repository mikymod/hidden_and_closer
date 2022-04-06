using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Debug.Log("INTERACT");
        SaveSystem.PlayerSave?.Invoke(SceneManager.GetActiveScene(), transform, true);
    }
}
