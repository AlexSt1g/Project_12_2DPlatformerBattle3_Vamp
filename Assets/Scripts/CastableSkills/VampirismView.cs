using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VampirismView : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float _currentRendererAlphaValue;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _currentRendererAlphaValue = _spriteRenderer.color.a;
        TurnOff();
    }

    public void Initialize(float radius)
    {
        float coefficient = radius * 2 / transform.lossyScale.x;
        float circleLocalScaleComponentValue = transform.localScale.x * coefficient;
        transform.localScale = new Vector3(circleLocalScaleComponentValue, circleLocalScaleComponentValue, circleLocalScaleComponentValue);
    }

    public void TurnOn()
    {
        ChangeRendererAlphaValue(_currentRendererAlphaValue);
    }

    public void TurnOff()
    {
        ChangeRendererAlphaValue(0f);
    }

    private void ChangeRendererAlphaValue(float alphaValue)
    {
        Color tmpColor = _spriteRenderer.color;
        tmpColor.a = alphaValue;
        _spriteRenderer.color = tmpColor;
    }
}
