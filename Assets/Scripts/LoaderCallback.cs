using UnityEngine;

namespace JamCraft.GMTK2023.Code
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool _isFirstUpdate = true;

        private void Update()
        {
            if (_isFirstUpdate)
            {
                _isFirstUpdate = false;

                // Call on first update.
                Loader.LoaderCallback();
            }
        }
    }
}