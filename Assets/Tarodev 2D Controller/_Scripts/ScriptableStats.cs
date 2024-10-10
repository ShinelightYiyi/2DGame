using UnityEngine;

namespace TarodevController
{
    [CreateAssetMenu]
    public class ScriptableStats : ScriptableObject
    {
        [Header("LAYERS")] [Tooltip("角色所在layer")]
        public LayerMask PlayerLayer;

        [Header("INPUT")] [Tooltip("是否键盘输入")]
        public bool SnapInput = true;

        [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
        public float VerticalDeadZoneThreshold = 0.3f;

        [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
        public float HorizontalDeadZoneThreshold = 0.1f;

        [Header("MOVEMENT")] [Tooltip("水平移动最大速度")]
        public float MaxSpeed = 14;

        [Tooltip("水平加速度")]
        public float Acceleration = 120;

        [Tooltip("地面摩擦力")]
        public float GroundDeceleration = 60;

        [Tooltip("空气摩擦力")]
        public float AirDeceleration = 30;

        [Tooltip("地面力,控制对象在地面上"), Range(0f, -10f)]  
        public float GroundingForce = -1.5f;

        [Tooltip("检测墙面距离"), Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;

        [Tooltip("冲刺"), Range(0f, 10000f)]
        public float DushPower = 500f;

        [Header("JUMP")] [Tooltip("The immediate velocity applied when jumping")]
        public float JumpPower = 36;

        [Tooltip("The maximum vertical movement speed")]
        public float MaxFallSpeed = 40;

        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        public float FallAcceleration = 110;

        [Tooltip("The gravity multiplier added when jump is released early")]
        public float JumpEndEarlyGravityModifier = 3;

        [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        public float CoyoteTime = .15f;

        [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
        public float JumpBuffer = .2f;
    }
}