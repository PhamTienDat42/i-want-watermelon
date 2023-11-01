using TMPro;
using UnityEngine;

namespace GamePlay
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameController controller;

        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private TextMeshProUGUI highScoreTMP;
        [SerializeField] private TextMeshProUGUI watermelonCountTMP;

        private void Start()
        {

        }

        public void SetScore()
        {
            scoreTMP.text = controller.Score.ToString();
        }

        public void SetWatermelonCount()
        {
            watermelonCountTMP.text = controller.WaterMelonCount.ToString();
        }
    }
}