using UnityEngine;

[CreateAssetMenu]
public class PlayerVariables : ScriptableObject
{
    public float MoveSpeedBase = 10f;
    public float[] MovespeedMultiplier = { 1f };
    public int MoveSpeedCurModifier = 0;

    public float JumpSpeedBase = 0.08f;
    public float JumpHeightBase = 5f;
    public float[] JumpHeightModifier = { 1f };
    public int JumpHeightCurModifier = 0;

    public float PlayerWidth = 0.5f;
    public float PlayerHeightFromCenter = 0.4f;
    public float FallSpeedBase = 3f;
}
