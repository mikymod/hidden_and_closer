using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    public class SceneTransition : MonoBehaviour
    {
        public static UnityAction TransitionFadeOut;

        private Animator anim;

        private void Awake() => anim = GetComponent<Animator>();
        private void OnEnable() => TransitionFadeOut += OnTransitionFadeOut;
        private void OnDisable() => TransitionFadeOut -= OnTransitionFadeOut;

        private void OnTransitionFadeOut() => anim.SetTrigger("FadeOut");
    }
}
