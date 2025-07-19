using System;
using UnityEngine;

namespace Runtime.UI.Dialogue
{
    public class DialogueInteractable : MonoBehaviour
    {
        public static event Action<Dialogue> DialogueStartCalled;

        [SerializeField] private DialogueSO startDialogue;


        [ContextMenu("Start Dialogue")]
        private void StartDialogue()
        {
            DialogueStartCalled?.Invoke(startDialogue.GetDialogue());
        }
    }
}