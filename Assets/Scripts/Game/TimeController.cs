using UnityEngine;

namespace GamePlay
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private GameView gameView;
        private float startTime;

        void Start()
        {
            startTime = Time.time;
        }

        void Update()
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
        }

        private void UpdateTimerText(float elapsedTime)
        {
            int hours = Mathf.FloorToInt(elapsedTime / 3600f);
            int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);

            gameView.SetTimer(hours, minutes, seconds);
        }
    }

}