using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyPlane
{
    public class Player : MonoBehaviour
    {
        Animator animator;
        Rigidbody2D _rigidbody;

        public GameObject Environments;
        public GameObject Obstacles;

        public float flapForce = 6f;
        public float forwardSpeed = 3f;
        public bool isDead = false;
        float deathCooldown = 0f;

        bool isFlap = false;

        private bool isGameStarted = false;

        public bool godMode = false;

        GameManager gameManager;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.Instance; // 싱글톤 패턴을 사용하여 GameManager 인스턴스를 가져옵니다.

            animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();

            if (animator == null)
                Debug.LogError("Not Founded Animator");

            if (_rigidbody == null)
                Debug.LogError("Not Founded Rigidbody");
            _rigidbody.simulated = false;
        }

        public void SetGame()
        {
            isGameStarted = true;
            _rigidbody.simulated = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isGameStarted)
                return;

            if (isDead)
            {
                if (deathCooldown <= 0f)
                {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // 스마트폰 탭 기능도 포함
                    {
                        GameManager.Instance.GameOver();
                    }
                }
                else
                {
                    deathCooldown -= Time.deltaTime;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // 스마트폰 탭 기능도 포함
                {
                    isFlap = true;
                }
            }

        }

        private void FixedUpdate()
        {
            if (isDead) return;

            Vector3 velocity = _rigidbody.velocity;
            velocity.x = forwardSpeed;

            if (isFlap)
            {
                velocity.y = flapForce;
                isFlap = false;
            }

            _rigidbody.velocity = velocity;

            float angle = Mathf.Clamp((_rigidbody.velocity.y * 10f), -90, 90); // y속도에 따라 범위를 조정해준다.
            transform.rotation = Quaternion.Euler(0, 0, angle); // rotation은 Quanternion값을 갖기 때문에 오일러를 활용하여 각도를 조정해준다.
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (godMode) return;

            if (isDead) return;

            isDead = true;
            deathCooldown = 1f;

            animator.SetInteger("IsDie", 1);
            gameManager.GameOver(); // 게임 오버 처리

        }

        public void ResetGame()
        {
            isGameStarted = false;
            _rigidbody.simulated = false;
            isDead = false;

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            Environments.transform.position = Vector3.zero;
            Obstacles.transform.position = Vector3.zero;

            //// 속도 초기화
            //_rigidbody.velocity = Vector2.zero;
        }
    }
}
