using Pixelplacement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViewR.HelpersLib.Extensions.EditorExtensions.HelpBox;

namespace ViewR.HelpersLib.SurgeExtensions.StateMachineExt
{
    /// <summary>
    /// Class that changes the state of the state machine (and can listen to additional events)
    /// </summary>
    public class TabButton : Selectable, IPointerClickHandler
    {
        [Space]
        [Header("StateMachine settings")]
        [Help("Additional events can be set up below - the loading of the state does not need to be set up as an event here.")]
        [SerializeField] 
        private StateMachine stateMachine;
        [SerializeField] 
        private GameObject loadState;
        
        [Header("Setup")]
        [SerializeField] 
        private bool changeSpriteIfActiveTabUpdated;
        [SerializeField, Tooltip("Should we \"select\"/\"click\" this tab on enable? Requires statemachine to have returnToDefaultOnDisable set to true.")]
        private bool returnToThisStateOnEnable;
        
        [Header("Refs")]
        [SerializeField] 
        private TabButtonManager tabButtonManager;
        [Space]

        private Image _buttonImage;
        private bool _selected;
        private Sprite _defaultSprite;
        
        public bool selected
        {
            get => _selected;
            set
            {
                _selected = value;
            }
        }

        // ToDO: set text field and get name based on referenced gameobject

        // Event delegates triggered on click.
        [SerializeField] private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
        

        public Button.ButtonClickedEvent onClick
        {
            get => m_OnClick;
            set => m_OnClick = value;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Press();
        }

        protected override void Awake()
        {
            base.Awake();

            if(!_buttonImage)
                _buttonImage = GetComponent<Image>();
            if(_buttonImage)
                _defaultSprite = _buttonImage.sprite;
            if(tabButtonManager)
                tabButtonManager.ATabButtonWasPressed += UpdateButtonVisuals;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (!Application.isPlaying)
                return;
            
            // Basically our "SelectThisButtonOnStart" class integrated in this class.
            if(returnToThisStateOnEnable && stateMachine.returnToDefaultOnDisable)
            {
                var data = new PointerEventData(EventSystem.current)
                {
                    button = PointerEventData.InputButton.Left
                };
                OnPointerClick(data);
            }
        }

        /// <summary>
        /// Updates the visuals if it should (<see cref="changeSpriteIfActiveTabUpdated"/>).
        /// </summary>
        private void UpdateButtonVisuals(TabButton activeTabButton)
        {
            if(!changeSpriteIfActiveTabUpdated) return;

            _buttonImage.sprite = activeTabButton == this
                ? tabButtonManager.buttonSelectedSprite
                : _defaultSprite;
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;
            
            // Update UI
            tabButtonManager.OnATabButtonWasPressed(this);

            // Set state machine
            stateMachine.ChangeState(loadState);

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick?.Invoke();

            selected = !selected;
        }
    }
}
