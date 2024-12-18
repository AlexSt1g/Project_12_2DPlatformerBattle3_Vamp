using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);
    private const KeyCode Jump = KeyCode.Space;
    private const KeyCode Attack = KeyCode.KeypadEnter;
    private const KeyCode SkillVampirism = KeyCode.KeypadPlus;

    public event Action JumpKeyPressed;
    public event Action AttackKeyPressed;
    public event Action SkillVampirismKeyPressed;

    public float Direction { get; private set; }

    private void Update()
    {
        Direction = Input.GetAxis(Horizontal);

        if (Input.GetKeyDown(Jump))
            JumpKeyPressed?.Invoke();

        if (Input.GetKeyDown(Attack))
            AttackKeyPressed?.Invoke();

        if (Input.GetKeyDown(SkillVampirism))
            SkillVampirismKeyPressed?.Invoke();
    }
}
