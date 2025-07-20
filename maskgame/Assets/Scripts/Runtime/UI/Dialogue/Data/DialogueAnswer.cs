namespace Runtime.UI.Dialogue
{
    public class DialogueAnswer
    {
        public string Text;
        public Dialogue NextDialogue;


        public DialogueAnswer(string text, Dialogue next)
        {
            Text = text;
            NextDialogue = next;
        }
    }
}