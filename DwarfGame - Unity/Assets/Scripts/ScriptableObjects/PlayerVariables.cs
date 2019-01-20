using UnityEngine;
using UnityEngine.Serialization;

namespace DwarfGame
{
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

        public float Width = 0.5f;
        public float HeightFromCenter = 0.4f;
        public float FallSpeedBase = 3f;

        public float Reach = 4f;
        public float SwingSpeed = 0.5f;
        public int BaseDamage = 2;
    }
}