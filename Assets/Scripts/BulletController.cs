using System.Collections;
using HNC.Audio;
using UnityEngine;

namespace HNC
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody _rb;
        private bool _hasCollide = false;

        [SerializeField] private GameObject bloodParticlesPrefab;
        [SerializeField] private GameObject dustParticlesPrefab;
        private GameObject _bloodGO;
        private ParticleSystem _bloodParticles;
        private GameObject _dustGO;
        private ParticleSystem _dustParticles;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _bloodGO = Instantiate(bloodParticlesPrefab, Vector3.zero, Quaternion.identity);
            _bloodParticles = _bloodGO.GetComponent<ParticleSystem>();
            _bloodParticles.Stop();
            _dustGO = Instantiate(dustParticlesPrefab, Vector3.zero, Quaternion.identity);
            _dustParticles = _dustGO.GetComponent<ParticleSystem>();
            _dustParticles.Stop();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_hasCollide)
            {
                return;
            }

            if (other.gameObject.TryGetComponent(out NewEnemyController enemy))
            {
                enemy.Damaged();

                _bloodGO.transform.position = other.GetContact(0).point;
                _bloodGO.transform.rotation = Quaternion.LookRotation(other.GetContact(0).normal, Vector3.up);
                _bloodParticles.Play();
            }
            else
            {
                _dustGO.transform.position = other.GetContact(0).point;
                _dustGO.transform.rotation = Quaternion.LookRotation(other.GetContact(0).normal, Vector3.up);
                _dustParticles.Play();
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

            if (_dustParticles.isPlaying)
            {
                _dustParticles.Stop();
            }

            if (_bloodParticles.isPlaying)
            {
                _bloodParticles.Stop();
            }

            gameObject.SetActive(false);
        }
    }
}
