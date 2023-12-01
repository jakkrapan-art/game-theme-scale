using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePhase : MonoBehaviour
{
  [SerializeField]
  private Text _phaseText;

  public void SetGamePhase(int phaseIndex)
  {
    var phase = phaseIndex == 0 ? "Buy" : "Play";
    _phaseText.text = phase;
  }
}
