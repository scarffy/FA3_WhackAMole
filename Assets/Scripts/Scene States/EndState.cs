using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class EndState : SceneState
    {
        private APIManager apiManager;
        private GameManager gameManager;

        private void Start()
        {
            apiManager = FindObjectOfType<APIManager>();
            gameManager = FindObjectOfType<GameManager>();
        }

        public override IEnumerator Run()
        {

            yield return null;
        }
    }
}
