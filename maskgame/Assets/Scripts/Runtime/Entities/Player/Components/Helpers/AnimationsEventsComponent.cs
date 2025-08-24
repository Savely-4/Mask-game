using System;
using UnityEngine;

namespace Runtime.Entities.Player
{
    /// <summary>
    /// Class for catching animations events and providing a way to subscribe to them
    /// </summary>
    public class AnimationsEventsComponent : MonoBehaviour
    {
        public event Action ActionEnded;
        public event Action ActionStarted;

        public event Action MeleeStartedHitting;
        public event Action MeleeStoppedHitting;


        public void InvokeActionEnded()
        {
            ActionEnded?.Invoke();
        }

        public void InvokeActionStarted()
        {
            ActionStarted?.Invoke();
        }


        public void OnMeleeStartedHitting()
        {
            MeleeStartedHitting?.Invoke();
        }

        public void OnMeleeStoppedHitting()
        {
            MeleeStoppedHitting?.Invoke();
        }
    }
}