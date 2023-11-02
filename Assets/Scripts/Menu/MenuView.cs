using GamePlay;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreTMP;
        [SerializeField] private TextMeshProUGUI watermelonCountTMP;

        private void Start()
        {
            highScoreTMP.text = PlayerPrefs.GetInt(Constants.HighScore, 0).ToString();
            watermelonCountTMP.text = PlayerPrefs.GetInt(Constants.WatermelonCount, 0).ToString();
        }

        public void OnPlayButtonClick()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }
    }
}