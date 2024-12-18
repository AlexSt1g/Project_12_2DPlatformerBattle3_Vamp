using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CastBar : MonoBehaviour
{
    [SerializeField] private DamageablePerson _skillUser;
    [SerializeField] private CastableSkill _castableSkill;
    [SerializeField] private Slider _castBarSlider;
    [SerializeField] private bool _alphaSwitchEnabled = true;

    private CanvasGroup _canvasGroup;
    private float _alphaTurnOffDelay = 1f;
    private WaitForSeconds _waitAlphaTurnOffDelay;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _waitAlphaTurnOffDelay = new WaitForSeconds(_alphaTurnOffDelay);
    }

    private void Start()
    {
        DisplayCastBar(_castableSkill.CastingTime, _castableSkill.CastingTime);
    }

    private void OnEnable()
    {
        _castableSkill.TimeRemainingChanged += DisplayCastBar;
        _skillUser.Died += OnSkillUserDied;
        _skillUser.Revived += OnSkillUserRevived;
    }

    private void OnDisable()
    {
        _castableSkill.TimeRemainingChanged -= DisplayCastBar;
        _skillUser.Died -= OnSkillUserDied;
        _skillUser.Revived -= OnSkillUserRevived;
    }

    private void DisplayCastBar(float currentTimeValue, float maxTimeValue)
    {
        _castBarSlider.value = GetTimeValueForSlider(currentTimeValue, maxTimeValue);
    }

    private float GetTimeValueForSlider(float currentTimeValue, float maxTimeValue)
    {
        return currentTimeValue / maxTimeValue;
    }

    private void OnSkillUserDied()
    {
        if (_alphaSwitchEnabled)
            StartCoroutine(TurnOffAlphaWithDelay());
    }

    private IEnumerator TurnOffAlphaWithDelay()
    {
        yield return _waitAlphaTurnOffDelay;

        _canvasGroup.alpha = 0;
    }

    private void OnSkillUserRevived()
    {
        if (_alphaSwitchEnabled)
            _canvasGroup.alpha = 1;
    }
}
