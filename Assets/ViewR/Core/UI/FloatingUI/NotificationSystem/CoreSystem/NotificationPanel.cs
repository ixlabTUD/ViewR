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
using ViewR.Core.UI.FloatingUI.ModalWindow;
using ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference;
using ViewR.HelpersLib.SurgeExtensions.Animators;

namespace ViewR.Core.UI.FloatingUI.NotificationSystem.CoreSystem
{
    /// <summary>
    /// A magic notification panel!
    /// Recommended: Either configure buttons OR an image.
    /// Recommended: Do not configure more than two buttons.
    /// </summary>
    public class NotificationPanel : MonoBehaviour
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
        
        [Header("Footer")] 
        [SerializeField] private Transform footerTransform;
        public Button confirmButton;
        public Button declineButton;
        public Button alternateButton1;
        public Button alternateButton2;
        public Button alternateButton3;
        
        [Header("Others")]
        [SerializeField]
        private GameObject notificationWindow;

        [Header("Tweening")]
        [FormerlySerializedAs("appearerController")]
        [SerializeField]
        private AppearerController additionalAppearerController;
        [SerializeField]
        private GameObject parentToDisable;
        [SerializeField]
        private CanvasGroup canvasGroup;
        public RectTransform modalWindowBoxTransform;
        [FormerlySerializedAs("tweenConfigFade")]
        [SerializeField]
        private TweenConfigFade tweenConfig;
        [SerializeField, Tooltip("The shake strength if selecting an invalid target")] 
        private float shakeStrength = 25;

        [SerializeField, 
         Tooltip("The fraction of the duration it takes until the window is re-opened if Close and Open " +
                 "are called immediately after each other.")]
        private float fractionOfDurationWhenReopening = 2f;

        
        [FormerlySerializedAs("modalWindowUpdateContentFitterLayout")]
        [Header("References")]
        public UpdateContentFitterLayout updateContentFitterLayout;
        [SerializeField] private ToggleFollower toggleFollower;
        public Transform confettiSpawnPosition;

        [Header("Events")]
        public UnityEvent newWindowWasSetUp;
        public UnityEvent<NotificationPanel> showingWindow;


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
        private NotificationPanelConfig _currentNotificationPanelConfig;

        [HideInInspector, Tooltip("Whether the window is currently fading out or not.")]
        public bool previousFadingOutInProgress;
        [HideInInspector, Tooltip("Set to true to pause the _closingRoutine.")]
        public bool pauseTimer;

        private Coroutine _showDelayedCoroutine;
        private Coroutine _closingRoutine;
        private bool _initialized;
        private bool _subscribedToTargetFollower;

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

            // Store defaults
            _defaultUIColors = new UIColors
            {
                headerColor = _headerBackGroundImage.color,
                bodyColor = _bodyBackGroundImage.color,
                footerColor = _footerBackGroundImage.color
            };
            _scalingRectInitialScale = modalWindowBoxTransform.localScale;

            // Set init values
            modalWindowBoxTransform.localScale = Vector3.zero;
            canvasGroup.alpha = 0;
            modalWindowBoxTransform.gameObject.SetActive(false);
            
