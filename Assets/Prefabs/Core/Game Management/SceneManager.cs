using System.Collections;
using System.Linq;
using UnityEngine;

namespace WhackAMole
{
    /// <summary>
    /// Manages the scene state flow, eg: A State Machine for current scene
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        public SceneState currentSceneState;
        public BaseGameData gameData;

        private SceneState[] sceneStates;

        // Start is called before the first frame update
        void Start()
        {
            sceneStates = FindObjectsOfType<SceneState>(true);

            DisableAllSceneStates();

            if (currentSceneState == null)
            {
                Debug.LogWarning("Current scene is null, please assign a scene state");
            }

            RunSceneState(currentSceneState);
        }

        void DisableAllSceneStates()
        {
            foreach (var sceneState in sceneStates)
            {
                sceneState.gameObject.SetActive(false);
            }
        }

        public void RunSceneState(SceneState sceneState)
        {
            sceneState = sceneStates.Where(s => s == sceneState).FirstOrDefault();

            if (sceneState != null)
            {
                sceneState.OnRunEnded -= HandleStateSceneOnRunEnded;
                sceneState.OnRunEnded += HandleStateSceneOnRunEnded;

                sceneState.OnExitEnded -= HandleStateSceneOnExitEnded;
                sceneState.OnExitEnded += HandleStateSceneOnExitEnded;

                sceneState.gameData = gameData;
                sceneState.gameObject.SetActive(true);
                currentSceneState = sceneState;
                StartCoroutine(RunCompleteSceneStateCoroutine(sceneState));
            }
        }

        IEnumerator RunCompleteSceneStateCoroutine(SceneState sceneState)
        {
            yield return sceneState.Entry();
            yield return sceneState.Run();
        }

        /// <summary>
        /// Handles what to do with the scene state after it has finished executing the run method
        /// First it will run the scene state's exit method
        /// Then it will run the next scene's entry method specified by the current scene state
        /// </summary>
        /// <param name="sceneState"></param>
        void HandleStateSceneOnRunEnded(SceneState sceneState)
        {
            // Starts the scene state's exit coroutine 
            StartCoroutine(sceneState.Exit());

            SceneState nextStateScene = sceneState.next;

            if (nextStateScene != null)
            {
                RunSceneState(nextStateScene);
            }
        }

        void HandleStateSceneOnExitEnded(SceneState sceneState)
        {
            if (currentSceneState == sceneState)
            {
                return;
            }

            sceneState.gameObject.SetActive(false);
        }

    }
}
