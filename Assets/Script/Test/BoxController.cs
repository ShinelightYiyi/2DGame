using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using TMPro;

namespace GameFrameWork
{
    public class BoxController : MonoBehaviour
    {
        public BoxMaterial material;
        [SerializeField] int BouncePower = 1;

        private Rigidbody2D _rb;
        private BoxCollider2D _boxCollider;
        private Vector2 _frameVelocity;

        IPlayerController player;

       [SerializeField] float GravityForce = 1f;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();  
            _boxCollider = GetComponent<BoxCollider2D>();

            CheckMaterial();

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayerController>();

            EventCenter.Instance.AddEventListener("离开箱子", () => ControlCondition());
        }


        private void FixedUpdate()
        {
            CheckCollisions();

            if(withGravity) HandleGravity();
            ApplyForce();
        }

        private void CheckMaterial()
        {
            float massRate = 1f;
            switch(material)
            {
                case BoxMaterial.Wood:
                    massRate = 0.35f;
                    _rb.mass = gameObject.transform.localScale.x * massRate;
                    break;
                case BoxMaterial.Iron:
                    _rb.mass = 100f;
                    break;
                case BoxMaterial.Bouncy:
                    _rb.mass = 1f;
                    break;

            }
        }


        #region Check

       // float _time = 0f;

        [HideInInspector] public bool isPull= false;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;


            if(isPull)
            {
                //  Debug.LogWarning("右边接触玩家");  
                switch(material)
                {
                    case BoxMaterial.Wood:
                        _frameVelocity.x = player.FrameInput.x;
                        break;
                    case BoxMaterial.Iron:
                        // _frameVelocity.x = player.FrameInput.x * 1f;
                      //  _frameVelocity = Vector2.zero;
                        // Debug.LogWarning("Pull");
                        break;
                    case BoxMaterial.Bouncy:

                        EventCenter.Instance.EventTrigger<float>("弹飞玩家", BouncePower);

                        break;
                }    
            }




            bool groundHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.size, 0f, Vector2.down,0.01f,~6);  

            if (groundHit)
            {
                isGround = true;
           //     Debug.LogWarning("箱子在地面");
            }
            else
            {
               isGround = false;
            }
        }
        #endregion


        #region Dush

        bool isDushing = false;
        [SerializeField] float dushPower = 1f;

        /// <summary>
        ///1为右，-1为左
        /// </summary>
        /// <param name="o"></param>
        public void IronBoxDush(int o)
        {
            if (!isDushing)
            {
            //    Debug.LogWarning("箱子起飞");
                StartCoroutine(RealDush(o));
            }
        }

        IEnumerator RealDush(int o)
        {
            isDushing = true;
            withGravity = false;
            EventCenter.Instance.EventTrigger<float>("弹飞玩家", 0.5f);
            isPull = false;
            _rb.mass = 1f;
            _frameVelocity = new Vector2(dushPower * o, 0f);
            yield return new WaitForSeconds(0.1f);
            withGravity = true;
            yield return new WaitForSeconds(0.1f);
            _frameVelocity = Vector2.zero;
            _rb.mass = 100f;
            isDushing = false;
        }


        #endregion





        #region Gravity
        bool isGround = false;
        bool withGravity = true;

        private void HandleGravity()
        {
            if(isGround && _rb.velocity.y<=0)
            {
                _frameVelocity.y = -1.5f;
                Debug.LogWarning("碰到地面");
            }
            else
            {
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -40f, 110 * Time.fixedDeltaTime*GravityForce);
            }
        }

        #endregion

        private void ControlCondition() => isPull = false;

        private void ApplyForce() => _rb.velocity = _frameVelocity;

    }


    public enum BoxMaterial
    {
        Wood,
        Iron,
        Bouncy
    }

}
