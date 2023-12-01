using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRestartScene : MonoBehaviour
{
  public void ToMainMenu()
  {
    SceneManager.LoadScene("MainMenuScene");
  }
}
