using System;
using System.Collections;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using SurgeExtensions;
using SurgeExtensions.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ViewR.Core.UI.FloatingUI.Follower;
using ViewR.Core.UI.FloatingUI.ModalWindow.ProgressBar;
using ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;
using ViewR.HelpersLib.SurgeExtensions.Animators.UI;

namespace ViewR.Core.UI.FloatingUI.ModalWindow
{
    /// <summary>
    /// Class to control the appearance of the "Modal Window".
    /// </summary>
    public class ModalWindowPanel : MonoBehaviour
    {
        [Header("Header")] 
        [SerializeField] private Transform headerTransform;
        [SerializeField] private TMP_Text titleTextField;
        [SerializeField] private GameObject closeButton;
        [SerializeField] private GameObject pinButton;

        [Header("Body")]
        [SerializeField] private Transform bodyTransform;
        [Space]
        [SerializeField] private Transform horizontalLayoutTransform;
        [SerializeField] private LayoutElement horizontalImageContainerLayoutElement;
        [SerializeField] private Image horizontalImage;
        [SerializeField] private TMP_Text horizontalText;
        [Space]
        [SerializeField] private Transform verticalLayoutTransform;
        [SerializeField] private LayoutElement verticalImageContainerLayoutElement;
        [SerializeField] private Image verticalImage;
        [SerializeField] private TMP_Text verticalText;
        [Space]
        [Help("These are disabled whenever the window is re-configured. They should be enabled by the user.")]
        public Transform fillHorizontalTransform;
        public LayoutElement fillImageContainerLayoutElement;
        public Image fillImage;
        public TMP_Text fillText;
        public UIFillAnimation uiFillAnimation;

        [Header("Selections")]
        [Help("Experimental. To see these, also enable their parents.")]
        public TMP_Dropdown dropdown1;
        public TMP_Dropdown dropdown2;
        public TMP_Dropdown dropdown3;
        public TMP_InputField inputField1;
        public TMP_InputField inputField2;
        public TMP_InputField inputField3;
        public Slider slider1;
        public Slider slider2;
        public Slider slider3;
        public Toggle toggle1;
        public Toggle toggle2;
        public Toggle toggle3;

        [Header("Progress bar")]
        public ProgressbarManager progressBarManager;
        
        [Header("Footer")] 
        [SerializeField] private Transform footerTransform;
        public Button confirmButton;
        public Button declineButton;
        public Button alternateButton1;
        public Button alternateButton2;
        public Button alternateButton3;

        [Header("Tweening")]
        [SerializeField] private GameObject parentToDisable;
        [SerializeField] private CanvasGroup canvasGroup;
        public RectTransform modalWindowBoxTransform;
        [FormerlySerializedAs("tweenConfigFade")]
        [SerializeField] private TweenConfigFade tweenConfig;
        [SerializeField, Tooltip("The shake strength if selecting an invalid target")] 
        private float shakeStrength = 25;

        [SerializeField, 
         Tooltip("The fraction of the duration it takes until the window is re-opened if Close and Open " +
                 "are called immediately after each other.")]
        private float fractionOfDurationWhenReopening = 2f;
        
        [Header("Button Color Change")]
        [SerializeField]
        private TweenConfig buttonColorTweenConfig;

        
        [Header("References")]
        public UpdateContentFitterLayout modalWindowUpdateContentFitterLayout;
        [SerializeField] private ToggleFollower toggleFollower;
        public Transform confettiSpawnPosition;

        [Header("Events")]
        public UnityEvent newWindowWasSetUp;

        private ButtonReference _confirmButtonReference;
        private ButtonReference _declineButtonReference;
        private ButtonReference _alternateButton1Reference;
        private ButtonReference _alternateButton2Reference;
        private ButtonReference _alternateButton3Reference;
        private Vector3 _scalingRectInitialScale;
        private UIColors _defaultUIColors;
        private TweenBase _tweenBaseFade;
        private TweenBase _tweenBaseScale;
        private TweenBase _tweenBaseShake;
        private Image _headerBackGroundImage;
        private Image _bodyBackGroundImage;
        private Image _footerBackGroundImage;
        private HorizontalOrVerticalLayoutGroup _horizontalLayoutGroup;
        private HorizontalOrVerticalLayoutGroup _verticalLayoutGroup;
        [HideInInspector] public HorizontalOrVerticalLayoutGroup fillLayoutGroup;
        private ModalWindowConfig _currentModalWindowConfig;

