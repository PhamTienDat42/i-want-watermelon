using UnityEngine;

namespace GamePlay
{
    public class GameModel : MonoBehaviour
    {
        private int currentScore;

        private void Awake()
        {
            HighScore = PlayerPrefs.GetInt(Constants.HighScore, 0);
            WatermelonCount = PlayerPrefs.GetInt(Constants.WatermelonCount, 0);
        }

        private void Start()
        {
			DontDestroyOnLoad(gameObject);
        }

        public void SetHighScore(int highScore)
        {
            HighScore = highScore;
            PlayerPrefs.SetInt(Constants.HighScore, HighScore);
            PlayerPrefs.Save();
        }

        public void SetWatermelonCount()
        {
            WatermelonCount++;
            PlayerPrefs.SetInt(Constants.WatermelonCount, WatermelonCount);
            PlayerPrefs.Save();
        }

        public int CurrentScore { get => currentScore; set => currentScore = value; }

        public int HighScore { get; set; }

        public int WatermelonCount { get; set; }
    }
}