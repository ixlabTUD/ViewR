using UnityEngine;

namespace ViewR.StatusManagement.Setters
{
    /// <summary>
    /// Sets the values of the <see cref="ClientPassthroughLevel"/>
    /// </summary>
    public class ClientPassthroughLevelSetter : MonoBehaviour
    {
        #region Set Value // Only applies value, if not already on this level. 

        /// <summary>
        /// Set the <see cref="ClientPassthroughLevel"/> - given it is different from the current value.
        /// </summary>
        /// <param name="newPassthroughLevel">The new level.</param>
        public void SetClientPassthrough(PassthroughLevel newPassthroughLevel)
        {
            ClientPassthroughLevel.SetStatus(newPassthroughLevel, false);
        }
        
        /// <summary>
        /// Set the <see cref="ClientPassthroughLevel"/> via float or int.
        /// </summary>
        /// <param name="newPassthroughLevel">The new level.</param>
        public void SetClientPassthrough(int newPassthroughLevel)
        {
            var newLevel = (PassthroughLevel)newPassthroughLevel;
            
            SetClientPassthrough(newLevel);
        } 
        
        /// <summary>
        /// Set the <see cref="ClientPassthroughLevel"/> via float or int.
        /// </summary>
        /// <remarks>
        /// (internally casts to int.)
        /// Required, as slider always spits out floats, even if capped to whole numbers.
        /// </remarks>
        /// <param name="newPassthroughLevel">The new level.</param>
        public void SetClientPassthrough(float newPassthroughLevel)
        {
            var newLevel = (int)newPassthroughLevel;
            
            SetClientPassthrough(newLevel);
        }

        #endregion

        #region Force Set Value

        /// <summary>
        /// Set the <see cref="ClientPassthroughLevel"/> - given it is different from the current value.
        /// </summary>
        /// <param name="newPassthroughLevel">The new level.</param>
        public void ForceSetClientPassthrough(PassthroughLevel newPassthroughLevel)
        {
            ClientPassthroughLevel.SetStatus(newPassthroughLevel, true);
        }
        
        /// <summary>
        /// Set the <see cref="ClientPassthroughLevel"/> via float or int.
        /// </summary>
        /// <param name="newPassthroughLevel">The new level.</param>
        public void ForceSetClientPassthrough(int newPassthroughLevel)
        {
            var newLevel = (PassthroughLevel)newPassthroughLevel;
            
            ForceSetClientPassthrough(newLevel);
        } 
        
        /// <summary>
        /// Set the <see cref="ClientPassthroughLevel"/> via float or int.
        /// </summary>
        /// <remarks>
        /// (internally casts to int.)
        /// Required, as slider always spits out floats, even if capped to whole numbers.
        /// </remarks>
        /// <param name="newPassthroughLevel">The new level.</param>
        public void ForceSetClientPassthrough(float newPassthroughLevel)
        {
            var newLevel = (int)newPassthroughLevel;
            
            ForceSetClientPassthrough(newLevel);
        } 

        #endregion
    }
}