        [HideInInspector, Tooltip("Whether the window is currently fading out or not.")]
        public bool previousFadingOutInProgress;

        private Coroutine _currentCoroutine;
        private bool _initialized;
        private bool _subscribedToTargetFollower;
        private TweenBase _buttonFadeTweenBase;


        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the object
        /// </summary>
        /// <param name="ignoreInitializedFlag">If true, we will run this regardless of the <see cref="_initialized"/> state.</param>
        private void Initialize(bool ignoreInitializedFlag = false)
        {
            if (_initialized && !ignoreInitializedFlag)
                return;

            // Get Refs
            _confirmButtonReference = new ButtonReference(confirmButton);
            _declineButtonReference = new ButtonReference(declineButton);
            _alternateButton1Reference = new ButtonReference(alternateButton1);
            _alternateButton2Reference = new ButtonReference(alternateButton2);
            _alternateButton3Reference = new ButtonReference(alternateButton3);
            
            _headerBackGroundImage = headerTransform.GetComponent<Image>();
            _bodyBackGroundImage = bodyTransform.GetComponent<Image>();
            _footerBackGroundImage = footerTransform.GetComponent<Image>();
            
            _horizontalLayoutGroup = horizontalLayoutTransform.GetComponent<HorizontalOrVerticalLayoutGroup>();
            _verticalLayoutGroup = verticalLayoutTransform.GetComponent<HorizontalOrVerticalLayoutGroup>();
            fillLayoutGroup = fillHorizontalTransform.GetComponent<HorizontalOrVerticalLayoutGroup>();

            // Store defaults
            _defaultUIColors = new UIColors
            {
                headerColor = _headerBackGroundImage.color,
                bodyColor = _bodyBackGroundImage.color,
                footerColor = _footerBackGroundImage.color
            };
            _scalingRectInitialScale = modalWindowBoxTransform.localScale;
            
            
            // Deactivate selections
            dropdown1.gameObject.SetActive(false);
            dropdown2.gameObject.SetActive(false);
            dropdown3.gameObject.SetActive(false);
            inputField1.gameObject.SetActive(false);
            inputField2.gameObject.SetActive(false);
            inputField3.gameObject.SetActive(false);
            slider1.gameObject.SetActive(false);
            slider2.gameObject.SetActive(false);
            slider3.gameObject.SetActive(false);
            toggle1.gameObject.SetActive(false);
            toggle2.gameObject.SetActive(false);
            toggle3.gameObject.SetActive(false);
            dropdown1.transform.parent.gameObject.SetActive(false);
            inputField1.transform.parent.gameObject.SetActive(false);
            slider1.transform.parent.gameObject.SetActive(false);
            toggle1.transform.parent.gameObject.SetActive(false);
            
            // Deactivate ProgressBarManager
            progressBarManager.gameObject.SetActive(false);

            // Set init values
            modalWindowBoxTransform.localScale = Vector3.zero;
            canvasGroup.alpha = 0;
            modalWindowBoxTransform.gameObject.SetActive(false);
            
            // Update flag
            _initialized = true;
        }

        #region Callbacks used by buttons

        public void Confirm()
        {
            if(_currentModalWindowConfig == null ||
               _currentModalWindowConfig.confirmButtonConfig.closeWindowIfButtonClicked)
                Close();
            _confirmButtonReference.onClickCallback?.Invoke();
        }

        public void Decline()
        {
            if(_currentModalWindowConfig == null ||
               _currentModalWindowConfig.declineButtonConfig.closeWindowIfButtonClicked)
                Close();
            _declineButtonReference.onClickCallback?.Invoke();
        }

        public void Alternate1()
        {
            if(_currentModalWindowConfig == null ||
               _currentModalWindowConfig.alternate1ButtonConfig.closeWindowIfButtonClicked)
                Close();
            _alternateButton1Reference.onClickCallback?.Invoke();
        }

        public void Alternate2()
        {
            if(_currentModalWindowConfig == null ||
               _currentModalWindowConfig.alternate2ButtonConfig.closeWindowIfButtonClicked)
                Close();
            _alternateButton2Reference.onClickCallback?.Invoke();
        }

        public void Alternate3()
        {
            if(_currentModalWindowConfig == null ||
               _currentModalWindowConfig.alternate3ButtonConfig.closeWindowIfButtonClicked) 
                Close();
            _alternateButton3Reference.onClickCallback?.Invoke();
        }

