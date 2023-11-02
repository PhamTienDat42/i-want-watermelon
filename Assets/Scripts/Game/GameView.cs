using TMPro;
using UnityEngine;
using Utilities;

namespace GamePlay
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameController controller;

        [Space(8.0f)]
        [Header("TMP")]
        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private TextMeshProUGUI highScoreTMP;
        [SerializeField] private TextMeshProUGUI watermelonCountTMP;
        [SerializeField] private TextMeshProUGUI timerTMP;

        private void Start()
        {
            Time.timeScale = 1.0f;
        }

        public void SetScore()
        {
            scoreTMP.text = controller.Score.ToString();
        }

        public void SetWatermelonCount()
        {
            watermelonCountTMP.text = controller.WaterMelonCount.ToString();
        }

        public void SetTimer(int h, int m, int s)
        {
            timerTMP.text = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }

        public void OnSettingButtonClick()
        {
            PopupHelpers.Show(Constants.SettingPopup);
            Time.timeScale = 0f;
        }
    }
}