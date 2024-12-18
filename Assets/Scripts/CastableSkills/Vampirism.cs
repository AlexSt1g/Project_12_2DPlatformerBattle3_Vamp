using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VampirismView))]
public class Vampirism : CastableSkill
{
    [SerializeField] private DamageablePerson _skillUser;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private int _damage = 4;
    [SerializeField] private float _damageDelay = 0.25f;

    private VampirismView _vampirismView;
    private Coroutine _skillCastingCoroutine;
    private Coroutine _HealthDrainingCoroutine;
    private WaitForSeconds _waitHealthDrainDelay;

    public override event Action<float, float> TimeRemainingChanged;

    private void Awake()
    {
        _vampirismView = GetComponent<VampirismView>();
        _waitHealthDrainDelay = new WaitForSeconds(_damageDelay);
    }

    private void Start()
    {
        _vampirismView.Initialize(_radius);
    }

    private void OnEnable()
    {
        _skillUser.Died += OnSkillUserDie;
    }

    private void OnDisable()
    {
        _skillUser.Died -= OnSkillUserDie;
    }

    public override void TryUseSkill()
    {
        if (IsActive == false && IsReloading == false)
        {
            IsActive = true;
            _vampirismView.TurnOn();

            _skillCastingCoroutine = StartCoroutine(UseSkillWithCastingTime());
        }
    }

    protected override void HandleEndOfSkillCasting()
    {
        StartCoroutine(ReloadWithReloadingTime());

        IsActive = false;
        _vampirismView.TurnOff();
    }

    protected override void OnSkillUserDie()
    {
        if (IsActive)
        {
            if (_skillCastingCoroutine != null)
                StopCoroutine(_skillCastingCoroutine);

            if (_HealthDrainingCoroutine != null)
                StopCoroutine(_HealthDrainingCoroutine);

            HandleEndOfSkillCasting();
        }
    }

    private IEnumerator UseSkillWithCastingTime()
    {
        _HealthDrainingCoroutine = StartCoroutine(DrainHealthWithDelay());

        for (float i = CastingTime; i > 0; i -= Time.deltaTime)
        {
            yield return null;

            TimeRemainingChanged?.Invoke(i, CastingTime);
        }

        HandleEndOfSkillCasting();
    }

    private IEnumerator DrainHealthWithDelay()
    {
        List<DamageablePerson> targets = new();
        DamageablePerson nearestTarget;

        while (IsActive)
        {
            Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayerMask);

            if (others.Length > 0)
            {
                foreach (var other in others)
                    if (other.TryGetComponent(out DamageablePerson target))
                        targets.Add(target);

                nearestTarget = targets[0];

                if (targets.Count > 1)
                    for (int i = 0; i < targets.Count; i++)
                        if (GetDistanceToTarget(targets[i].transform.position) < GetDistanceToTarget(nearestTarget.transform.position))
                            nearestTarget = targets[i];

                targets.Clear();

                if (nearestTarget.IsDead == false)
                {
                    nearestTarget.TakeHit(_damage);
                    _skillUser.Heal(_damage);
                }
            }

            yield return _waitHealthDrainDelay;
        }
    }

    private float GetDistanceToTarget(Vector3 targetPosition)
    {
        return (transform.position - targetPosition).magnitude;
    }

    private IEnumerator ReloadWithReloadingTime()
    {
        IsReloading = true;

        for (float i = 0; i < ReloadingTime; i += Time.deltaTime)
        {
            yield return null;

            TimeRemainingChanged?.Invoke(i, ReloadingTime);
        }

        TimeRemainingChanged?.Invoke(ReloadingTime, ReloadingTime);

        IsReloading = false;
    }
}
