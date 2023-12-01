using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTransition : MonoBehaviour, ITransition
{
  [SerializeField]
  private RectTransform _panel;
  [SerializeField]
  private float slideSpeed = 1000f;
  [SerializeField]
  private TransitionDirection transitionDirection = TransitionDirection.Left; // Default transition direction
  private Vector2 _defaultAnchored = default;

  private void Awake()
  {
    _defaultAnchored = _panel.anchoredPosition;
  }

  void Start()
  {
    // Ensure the panel is initially hidden
    HidePanel();
  }

  public void TransitionIn()
  {
    _panel.gameObject.SetActive(true);
    Vector2 targetPosition = GetTargetPosition();
    StartCoroutine(SlidePanel(targetPosition, 0.5f));
  }

  public void TransitionOut()
  {
    Vector2 targetPosition = GetHiddenPosition();
    StartCoroutine(SlidePanel(targetPosition, 0.5f, () => { _panel.gameObject.SetActive(false); }));
  }

  private void HidePanel()
  {
    _panel.anchoredPosition = GetHiddenPosition();
    _panel.gameObject.SetActive(false);
  }

  private Vector2 GetHiddenPosition()
  {
    switch (transitionDirection)
    {
      case TransitionDirection.Up:
        return new Vector2(_panel.anchoredPosition.x, _defaultAnchored.y - _panel.rect.height);
      case TransitionDirection.Down:
        return new Vector2(_panel.anchoredPosition.x, _defaultAnchored.y + _panel.rect.height);
      case TransitionDirection.Left:
        return new Vector2(_defaultAnchored.x - _panel.rect.width, _panel.anchoredPosition.y);
      case TransitionDirection.Right:
        return new Vector2(_defaultAnchored.x + _panel.rect.width, _panel.anchoredPosition.y);
      default:
        return Vector2.zero;
    }
  }

  private Vector2 GetTargetPosition()
  {
    return _defaultAnchored;
  }

  private System.Collections.IEnumerator SlidePanel(Vector2 targetPosition, float duration, Action callback = null)
  {
    float elapsedTime = 0;
    Vector2 startingPosition = _panel.anchoredPosition;

    while (elapsedTime < duration)
    {
      _panel.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / duration);
      elapsedTime += Time.deltaTime;
      yield return null;
    }

    _panel.anchoredPosition = targetPosition;
    callback?.Invoke();
  }
}
