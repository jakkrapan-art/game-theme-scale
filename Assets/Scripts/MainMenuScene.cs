using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
  private void Start()
  {
    SceneManager.LoadScene("GameScene");
  }
}
