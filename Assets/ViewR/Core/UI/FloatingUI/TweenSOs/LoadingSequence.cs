using UnityEngine;

namespace ViewR.Core.UI.FloatingUI.TweenSOs
{
    [System.Serializable]
    public class Tip
    {
        public string tipTitle;
        public string tip;
    }
    
    [CreateAssetMenu(fileName = "Loading Sequence", menuName = "Loading/Loading Sequence", order = 0)]
    public class LoadingSequence : ScriptableObject
    {
        [Header("Backdrop")]
        public Sprite[] backgrounds;
        
        [Header("Tips")]
        public Tip[] tips;
        
        [Header("Debugging")]
        [SerializeField] protected bool debugging;

        // private void OnValidate()
        // {
        //     // if (tips.Length == tipsHeaders.Length)
        //     //     return;
        // }


    }
}