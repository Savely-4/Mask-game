namespace Runtime.UI.Dialogue
{
    /// <summary>
    /// Class that represents character sentences and possible answers on it
    /// </summary>
    public class Dialogue
    {
        public string CharacterName;

        public DialogueSentence[] Sentences;
        public DialogueAnswer[] Answers;


        public Dialogue(string characterName, DialogueSentence[] sentences, DialogueAnswer[] answers)
        {
            CharacterName = characterName;

            Sentences = sentences;
            Answers = answers;
        }
    }
}