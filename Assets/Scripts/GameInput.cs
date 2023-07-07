using DG.Tweening;
using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class GameInput : MonoBehaviour
    { 
        private void Update()
        {
            // Escape pauses the game.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance.TogglePauseGame();
            }
        }
    }
}