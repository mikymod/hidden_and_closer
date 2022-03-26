using System.Collections;
using UnityEngine;

// FIXME: temp behaviour
public class BulletController : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("On Collision enter");
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(1);

        _rb.useGravity = false;
        _rb.isKinematic = true;
        gameObject.SetActive(false);
    }
}
