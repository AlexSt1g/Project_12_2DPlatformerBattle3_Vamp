using System;
using UnityEngine;

public abstract class CastableSkill : MonoBehaviour
{
    public abstract event Action<float, float> TimeRemainingChanged;

    [field: SerializeField] public float CastingTime { get; protected set; } = 1f;
    [field: SerializeField] public float ReloadingTime { get; protected set; } = 1f;
    public bool IsActive { get; protected set; }
    public bool IsReloading { get; protected set; }

    public abstract void TryUseSkill();

    protected abstract void HandleEndOfSkillCasting();

    protected abstract void OnSkillUserDie();
}
