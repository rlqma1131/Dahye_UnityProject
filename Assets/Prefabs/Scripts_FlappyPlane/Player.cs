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
            gameManager = GameManager.Instance; // �̱��� ������ ����Ͽ� GameManager �ν��Ͻ��� �����ɴϴ�.

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
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // ����Ʈ�� �� ��ɵ� ����
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
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // ����Ʈ�� �� ��ɵ� ����
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

            float angle = Mathf.Clamp((_rigidbody.velocity.y * 10f), -90, 90); // y�ӵ��� ���� ������ �������ش�.
            transform.rotation = Quaternion.Euler(0, 0, angle); // rotation�� Quanternion���� ���� ������ ���Ϸ��� Ȱ���Ͽ� ������ �������ش�.
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (godMode) return;

            if (isDead) return;

            isDead = true;
            deathCooldown = 1f;

            animator.SetInteger("IsDie", 1);
            gameManager.GameOver(); // ���� ���� ó��

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

            //// �ӵ� �ʱ�ȭ
            //_rigidbody.velocity = Vector2.zero;
        }
    }
}
