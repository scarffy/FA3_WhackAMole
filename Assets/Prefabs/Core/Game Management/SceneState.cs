using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public abstract class SceneState : MonoBehaviour
{
       public SceneState next;

       [HideInInspector]
       public BaseGameData gameData;

        #region Events
        // Entry
        public delegate void OnEntryStartedEventHandler(SceneState sceneState);
        public event OnEntryStartedEventHandler OnEntryStarted;
        public delegate void OnEntryEndedEventHandler(SceneState sceneState);
        public event OnEntryEndedEventHandler OnEntryEnded;

        // Run
        public delegate void OnRunStartedEventHandler(SceneState sceneState);
        public event OnRunStartedEventHandler OnRunStarted;
        public delegate void OnRunEndedEventHandler(SceneState sceneState);
        public event OnRunEndedEventHandler OnRunEnded;

        // Exit
        public delegate void OnExitStartedEventHandler(SceneState sceneState);
        public event OnExitStartedEventHandler OnExitStarted;
        public delegate void OnExitEndedEventHandler(SceneState sceneState);
        public event OnExitEndedEventHandler OnExitEnded;
        #endregion

        public IEnumerator Entry()
        {
            OnEntryStarted?.Invoke(this);
            yield return OnEntry();
            OnEntryEnded?.Invoke(this);
        }

        public virtual IEnumerator OnEntry()
        {
            yield return null;
        }

        public abstract IEnumerator Run();

        public IEnumerator Exit()
        {
            OnExitStarted?.Invoke(this);
            yield return OnExit();
            OnExitEnded?.Invoke(this);
        }

        public virtual IEnumerator OnExit()
        {
            yield return null;
        }

        public virtual void Finished()
        {
            OnRunEnded?.Invoke(this);
        }
}

