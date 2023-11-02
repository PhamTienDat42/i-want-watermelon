using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace SettingPopup
{
    public class SettingView : MonoBehaviour
    {
        public void OnContinueButtonClick()
        {
            Time.timeScale = 1.0f;
            PopupHelpers.Close();
        }

        public void OnHomeButtonClick()
        {
            SceneManager.LoadScene(Constants.MenuScene);
        }
    }
}