            // Update flag
            _initialized = true;
        }

        private void OnDisable()
        {
            // Ensure dict is up to date
            if (NotificationPanelManager.IsInstanceRegistered)
                NotificationPanelManager.Instance.TryRemoveValue(this);
            
            if (_closingRoutine != null)
                StopCoroutine(_closingRoutine);
        }

        #region Callbacks used by buttons

        public void Confirm()
        {
            if(_currentNotificationPanelConfig == null ||
               _currentNotificationPanelConfig.confirmButtonConfig.closeWindowIfButtonClicked)
                Close();
            _confirmButtonReference.onClickCallback?.Invoke();
        }

        public void Decline()
        {
            if(_currentNotificationPanelConfig == null ||
               _currentNotificationPanelConfig.declineButtonConfig.closeWindowIfButtonClicked)
                Close();
            _declineButtonReference.onClickCallback?.Invoke();
        }

        public void Alternate1()
        {
            if(_currentNotificationPanelConfig == null ||
               _currentNotificationPanelConfig.alternate1ButtonConfig.closeWindowIfButtonClicked)
                Close();
            _alternateButton1Reference.onClickCallback?.Invoke();
        }

        public void Alternate2()
        {
            if(_currentNotificationPanelConfig == null ||
               _currentNotificationPanelConfig.alternate2ButtonConfig.closeWindowIfButtonClicked)
                Close();
            _alternateButton2Reference.onClickCallback?.Invoke();
        }

        public void Alternate3()
        {
            if(_currentNotificationPanelConfig == null ||
               _currentNotificationPanelConfig.alternate3ButtonConfig.closeWindowIfButtonClicked) 
                Close();
            _alternateButton3Reference.onClickCallback?.Invoke();
        }

        #endregion

        #region Private Methods to handle the Windows appearance and functions

        /// <summary>
        /// Configures the window to the horizontal layout.
        /// </summary>
        /// <param name="notificationPanelConfig"></param>
        private void SetupWindow(NotificationPanelConfig notificationPanelConfig)
        {
            if(_defaultUIColors == null)
                Initialize();
            
            // Show / Hide the header if none was given
            headerTransform.gameObject.SetActive(!string.IsNullOrEmpty(notificationPanelConfig.title));
            titleTextField.text = notificationPanelConfig.title;

            //! Body.
            // Show
            horizontalLayoutTransform.gameObject.SetActive(true);

            // Show / Hide the Image if none was given
            horizontalImage.sprite = notificationPanelConfig.image;
            horizontalImageContainerLayoutElement.gameObject.SetActive(notificationPanelConfig.image != null);

            // Show / Hide the text if none was given
            horizontalText.text = notificationPanelConfig.message;
            horizontalText.gameObject.SetActive(!string.IsNullOrEmpty(notificationPanelConfig.message));

            // Set order of Image and Text:
            _horizontalLayoutGroup.reverseArrangement = notificationPanelConfig.reverseImageAndTextOrder;


            //! Set up buttons
            bool anyButtonActive;
            anyButtonActive = SetupButton(notificationPanelConfig.confirmButtonConfig, _confirmButtonReference);
            anyButtonActive = SetupButton(notificationPanelConfig.declineButtonConfig, _declineButtonReference) || anyButtonActive;
            anyButtonActive = SetupButton(notificationPanelConfig.alternate1ButtonConfig, _alternateButton1Reference) || anyButtonActive;
            anyButtonActive = SetupButton(notificationPanelConfig.alternate2ButtonConfig, _alternateButton2Reference) || anyButtonActive;
            anyButtonActive = SetupButton(notificationPanelConfig.alternate3ButtonConfig, _alternateButton3Reference) || anyButtonActive;

            // Show / Hide close button
            closeButton.SetActive(!anyButtonActive || notificationPanelConfig.forceShowCloseButton);
            pinButton.SetActive(notificationPanelConfig.showPinButton);
            
            // Handle toggle follower if any given.
            if (toggleFollower != null)
            {
                // Force "unpin" if we don't show the pin button
                if (!notificationPanelConfig.showPinButton)
                    toggleFollower.EnableFollowing(true);

                TryUnsubscribeFromTargetFollower();

                // Force Pin or Unpin if queried.
                switch (notificationPanelConfig.forceFollowState)
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

            // Show / Hide the footer if no buttons were given
            footerTransform.gameObject.SetActive(anyButtonActive);

            // Set up colors
            _headerBackGroundImage.color = notificationPanelConfig.uiColors.useCustomColors
                ? notificationPanelConfig.uiColors.headerColor
                : _defaultUIColors.headerColor;
            _bodyBackGroundImage.color = notificationPanelConfig.uiColors.useCustomColors
                ? notificationPanelConfig.uiColors.bodyColor
                : _defaultUIColors.bodyColor;
            _footerBackGroundImage.color = notificationPanelConfig.uiColors.useCustomColors
                ? notificationPanelConfig.uiColors.footerColor
                : _defaultUIColors.footerColor;

            _currentNotificationPanelConfig = notificationPanelConfig;

            // To ensure the canvas size and proportions get re-fitted
            // Canvas.ForceUpdateCanvases(); // fairly cpu hungry
            updateContentFitterLayout.RecalculateLayouts();
            
            // Should it auto close?
            if (notificationPanelConfig.shouldAutoClose)
            {
                // Ensure pause timer is false
                pauseTimer = false;
                // Start AutoClose.
                _closingRoutine = StartCoroutine(CloseAfterSeconds(notificationPanelConfig.autoCloseTimeOut));
            }

            newWindowWasSetUp?.Invoke();
        }

        #region AutoClose

        /// <summary>
        /// Closes the panel after <see cref="autoCloseTimeOut"/> seconds.
        /// The countdown is paused while <see cref="pauseTimer"/> is true.
        /// </summary>
        private IEnumerator CloseAfterSeconds(float autoCloseTimeOut)
        {
            var timer = 0.0f;
            
            while (timer < autoCloseTimeOut)
            {
                // Don't increase the timer if pausing. Just return.
                if (pauseTimer)
                    yield return null;
                
                timer += Time.deltaTime;

                // wait.
                yield return null;
            }
            
            Close();
        }

        /// <summary>
        /// Pause the timer.
        /// i.e. when the Notification panel is being pinned.
        /// </summary>
        /// <param name="pause"></param>
        public void PauseTimer(bool pause)
        {
            pauseTimer = pause;
        }

        public void DoPauseTimer()
        {
            PauseTimer(true);
        }

        public void UnPauseTimer()
        {
            PauseTimer(false);
        }
        
        

        #endregion

        #region React To TargetFollower.ArrivedAtTargetForFirstTime
        // These methods belong to NotificationPanelConfig.UIFollowMode.PinUponReachingTarget

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
        /// <returns>Whether the bool was activated or not.</returns>
        private static bool SetupButton(ButtonConfig buttonConfig, ButtonReference buttonReference)
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

            if (additionalAppearerController)
            {
                additionalAppearerController.AppearOutInverse();
            }

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
                    if (parentToDisable) parentToDisable.SetActive(false);
                });
            
            // Try to unregister.
            if (NotificationPanelManager.IsInstanceRegistered)
                NotificationPanelManager.Instance.TryRemoveValue(this);
        }

        /// <summary>
        /// Starts the coroutine <see cref="ShowWindowPotentiallyDelayed"/> to show the window.
        ///
        /// WARNING: Setup buttons before hand - see <see cref="NotificationPanelCaller.ShowWindow(Action)"/> for an example
        /// </summary>
        public void ShowWindow(NotificationPanelConfig notificationPanelConfig, Action callback = null)
        {
            // Stop coroutine if any present.
            if (_closingRoutine != null)
                StopCoroutine(_closingRoutine);
            
            if(parentToDisable) parentToDisable.SetActive(true);

            // Ensure we stop previous setup if any is currently running
            if (_showDelayedCoroutine != null)
                StopCoroutine(_showDelayedCoroutine);

            _showDelayedCoroutine =  StartCoroutine(ShowWindowPotentiallyDelayed(notificationPanelConfig, callback));
        }

        /// <summary>
        /// Should only be used, if the state machine allows re-entry. 
        /// </summary>
        public void ReEnableTheCurrentWindow()
        {
            if(parentToDisable) parentToDisable.SetActive(true);
            
            
            if (additionalAppearerController)
            {
                additionalAppearerController.AppearIn();
            }

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
        private IEnumerator ShowWindowPotentiallyDelayed(NotificationPanelConfig notificationPanelConfig, Action callback = null)
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
            SetupWindow(notificationPanelConfig);

            // Show the window
            modalWindowBoxTransform.gameObject.SetActive(true);

            if (additionalAppearerController)
            {
                additionalAppearerController.AppearIn();
            }

            // Stop current tweens
            _tweenBaseFade?.Stop();
            _tweenBaseScale?.Stop();

            // Start new tweens
            _tweenBaseFade = Tween.CanvasGroupAlpha(canvasGroup, 1, tweenConfig.Duration, tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);
            _tweenBaseScale = Tween.LocalScale(modalWindowBoxTransform, _scalingRectInitialScale,
                tweenConfig.Duration,
                tweenConfig.Delay,
                tweenConfig.AnimationCurve, tweenConfig.loopType);

            // Showing window
            showingWindow?.Invoke(this);

            // uiFloatAndFadeIn.Appear(true);

            // Invoke callback
            callback?.Invoke();
        }

        /// <summary>
        /// Cheap way to update the text and image of the window
        ///
        /// A more mature method could be made up with the <see cref="NotificationPanelConfig"/>. Yet, the buttons need to be updated too.
        /// This more performant solution will do for now.
        /// </summary>
        /// <param name="newText"></param>
        /// <param name="newImage"></param>
        public void UpdateWindow(string newText = null, Sprite newImage = null)
        {
            if(newText != null)
                horizontalText.text = newText;

            if(newImage != null)
                horizontalImage.sprite = newImage;
        }

        #endregion
    }
}