        #endregion

        #region Button color tweening

        #region Public Methods

        public void TweenConfirmButtonColor(Color newColor, TweenConfig colorTweenConfig = null) => TweenButtonColor(newColor, _confirmButtonReference);

        public void TweenConfirmButtonColorToInitialColor() => RestoreInitialButtonColor(_currentModalWindowConfig.confirmButtonConfig, _confirmButtonReference);

        public void TweenDeclineButtonColor(Color newColor, TweenConfig colorTweenConfig = null) => TweenButtonColor(newColor, _declineButtonReference);

        public void TweenDeclineButtonColorToInitialColor() => RestoreInitialButtonColor(_currentModalWindowConfig.declineButtonConfig, _declineButtonReference);

        public void TweenAlternateButton1Color(Color newColor, TweenConfig colorTweenConfig = null) => TweenButtonColor(newColor, _alternateButton1Reference);

        public void TweenAlternateButton1ColorToInitialColor() => RestoreInitialButtonColor(_currentModalWindowConfig.alternate1ButtonConfig, _alternateButton1Reference);

        public void TweenAlternateButton2Color(Color newColor, TweenConfig colorTweenConfig = null) => TweenButtonColor(newColor, _alternateButton2Reference);

        public void TweenAlternateButton2ColorToInitialColor() => RestoreInitialButtonColor(_currentModalWindowConfig.alternate2ButtonConfig, _alternateButton2Reference);

        public void TweenAlternateButton3Color(Color newColor, TweenConfig colorTweenConfig = null) => TweenButtonColor(newColor, _alternateButton3Reference);

        public void TweenAlternateButton3ColorToInitialColor() => RestoreInitialButtonColor(_currentModalWindowConfig.alternate3ButtonConfig, _alternateButton3Reference);

        #endregion


        private void TweenButtonColor(Color targetColor, ButtonReference buttonReference, TweenConfig colorTweenConfig = null)
        {
            colorTweenConfig = colorTweenConfig != null ? colorTweenConfig : buttonColorTweenConfig;

            _buttonFadeTweenBase = Tween.Color(buttonReference.imageRef,
                targetColor,
                colorTweenConfig.Duration,
                colorTweenConfig.Delay,
                colorTweenConfig.AnimationCurve,
                colorTweenConfig.loopType,
                obeyTimescale: colorTweenConfig.obeyTimescale);
        }

        private void RestoreInitialButtonColor(ButtonConfig buttonConfig, ButtonReference buttonReference, TweenConfig colorTweenConfig = null)
        {
            var targetColor = buttonConfig.useCustomColor
                ? buttonConfig.customButtonColor
                : buttonReference.defaultColor;

            colorTweenConfig = colorTweenConfig != null ? colorTweenConfig : buttonColorTweenConfig;

            TweenButtonColor(targetColor, buttonReference, colorTweenConfig);
        }

        #endregion

        #region Private Methods to handle the Windows appearance and functions

