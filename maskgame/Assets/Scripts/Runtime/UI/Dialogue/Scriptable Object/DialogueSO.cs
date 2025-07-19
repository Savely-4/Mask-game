using System.Linq;
using UnityEngine;

namespace Runtime.UI.Dialogue
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [SerializeField] private string characterName;

        [Header("Sentences")]
        [Multiline]
        [SerializeField] private string[] sentences;

        [Header("Answers")]
        [SerializeField] private Answer[] answers;


        public Dialogue GetDialogue()
        {
            var dialogueSentences = sentences.Select(t => new DialogueSentence(t)).ToArray();
            var dialogueAnswers = answers.Select(t => new DialogueAnswer(t.text, t.next?.GetDialogue())).ToArray();

            return new Dialogue(characterName, dialogueSentences, dialogueAnswers);
        }



        [System.Serializable]
        private struct Answer
        {
            [Multiline] public string text;
            public DialogueSO next;
        }
    }
}