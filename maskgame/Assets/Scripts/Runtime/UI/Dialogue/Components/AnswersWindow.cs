using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.UI.Dialogue
{
    public class AnswersWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private DialogueAnswerButton _buttonPrefab;
        [SerializeField] private Transform _buttonsParent;

        private List<DialogueAnswerButton> buttons = new();

        public event Action<DialogueAnswer> AnswerSelected;


        void Start()
        {
            DialogueAnswerButton.Pressed += OnButtonPressed;

            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        void OnDestroy()
        {
            DialogueAnswerButton.Pressed -= OnButtonPressed;
        }


        public void ShowAnswers(DialogueAnswer[] answers)
        {
            UpdateButtonCount(answers.Length);

            for (int i = 0; i < answers.Length; i++)
                buttons[i].SetAnswer(answers[i]);

            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }


        //TODO: Use object pooling service
        private void UpdateButtonCount(int count)
        {
            if (buttons.Count == count)
                return;

            //Create additional buttons
            while (buttons.Count < count)
            {
                var button = Instantiate(_buttonPrefab, _buttonsParent);
                buttons.Add(button);
            }

            //Delete extra buttons from top
            while (buttons.Count > count)
            {
                var lastIndex = buttons.Count - 1;
                Destroy(buttons[lastIndex].gameObject);
                buttons.RemoveAt(lastIndex);
            }
        }


        private void OnButtonPressed(DialogueAnswer answer)
        {
            AnswerSelected?.Invoke(answer);

            //Hide
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }
}