using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Dialogue
{
    //TODO: Add script for hideable panels
    public class DialogueWindow : MonoBehaviour, IDialogueUI
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AnswersWindow _answersWindow;

        [SerializeField] private TextMeshProUGUI _textSentence;

        [SerializeField] private Button _continueButton;

        public event Action DialogueStarted;
        public event Action DialogueFinished;

        private Dialogue current;
        private Queue<DialogueSentence> sentenceQueue = new();


        //TODO: Better Input with less coupling
        private void Start()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            _continueButton.onClick.AddListener(Continue);

            _answersWindow.AnswerSelected += OnAnswerSelected;
            DialogueInteractable.DialogueStartCalled += StartDialogue;
        }

        void OnDestroy()
        {
            _answersWindow.AnswerSelected -= OnAnswerSelected;
            DialogueInteractable.DialogueStartCalled -= StartDialogue;
        }



        //TODO: Add service for continue button
        //TODO: Remove cursor control
        public void StartDialogue(Dialogue dialogue)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            //Continue press subscribe

            sentenceQueue.Clear();
            foreach (var sentence in dialogue.Sentences)
                sentenceQueue.Enqueue(sentence);

            current = dialogue;

            ShowSentence(sentenceQueue.Dequeue());

            DialogueStarted?.Invoke();

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void Stop()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;

            sentenceQueue.Clear();
            current = null;

            DialogueFinished?.Invoke();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        public void Continue()
        {
            if (sentenceQueue.Count > 0)
            {
                ShowSentence(sentenceQueue.Dequeue());
                return;
            }

            if (current.Answers.Length > 0)
            {
                _answersWindow.ShowAnswers(current.Answers);
                return;
            }

            Stop();
        }



        private void OnAnswerSelected(DialogueAnswer answer)
        {
            if (answer.NextDialogue != null)
                StartDialogue(answer.NextDialogue);
        }



        private void ShowSentence(DialogueSentence dialogueSentence)
        {
            _textSentence.SetText(dialogueSentence.Text);
        }
    }
}