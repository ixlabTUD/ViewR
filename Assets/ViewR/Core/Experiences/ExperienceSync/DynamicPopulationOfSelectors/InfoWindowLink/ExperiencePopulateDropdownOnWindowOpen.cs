using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.HelpersLib.Extensions.General;
using Debug = UnityEngine.Debug;

namespace ViewR.Core.Experiences.ExperienceSync.DynamicPopulationOfSelectors.InfoWindowLink
{
    public class ExperiencePopulateDropdownOnWindowOpen : MonoBehaviour
    {
        [SerializeField]
        private ExperienceChooser spaceExperienceChooser;
        [SerializeField]
        private ModalWindowPanel modalWindowPanel;
        
        [SerializeField]
        private bool runOnEnable;
        [SerializeField]
        private bool debugging;
        [SerializeField]
        private int dropdownIndex = 0;
        [SerializeField]
        private float dropdownPreferredWidthForSelection = 500f;
        
        private CancellationTokenSource _tokenSource;
        private TMP_Dropdown _dropdownToModify;
        private List<string> _options;
        private float? _dropdownPreviousPreferredWidth;
        private const float DropdownPreferredWidthForSelection = 500f;
        private LayoutElement _dropdownLayoutElement;
        


        internal void Awake()
        {
            _tokenSource = new CancellationTokenSource();

            // Get refs
            if (dropdownIndex == 0 || dropdownIndex < 0 || dropdownIndex >= 3)
                _dropdownToModify = modalWindowPanel.dropdown1;
            else if (dropdownIndex == 1)
                _dropdownToModify = modalWindowPanel.dropdown2;
            else if (dropdownIndex == 3)
                _dropdownToModify = modalWindowPanel.dropdown3;
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
            for (var i = 0; i < spaceExperienceChooser.experiences.Length; i++)
            {
                var experience = spaceExperienceChooser.experiences[i];
                names.Add(experience.experienceTitle);
            }

            _options = names;
            _dropdownToModify.ClearOptions();
            _dropdownToModify.AddOptions(_options);
            
            // Wait and then display the dropdown
            StartCoroutine(DisplayDropdown());
        }
        
        public void ProposeNewSetting()
        {
            spaceExperienceChooser.ProposeNewModel(_dropdownToModify.value);
        }

        private IEnumerator DisplayDropdown()
        {
            // Wait for dropdowns to populate
            while (_options == null)
                yield return null;
            
            // Wait for fading animation to re-start.
            while (modalWindowPanel.previousFadingOutInProgress)
                yield return null;
            
            // Update layout group
            _dropdownLayoutElement = _dropdownToModify.GetComponent<LayoutElement>();
            _dropdownPreviousPreferredWidth = _dropdownLayoutElement.preferredWidth; 
            _dropdownLayoutElement.preferredWidth = dropdownPreferredWidthForSelection;

            // Wait a frame
            yield return new WaitForEndOfFrame();
            
            // Show dropdown.
            _dropdownToModify.gameObject.SetActive(true);
            _dropdownToModify.transform.parent.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            // Cancel async task if we are quitting
            _tokenSource?.Cancel();
            
            // Restore layout, if changed
            if(_dropdownLayoutElement && _dropdownPreviousPreferredWidth != null)
                _dropdownLayoutElement.preferredWidth = (float) _dropdownPreviousPreferredWidth; 
        }

        /// <summary>
        /// Running command async to not freeze thread.
        /// </summary>
        private async void PopulateDropdownAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if(_options == null)
                _options = await Task.Run((() =>
                        {
                            var names = new List<string>();
                            for (var i = 0; i < spaceExperienceChooser.experiences.Length; i++)
                            {
                                var experience = spaceExperienceChooser.experiences[i];
                                names.Add(experience.experienceTitle);
                            }

                            // Bail if it was cancelled - should best be in a loop
                            if (_tokenSource.IsCancellationRequested)
                                return names;
                        
                            return names;
                        }
                    ), _tokenSource.Token);
            
            // Bail if it was cancelled
            if (_tokenSource.IsCancellationRequested)
                return;
            
            _dropdownToModify.ClearOptions();
            _dropdownToModify.AddOptions(_options);
            
            stopwatch.Stop();
            if(debugging)
                Debug.Log($"Operation took {stopwatch.ElapsedMilliseconds} ms; Result: {_options.Count}".StartWithFrom(GetType()), this);
        }
   
    }
}
