using UnityEngine;

namespace MiniGamePlinko
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody2D rb;
        private CircleCollider2D cd;
        public PhysicsMaterial2D _material;

        // Set the initial bounciness value
        public float bounceFactor = 0.5f;

        //the force the ball bounces by when initially being dropped
        public float wakeUpBounceFactor = 0.8f;

        //the force the ball bounces by when it hits an obstacle    
        public float obstacleBounceFactor = 0.1f;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cd = GetComponent<CircleCollider2D>();

            //GetComponent<CircleCollider2D>().sharedMaterial = _material;
            // cd.sharedMaterial = _material;

            //_material = rb.sharedMaterial;
            //if (_material == null)
            //{
            //    Debug.LogError("ChangeBounciness script requires a Physics Material 2D component!");
            //    return;
            //}
            //else
            //{
            //}

            rb.Sleep();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && rb.IsSleeping())
            {
                rb.WakeUp();
                // Apply a force to the ball in a random direction
                Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                rb.AddForce(dropDirection * wakeUpBounceFactor, ForceMode2D.Impulse);
            }

            if (!rb.IsSleeping())
            {
                Mathf.Clamp(bounceFactor, 0f, 1f);
                _material.bounciness = bounceFactor;
                cd.sharedMaterial = _material;
            }

            //Debug.Log(_material.bounciness);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Calculate the bounce direction
            Vector2 incomingDirection = rb.velocity.normalized;
            Vector2 normal = collision.contacts[0].normal;
            Vector2 bounceDirection = Vector2.Reflect(incomingDirection, normal);

            // Add a random angle to the bounce direction
            float angle = Random.Range(-45, 45);
            bounceDirection = Quaternion.Euler(0, 0, angle) * bounceDirection;

            // Apply the bounce force
            rb.AddForce(bounceDirection * obstacleBounceFactor, ForceMode2D.Impulse);
        }
    }
}