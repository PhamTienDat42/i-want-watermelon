using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuView : MonoBehaviour
    {
        public void OnPlayButtonClick()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }
    }
}