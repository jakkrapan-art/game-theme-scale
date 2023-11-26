using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGame : MonoBehaviour
{
  private void Start()
  {
    GameController controller = new GameObject("GameController").AddComponent<GameController>();
    controller.Initialize();
  }
}
