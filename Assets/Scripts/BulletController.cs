using System.Collections;
using HNC.Audio;
using UnityEngine;

namespace HNC
{
    // FIXME: temp behaviour
    public class BulletController : MonoBehaviour
    {
        private Rigidbody _rb;
        private bool _hasCollide = false;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void OnCollisionEnter(Collision other)
        {
            if (!_hasCollide && other.gameObject.TryGetComponent(out EnemyController enemy))
            {
                enemy.Damaged();
            }
            _hasCollide = true;
            StartCoroutine(Reset());
            transform.GetComponent<AudioRockController>().PlayHitSound();
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(1);

            _hasCollide = false;
            _rb.useGravity = false;
            _rb.isKinematic = true;
            gameObject.SetActive(false);
        }
    }
}
