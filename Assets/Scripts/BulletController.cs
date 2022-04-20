using System.Collections;
using HNC.Audio;
using UnityEngine;

namespace HNC
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody _rb;
        private bool _hasCollide = false;

        [SerializeField] private ParticleSystem bloodParticles;
        [SerializeField] private ParticleSystem dustParticles;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_hasCollide)
            {
                return;
            }

            if (other.gameObject.TryGetComponent(out EnemyController enemy))
            {
                enemy.Damaged();

                bloodParticles.transform.parent = null;
                bloodParticles.transform.position = other.GetContact(0).point;
                bloodParticles.transform.rotation = Quaternion.LookRotation(other.GetContact(0).normal, Vector3.up);
                bloodParticles.Play();
            }
            else
            {
                dustParticles.transform.parent = null;
                dustParticles.transform.position = other.GetContact(0).point;
                dustParticles.transform.rotation = Quaternion.LookRotation(other.GetContact(0).normal, Vector3.up);
                dustParticles.Play();
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

            if (dustParticles.isPlaying)
            {
                dustParticles.Stop();
                dustParticles.transform.parent = transform;
            }

            if (bloodParticles.isPlaying)
            {
                bloodParticles.Stop();
                bloodParticles.transform.parent = transform;
            }

            gameObject.SetActive(false);
        }
    }
}
