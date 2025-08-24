using System;

namespace Runtime.UI.Dialogue
{
    /// <summary>
    /// Interface for dialogue UI controls and events
    /// </summary>
    public interface IDialogueUI
    {
        event Action DialogueStarted;
        event Action DialogueFinished;

        void StartDialogue(Dialogue dialogue);
        void Stop();
    }
}