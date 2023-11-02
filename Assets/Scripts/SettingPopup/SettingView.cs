using GamePlay;
using TMPro;
using Utilities;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SettingPopup
{
    public class SettingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreTMP;
        [SerializeField] private TextMeshProUGUI watermelonCountTMP;

        private GameModel model;

        private void Awake()
        {
            var modelObj = GameObject.FindGameObjectWithTag(Constants.DataTag);
            if (modelObj != null)
            {
                model = modelObj.GetComponent<GameModel>();
            }
            else
            {
                SceneManager.LoadScene(Constants.MenuScene);
            }
        }

        private void Start()
        {
            highScoreTMP.text = PlayerPrefs.GetInt(Constants.HighScore, 0).ToString();
            watermelonCountTMP.text = PlayerPrefs.GetInt(Constants.WatermelonCount, 0).ToString();
        }

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