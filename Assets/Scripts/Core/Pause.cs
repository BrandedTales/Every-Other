using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] bool gamePaused = false;

        public void PauseGame()
        {
            gamePaused = true;
            Time.timeScale = 0;
        }

        public void UnpauseGame()
        {
            gamePaused = false;
            Time.timeScale = 1;
        }

        public bool GamePaused() { return gamePaused; }

    }
}