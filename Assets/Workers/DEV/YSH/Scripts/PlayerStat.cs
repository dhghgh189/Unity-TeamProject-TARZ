using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField]
    private float jumpForce;
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }

    [SerializeField]
    private float dashSpeed;
    public float DashSpeed { get {return dashSpeed; } set { dashSpeed = value; } }

    [SerializeField]
    private float dashTime;
    public float DashTime { get { return dashTime; } set { dashTime = value; } }

    [SerializeField]
    private int[] meleeDamages;
    public int[] MeleeDamages { get { return meleeDamages; } set { meleeDamages = value; } }
}
