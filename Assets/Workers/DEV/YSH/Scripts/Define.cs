using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public static readonly int HASH_ANIM_IDLE = Animator.StringToHash("Idle");
    public static readonly int HASH_ANIM_MOVE = Animator.StringToHash("Move");
    public static readonly int HASH_ANIM_JUMP = Animator.StringToHash("Jump");
    public static readonly int HASH_ANIM_FALL = Animator.StringToHash("Fall");
    public static readonly int HASH_ANIM_MELEE = Animator.StringToHash("Melee");
    public static readonly int HASH_ANIM_DASH = Animator.StringToHash("Dash");
    public static readonly int HASH_ANIM_DRAIN = Animator.StringToHash("Drain");
    public static readonly int HASH_ANIM_DAMAGED = Animator.StringToHash("Damage");

    public const int USEKEY_MAXCOUNT = 4;

    public enum SceneType { Title, Lobby, Game }
}
