using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace ViewR.HelpersLib.Utils.Version
{
    /// <summary>
    /// A class to write the Application version to a text field.
    /// </summary>
    public class VersionToTMP : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textField;

        protected void Awake()
        {
            if (textField == null)
                textField = GetComponent<TMP_Text>();
        }

        protected virtual void Start()
        {
            Assert.IsNotNull(textField);
            textField.text = $"Version: {Application.version}";
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (textField == null)
                textField = GetComponent<TMP_Text>();
        }
#endif
    }
}