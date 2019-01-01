using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerVariables : ScriptableObject
{
    public float MoveSpeedBase = 10f;
    public float[] MovespeedMultiplier = { 1f };
    public int MoveSpeedCurModifier = 0;

    public float JumpStrengthBase = 5f;
    public float[] JumpStrengthMultiplier = { 1f };
    public int JumpStrengthCurModifier = 0;
}
