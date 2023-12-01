using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGame : MonoBehaviour
{
  [SerializeField] List<Tower> startTowers = new List<Tower>();

  private void Start()
  {
    GameController controller = new GameObject("GameController").AddComponent<GameController>();
    controller.Initialize(startTowers);
  }
}