        /// <summary>
        /// Configures the window to the horizontal layout.
        /// </summary>
        /// <param name="modalWindowConfig"></param>
        private void SetupWindow(ModalWindowConfig modalWindowConfig)
        {
            if(_defaultUIColors == null)
                Initialize();
            
            // Hide experimental selections. Re-enable them via script after calling this method.
            dropdown1.transform.parent.gameObject.SetActive(false);
            inputField1.transform.parent.gameObject.SetActive(false);
            slider1.transform.parent.gameObject.SetActive(false);
            toggle1.transform.parent.gameObject.SetActive(false);
            
            // Deactivate ProgressBarManager. Re-enable it via script after calling this method.
            progressBarManager.gameObject.SetActive(false);
            
            // Show / Hide the header if none was given
            headerTransform.gameObject.SetActive(!string.IsNullOrEmpty(modalWindowConfig.title));
            titleTextField.text = modalWindowConfig.title;

            // For the body, it depends whether we want the vertical or horizontal layout - so its split up here.
            switch (modalWindowConfig.modalWindowBodyLayout)
            {
                case ModalWindowBodyLayout.Horizontal:
                    // Hide 
                    verticalLayoutTransform.gameObject.SetActive(false);
                    horizontalLayoutTransform.gameObject.SetActive(true);

                    // Show / Hide the Image if none was given
                    horizontalImage.sprite = modalWindowConfig.image;
                    horizontalImageContainerLayoutElement.gameObject.SetActive(modalWindowConfig.image != null);

                    // Show / Hide the text if none was given
                    horizontalText.text = modalWindowConfig.message;
                    horizontalText.gameObject.SetActive(!string.IsNullOrEmpty(modalWindowConfig.message));
                    
                    // Set order of Image and Text:
                    _horizontalLayoutGroup.reverseArrangement = modalWindowConfig.reverseImageAndTextOrder;
                    
                    break;
                case ModalWindowBodyLayout.Vertical:
                    // Hide 
                    horizontalLayoutTransform.gameObject.SetActive(false);
                    verticalLayoutTransform.gameObject.SetActive(true);

                    // Show / Hide the Image if none was given
                    verticalImage.sprite = modalWindowConfig.image;
                    verticalImageContainerLayoutElement.gameObject.SetActive(modalWindowConfig.image != null);

                    // Show / Hide the text if none was given
                    verticalText.text = modalWindowConfig.message;
                    verticalText.gameObject.SetActive(!string.IsNullOrEmpty(modalWindowConfig.message));
                    
                    // Set order of Image and Text:
                    _verticalLayoutGroup.reverseArrangement = modalWindowConfig.reverseImageAndTextOrder;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Set up buttons
            bool anyButtonActive;
            anyButtonActive = SetupButton(modalWindowConfig.confirmButtonConfig, _confirmButtonReference);
            anyButtonActive = SetupButton(modalWindowConfig.declineButtonConfig, _declineButtonReference) || anyButtonActive;
            anyButtonActive = SetupButton(modalWindowConfig.alternate1ButtonConfig, _alternateButton1Reference) || anyButtonActive;
            anyButtonActive = SetupButton(modalWindowConfig.alternate2ButtonConfig, _alternateButton2Reference) || anyButtonActive;
            anyButtonActive = SetupButton(modalWindowConfig.alternate3ButtonConfig, _alternateButton3Reference) || anyButtonActive;

            // Show / Hide close button
            closeButton.SetActive(!anyButtonActive);
            pinButton.SetActive(modalWindowConfig.showPinButton);
            
            // Handle toggle follower if any given.
            if (toggleFollower != null)
            {
                // Force "unpin" if we don't show the pin button
                if (!modalWindowConfig.showPinButton)
                    toggleFollower.EnableFollowing(true);
                
                SetToFollowMode(modalWindowConfig.forceFollowState);
            }

            // Show / Hide the footer if no buttons were given
            footerTransform.gameObject.SetActive(anyButtonActive);

            // Set up colors
            _headerBackGroundImage.color = modalWindowConfig.uiColors.useCustomColors
                ? modalWindowConfig.uiColors.headerColor
                : _defaultUIColors.headerColor;
            _bodyBackGroundImage.color = modalWindowConfig.uiColors.useCustomColors
                ? modalWindowConfig.uiColors.bodyColor
                : _defaultUIColors.bodyColor;
            _footerBackGroundImage.color = modalWindowConfig.uiColors.useCustomColors
                ? modalWindowConfig.uiColors.footerColor
                : _defaultUIColors.footerColor;
            
            // Hide the Fill-info
            fillHorizontalTransform.gameObject.SetActive(false);
            fillImage.gameObject.SetActive(false);
            fillImage.gameObject.SetActive(false);

            _currentModalWindowConfig = modalWindowConfig;

            // To ensure the canvas size and proportions get re-fitted
            // Canvas.ForceUpdateCanvases(); // fairly cpu hungry
            modalWindowUpdateContentFitterLayout.RecalculateLayouts();
            
            newWindowWasSetUp?.Invoke();
        }

        #region React To TargetFollower.ArrivedAtTargetForFirstTime
        // These methods belong to ModalWindowConfig.UIFollowMode.PinUponReachingTarget

        private void TrySubscribeToTargetFollower()
        {
            // Bail if already subscribed
            if(_subscribedToTargetFollower)
                return;
            
            toggleFollower.TargetFollower.ArrivedAtTargetForFirstTime += StopFollowingOnceReachingTarget;
            
            _subscribedToTargetFollower = true;
        }

        private void TryUnsubscribeFromTargetFollower()
        {
            // Bail if not subscribed
            if(!_subscribedToTargetFollower)
                return;
            
            toggleFollower.TargetFollower.ArrivedAtTargetForFirstTime -= StopFollowingOnceReachingTarget;
            
            _subscribedToTargetFollower = false;
        }

        private void StopFollowingOnceReachingTarget()
        {
            toggleFollower.EnableFollowing(false);
        }

        #endregion

        /// <summary>
        /// Sets up the button (<see cref="buttonReference"/>) according to the given <see cref="buttonConfig"/>.
        /// If <see cref="buttonConfig"/> is null, it will hide the button.
        /// </summary>
        /// <param name="buttonConfig"></param>
        /// <param name="buttonReference"></param>
        /// <returns>whether the bool was activated or not.</returns>
        private bool SetupButton(ButtonConfig buttonConfig, ButtonReference buttonReference)
        {
            if (buttonConfig != null 
                && buttonConfig.clickAction != null 
                // && !buttonConfig.buttonText.Equals(string.Empty)
                )
            {
                // Show the button
                buttonReference.button.gameObject.SetActive(true);

                // Set text and text color
                buttonReference.textField.text = !string.IsNullOrEmpty(buttonConfig.buttonText)
                    ? buttonConfig.buttonText
                    : buttonReference.defaultText;
                buttonReference.textField.color = buttonConfig.useCustomFontColor
                    ? buttonConfig.customFontColor
                    : buttonReference.defaultFontColor;

                // Set button image color and sprite
                // buttonReference.imageRef.color = buttonConfig.buttonColor ?? buttonReference.defaultColor;
                buttonReference.imageRef.color = buttonConfig.useCustomColor
                    ? buttonConfig.customButtonColor
                    : buttonReference.defaultColor;
                buttonReference.imageRef.sprite = buttonConfig.useCustomBackgroundSprite
                    ? buttonConfig.customButtonBackgroundSprite
                    : buttonReference.defaultBackgroundSprite;

                // Set action
                buttonReference.onClickCallback = buttonConfig.clickAction;

                return true;
            }
            else
            {
                // Hide the button
                buttonReference.button.gameObject.SetActive(false);
                buttonReference.onClickCallback = null;
                
                return false;
            }
        }

        #endregion

        #region Controll behaviour from outside

        public void SetToFollowMode(UIFollowMode uiFollowMode)
        {
            TryUnsubscribeFromTargetFollower();

            // Force Pin or Unpin if queried.
            switch (uiFollowMode)
            {
                case UIFollowMode.ForceFollowing:
                    toggleFollower.EnableFollowing(true);
                    break;
                case UIFollowMode.ForcePinned:
                    toggleFollower.EnableFollowing(false);
                    break;
                case UIFollowMode.ForcePinnedAndPinUponReachingTarget:
                    toggleFollower.EnableFollowing(false);
                    TrySubscribeToTargetFollower();
                    break;
                case UIFollowMode.Uncontrolled:
                    // Don't do anything.
                    break;
                case UIFollowMode.ForceFollowAndPinUponReachingTarget:
                    toggleFollower.EnableFollowing(true);
                    TrySubscribeToTargetFollower();
                    break;
                case UIFollowMode.UncontrolledAndPinUponReachingTarget:
                    TrySubscribeToTargetFollower();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Opening, Updating and Closing Methods

        public void Shake()
        {
            // Tween.AnchoredPosition(modalWindowBoxTransform, modalWindowBoxTransform.position.ToVector2(), .4f, 0,
            //     TweenAnimationExtension.GetTweenAnimation(TweenAnimationCurve.EaseWobble));

            _tweenBaseShake?.Stop();
            _tweenBaseShake = Tween.Value(
                modalWindowBoxTransform.localPosition, 
                Vector3.right * shakeStrength, 
                vector3 =>
                    {
                        // Debug.Log($"New Position: {vector3}".StartWithFrom(GetType()), this);
                        modalWindowBoxTransform.localPosition = vector3;
                    },
                .33f,
                0f,
                TweenAnimationExtension.GetTweenAnimation(TweenAnimationCurve.EaseWobble),
                Tween.LoopType.None,
                completeCallback: () => modalWindowBoxTransform.localPosition = Vector3.zero); 
            
            // Tween.Shake(modalWindowBoxTransform, modalWindowBoxTransform.position, modalWindowBoxTransform.right, .4f,
            //     0f, Tween.LoopType.None);
        }

        public void Close()
        {
            // uiFloatAndFadeIn.Appear(appear: false, true, callback: () => modalWindowBoxTransform.gameObject.SetActive(false));

            // Stop current tweens
            _tweenBaseFade?.Stop();
            _tweenBaseScale?.Stop();

            // Start tweens
            _tweenBaseFade = Tween.CanvasGroupAlpha(canvasGroup, 0, tweenConfig.Duration, tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);
            _tweenBaseScale = Tween.LocalScale(modalWindowBoxTransform, Vector3.zero, tweenConfig.Duration,
                tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType,
                completeCallback: () =>
                {
                    modalWindowBoxTransform.gameObject.SetActive(false); 
                    if(parentToDisable) parentToDisable.SetActive(false);
                });
        }

        /// <summary>
        /// Starts the coroutine <see cref="ShowWindowPotentiallyDelayed"/> to show the window.
        ///
        /// WARNING: Setup buttons before hand - see <see cref="InfoWindowCaller.ShowWindow()"/> for an example
        /// </summary>
        /// <param name="modalWindowConfig"></param>
        public void ShowWindow(ModalWindowConfig modalWindowConfig, Action callback = null)
        {
            if(parentToDisable) parentToDisable.SetActive(true);

            // Ensure we stop previous setup if any is currently running
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine =  StartCoroutine(ShowWindowPotentiallyDelayed(modalWindowConfig, callback));
        }

        /// <summary>
        /// Should only be used, if the state machine allows re-entry. 
        /// </summary>
        public void ReEnableTheCurrentWindow()
        {
            if(parentToDisable) parentToDisable.SetActive(true);

            // Show the window
            modalWindowBoxTransform.gameObject.SetActive(true);

            // Stop current tweens
            _tweenBaseFade?.Stop();
            _tweenBaseScale?.Stop();

            // Start new tweens
            _tweenBaseFade = Tween.CanvasGroupAlpha(canvasGroup, 1, tweenConfig.Duration, tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);
            _tweenBaseScale = Tween.LocalScale(modalWindowBoxTransform, Vector3.one, tweenConfig.Duration,
                tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);
        }

        /// <summary>
        /// Shows the window, but potentially delayed, if it is previously still closing.
        /// This allows us to get the visual of "a new window opens"
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowWindowPotentiallyDelayed(ModalWindowConfig modalWindowConfig, Action callback = null)
        {
            // Wait if there is something going on.
            if (_tweenBaseFade != null)
            {
                // Duration until stopping:
                var countdown = tweenConfig.Duration / fractionOfDurationWhenReopening;
                // Wait, if we are either running or delayed AND while the countdown is > 0
                while ((_tweenBaseFade.Status.Equals(Tween.TweenStatus.Running) ||
                        _tweenBaseFade.Status.Equals(Tween.TweenStatus.Delayed))
                       &&
                       countdown > 0)
                {
                    countdown -= Time.deltaTime;
                    previousFadingOutInProgress = true;
                    yield return null;
                }
                previousFadingOutInProgress = false;
            }
            
            if(parentToDisable) parentToDisable.SetActive(true);

            // Set up the window
            SetupWindow(modalWindowConfig);

            // Show the window
            modalWindowBoxTransform.gameObject.SetActive(true);

            // Stop current tweens
            _tweenBaseFade?.Stop();
            _tweenBaseScale?.Stop();

            // Start new tweens
            _tweenBaseFade = Tween.CanvasGroupAlpha(canvasGroup, 1, tweenConfig.Duration, tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);
            _tweenBaseScale = Tween.LocalScale(modalWindowBoxTransform, _scalingRectInitialScale, tweenConfig.Duration,
                tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);

            // uiFloatAndFadeIn.Appear(true);
            
            // Invoke callback
            callback?.Invoke();
        }

        /// <summary>
        /// Cheap way to update the text and image of the window
        ///
        /// A more mature method could be made up with the <see cref="ModalWindowConfig"/>. Yet, the buttons need to be updated too.
        /// This more performant solution will do for now.
        /// </summary>
        /// <param name="newText"></param>
        /// <param name="newImage"></param>
        public void UpdateWindow(string newText = null, Sprite newImage = null)
        {
            if(newText != null)
                horizontalText.text = verticalText.text = newText;

            if(newImage != null)
                horizontalImage.sprite = verticalImage.sprite = newImage;
        }

        #endregion
    }
}