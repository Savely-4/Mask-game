using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.Dialogue
{
    [RequireComponent(typeof(Button))]
    public class DialogueAnswerButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textAnswer;

        public static event Action<DialogueAnswer> Pressed;

        private DialogueAnswer answer;


        void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnPressed);
        }


        public void SetAnswer(DialogueAnswer value)
        {
            answer = value;

            _textAnswer.SetText(value.Text);
        }



        private void OnPressed()
        {
            Pressed?.Invoke(answer);
        }
    }
}