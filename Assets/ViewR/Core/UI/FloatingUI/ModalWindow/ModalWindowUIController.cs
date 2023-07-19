using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;
using ViewR.Core.UI.Visuals.Reward;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    /// <summary>
    /// Singleton class to allow simple reference to the UI components.
    /// </summary>
    public class ModalWindowUIController : SingletonExtended<ModalWindowUIController>
    {
        [Header("References")]
        [SerializeField] private ModalWindowPanel modalWindow;
        public ModalWindowPanel ModalWindowPanel => modalWindow;

        [FormerlySerializedAs("modalWindowFireParticleBurst")]
        [SerializeField]
        private FireParticleBurst fireParticleBurst;
        public FireParticleBurst FireParticleBurst => fireParticleBurst;
        
        [Header("Config Behaviour")]
        [SerializeField] private AudioSource modalWindowAudioSource;
        public AudioSource ModalWindowAudioSource => modalWindowAudioSource;
        
        [SerializeField] private AudioClip errorSound;
        public AudioClip ErrorSound => errorSound;
        
    }
}