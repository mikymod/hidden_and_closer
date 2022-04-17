using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HNC.Audio
{
    public class AudioMainMenuUIController : MonoBehaviour
    {
        [SerializeField] private AudioClipsBankSO generalUIClipsBank;
        [SerializeField] private AudioConfigurationSO genericUIConfiguration;


        public void PlayGeneralUISound()
        {
            AudioEventsManager.OnSoundPlay?.Invoke(generalUIClipsBank, genericUIConfiguration, null, 0f);
        }
    }
}
