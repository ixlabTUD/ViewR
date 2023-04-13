using System;
using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.IntroductionSequencing.FineTunedAlignment
{
    /// <summary>
    /// A simple class to stop tweakers when needed! From anywhere :)
    /// </summary>
    public class AlignmentTuningDisabler : MonoBehaviour
    {
        public static event Action TranslationalAlignmentManagersShouldStop;
        public static event Action RotationalAlignmentManagersShouldStop;
        
        public void DisableAllTranslationalTuners()
        {
            InvokeAllTranslationalManagersShouldStop();
        }
        
        public void DisableAllRotationalTuners()
        {
            InvokeAllRotationalManagersShouldStop();
        }
        
        public static void InvokeAllTranslationalManagersShouldStop()
        {
            TranslationalAlignmentManagersShouldStop?.Invoke();
        }
        
        public static void InvokeAllRotationalManagersShouldStop()
        {
            RotationalAlignmentManagersShouldStop?.Invoke();
        }
    }
}