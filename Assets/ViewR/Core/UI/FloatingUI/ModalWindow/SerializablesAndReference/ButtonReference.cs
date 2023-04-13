using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference
{
    public class ButtonReference
    {
        public Button button;
        public Action onClickCallback;
        public TMP_Text textField;
        public Image imageRef;
        public Color defaultColor;
        public string defaultText;
        public readonly Sprite defaultBackgroundSprite;
        public readonly Color defaultFontColor;


        public ButtonReference(Button button)
        {
            textField = button.GetComponentInChildren<TMP_Text>();
            imageRef = button.GetComponentInChildren<Image>();
            defaultColor = imageRef.color;
            defaultBackgroundSprite = imageRef.sprite;
            defaultFontColor = textField.color;
            defaultText = textField.text;
            this.button = button;
        }
    }
}