using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.Experiences.ExperienceSync.DynamicPopulationOfSelectors
{
    public class ExperiencePopulateDropdown : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private ExperienceChooser experienceChooser;
        [SerializeField]
        private TMP_Dropdown dropdownToModify;
        [Header("Settings")]
        [SerializeField]
        private bool runOnEnable;
        [SerializeField]
        private bool setPreferredWidth;
        [SerializeField]
        private float dropdownPreferredWidthForSelection = 500f;
        
        [Header("Debugging")]
        [SerializeField]
        private bool debugging;

        private CancellationTokenSource _tokenSource;
        private List<string> _options;
        private float? _dropdownPreviousPreferredWidth;
        private LayoutElement _dropdownLayoutElement;
        private bool _initialized;
        
        internal void Awake()
        {
            _tokenSource = new CancellationTokenSource();
        }

        private void OnEnable()
        {
            if (runOnEnable)
            {
                ConfigureAndShowDropdown();
            }
        }

        public void ConfigureAndShowDropdown()
        {
            // Do the magic
            var names = new List<string>();
            for (var i = 0; i < experienceChooser.experiences.Length; i++)
            {
                var experience = experienceChooser.experiences[i];
                names.Add(experience.experienceTitle);
            }

            _options = names;
            dropdownToModify.ClearOptions();
            dropdownToModify.AddOptions(_options);
            
            // Wait and then display the dropdown
            StartCoroutine(DisplayDropdown());
        }
        
        public void ProposeNewSetting()
        {
            experienceChooser.ProposeNewModel(dropdownToModify.value);
        }

        private IEnumerator DisplayDropdown()
        {
            // Wait for dropdowns to populate
            while (_options == null)
                yield return null;
            
            // Update layout group
            if (setPreferredWidth)
            {
                _dropdownLayoutElement = dropdownToModify.GetComponent<LayoutElement>();
                _dropdownPreviousPreferredWidth = _dropdownLayoutElement.preferredWidth; 
                _dropdownLayoutElement.preferredWidth = dropdownPreferredWidthForSelection;
            }

            // Wait a frame
            yield return new WaitForEndOfFrame();
            
            // Show dropdown.
            dropdownToModify.gameObject.SetActive(true);
            dropdownToModify.transform.parent.gameObject.SetActive(true);

            if (!_initialized)
            {
                // Subscribe:
                experienceChooser.NewSpaceExperienceWasConfigured += SpaceExperienceChooserOnNewSpaceExperienceWasConfigured;
                
                _initialized = true;
            }
        }

        private void SpaceExperienceChooserOnNewSpaceExperienceWasConfigured(int newID)
        {
            if (gameObject.activeSelf && gameObject.activeInHierarchy && this.enabled)
            {
                // Applies the new value to the dropdown.
                dropdownToModify.value = newID;
            }
        }

        private void OnDisable()
        {
            // Cancel async task if we are quitting
            _tokenSource?.Cancel();

            // Restore layout, if changed
            if (setPreferredWidth)
            {
                if(_dropdownLayoutElement && _dropdownPreviousPreferredWidth != null)
                    _dropdownLayoutElement.preferredWidth = (float) _dropdownPreviousPreferredWidth; 
            }

            if (runOnEnable)
            {
                // Unsubscribe:
                experienceChooser.NewSpaceExperienceWasConfigured -= SpaceExperienceChooserOnNewSpaceExperienceWasConfigured;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe:
            experienceChooser.NewSpaceExperienceWasConfigured -= SpaceExperienceChooserOnNewSpaceExperienceWasConfigured;
        }
    }
}