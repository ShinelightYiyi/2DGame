using System;
using UnityEngine;
using GameFrameWork;
using System.Collections;

namespace TarodevController
{
    
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        private bool canInput;

        #region Interface

        //ʵ�ֽӿ�

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        public event Action DushEvent;

        #endregion

        private float _time;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            canInput = true;
            canDush = false;
            EventCenter.Instance.AddEventListener<bool>("��������", (o) => ControlInput(o));
            EventCenter.Instance.AddEventListener<float>("�������",(o)=>StartBounce(o));

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;

            if(canInput)  GatherInput();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="o"></param>
        private void ControlInput(bool o)
        {
            canInput = o;
            if(o)
            {
                Debug.Log("���ڿ�������");
            }
            else
            {
                Debug.Log("���ڲ�������");
               // isHeld = false;
                _frameInput.Move.x = 0;
            }
        }


        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.K),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.K),
                hallHeld = Input.GetKey(KeyCode.J),
                DushDown = Input.GetKey(KeyCode.LeftShift),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };  //�����������

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }  //�ֱ�����

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }  //������Ծ
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            HandleHeld();
            HandleDush();


            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            //������

            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);
            //����컨��

            bool leftHallHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.left, _stats.GrounderDistance, 1<<3);

            bool rightHallHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.right, _stats.GrounderDistance, 1<<3);

            RaycastHit2D boxHitRight = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.right, _stats.GrounderDistance, 1<<8);  //��߼������

            RaycastHit2D boxHitLeft = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.left, _stats.GrounderDistance, 1<<8);  //��߼������

            if(boxHitLeft.collider != null )
            {
                BoxController box = boxHitLeft.collider.GetComponent<BoxController>();  
                if(isDushing && box.material == BoxMaterial.Iron)
                {
                    box.IronBoxDush(-1);
                }
                box.isPull = true;
            }
            else if(boxHitRight.collider != null)
            {
                BoxController box = boxHitRight.collider.GetComponent<BoxController>();
                if (isDushing && box.material == BoxMaterial.Iron)
                {
                    box.IronBoxDush(1);
                }
                box.isPull = true;
            }
            else if( boxHitRight.collider == null && boxHitLeft.collider == null )
            {
                EventCenter.Instance.EventTrigger("�뿪����");
            }

            


            if ( (leftHallHit || rightHallHit)  && !_frameInput.JumpDown)
            {
                Debug.Log("ײ��ǽ");  
                canHeld = true;
            }
            else
            {
                canHeld = false;
            }


            // Hit a Ceiling
            if (ceilingHit)
            {
               // Debug.Log("ײ���컨��");
                _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }


            if (!_grounded && groundHit)  
            {
              //  Debug.Log("�ŵ�");
                isFlying = false;
                _grounded = true;
                canDush = true;
                _coyoteUsable = true;  //����ʱ��
                _bufferedJumpUsable = true;  //Ԥ����
                _endedJumpEarly = false;  //�ж���Ծ���
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            else if (_grounded && !groundHit)
            {
             //   Debug.Log("�뿪����");
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);  
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion

        #region Held
        private bool canHeld;
        private bool isHeld;



        private void HandleHeld()
        {
            if(_frameInput.JumpDown)
            {
                canHeld = false;
            }


            if(canHeld && _frameInput.hallHeld && !_frameInput.JumpDown)
            {
                _frameVelocity = Vector2.zero;
                _jumpToConsume = true;
                Debug.Log("��ǽ");
                isHeld = true;
            }
            else
            {
                isHeld = false;
            }
        }


        #endregion

        #region Dush

        private bool isDushing;
        private bool canDush;
        private bool nextDushing = false;

        private void HandleDush()
        {
            if (_frameInput.DushDown && canDush)
            {
                StartCoroutine(RealDush());
              //  Debug.Log("���");
            }
        }

        private IEnumerator RealDush()
        {
            if(canDush && !nextDushing)
            {
                nextDushing = true;
                isDushing = true;
                canInput = false;
                DushEvent?.Invoke();
              //  Debug.Log("��̿�ʼ");
                if (_frameInput.Move.x>0 && _frameInput.DushDown)
                {
                    _frameVelocity = Vector2.right * _stats.DushPower;  
                }
                else if(_frameInput.Move.x < 0 && _frameInput.DushDown)
                {
                    _frameVelocity = Vector2.left * _stats.DushPower;
                }
                canDush = false;
                yield return new WaitForSeconds(0.03f);
                canInput = true;
               // yield return new WaitForSeconds(0.02f);
                yield return new WaitForSeconds(0.05f);
                isDushing = false;

                if (!isBounce) 
                { 
                _frameVelocity = Vector2.zero;
                _frameInput.Move = Vector2.zero;
                }

                yield return new WaitForSeconds(0.15f);
                nextDushing = false;
              //  Debug.Log("��̽���");
            }
        }


        #endregion

        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;  //���뻺��
        private bool _endedJumpEarly;  //�ж���Ծ�Ƿ���ǰ����  
        private bool _coyoteUsable; //����ʱ��
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;  //���ΰ���ʱ���С���趨ʱ��  
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;  //�뿪����ʱ��С������ʱ��  

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;  //�ж���Ծ�Ƿ���ǰ����  

            if (!_jumpToConsume && !HasBufferedJump ) return;

            if (_grounded || CanUseCoyote || isHeld) ExecuteJump();  //�ڵ��������ʱ�� ����Խ�����Ծ  

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();

        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0 && !isDushing)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;  //�жϵ���Ϳ��м���
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);  //�𽥼���  
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime); //��������
            }
        }

        #endregion


        #region Bounce

        bool canBounce = true;
        bool isBounce = false;
        bool isFlying = false;

        private void StartBounce(float direction)
        {
            if(canBounce && (isDushing || isFlying))  StartCoroutine(BouncePower(direction));
        }

        IEnumerator BouncePower(float direction)
        {
            canBounce = false;
            canInput = false;
            isBounce = true;

          //  if (!isDushing) _frameVelocity = -_frameVelocity * direction;
            if (isDushing || isFlying) 
            {
                isFlying = true;
               // Debug.LogWarning("���"); 
                _frameVelocity = ( new Vector2(-_frameVelocity.x  , 20f)) * direction; 
            }

            _frameInput.Move = Vector2.zero;
            yield return new WaitForSeconds(0.1f);  
            canInput = true;
            isBounce=false;
            yield return new WaitForSeconds(0.1f);
            canBounce = true;
        }    

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else if(!isHeld && !isDushing) //��������
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime); //�������ٶ�
            }
          
        }
        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;  //�����ƶ�

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("ȱ�������ļ�", this);
        }
#endif
    }



    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public bool hallHeld;
        public bool DushDown;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;

        public event Action DushEvent;
        public Vector2 FrameInput { get; }
    }
}