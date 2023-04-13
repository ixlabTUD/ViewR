using UnityEngine;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    /// <summary>
    /// A simple way to fire an event when a Tab Button was pressed.
    /// This allows us to easily change the sprites on the active tab button
    /// </summary>
    public class TabButtonManager : MonoBehaviour
    {
        [Header("Visuals")]
        public Sprite buttonSelectedSprite;

        public delegate void AButtonWasPressed(TabButton activeTabButton);

        public event AButtonWasPressed ATabButtonWasPressed;

        public void OnATabButtonWasPressed(TabButton activeTabButton)
        {
            ATabButtonWasPressed?.Invoke(activeTabButton);
        }
    }
}