using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer characterRenderer;

        Rigidbody2D _rigidbody;
        AnimationHandler animationHandler;

        Vector2 movementDirection = Vector2.zero;
        Vector2 lookDirection = Vector2.right;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            animationHandler = GetComponent<AnimationHandler>();
        }

        private void Update()
        {
            Rotate(lookDirection);
        }

        private void FixedUpdate()
        {
            Movment(movementDirection);
        }

        private void Rotate(Vector2 direction)
        {
            if (direction == Vector2.zero) return;
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bool isLeft = Mathf.Abs(rotZ) > 90f;
            characterRenderer.flipX = isLeft;
        }

        private void Movment(Vector2 direction)
        {
            direction = direction.normalized * 3f;
            _rigidbody.velocity = direction;
            animationHandler.Move(direction);
        }

        void OnMove(InputValue inputValue)
        {
            movementDirection = inputValue.Get<Vector2>();
        }
    }
}
