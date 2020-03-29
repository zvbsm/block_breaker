using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    // config params
    [SerializeField] Paddle paddle;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    // state
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // cached commponent references
    AudioSource audioSource;
    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start() {
        paddleToBallVector = transform.position - paddle.transform.position;
        audioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (!hasStarted) {
            LockBallToPaddle();
            LaunchBallFromPaddle();
        }
    }

    private void LaunchBallFromPaddle() {
        // 0 = left click
        if (Input.GetMouseButtonDown(0)) {
            hasStarted = true;
            // GetComponent to access the component that has been applied to the object.
            // setting the velocity requires creating a vector object to specify
            // the direction and distance to aim for (in x y coordinates)
            // this means to go slightly 2f/ at an angle,
            // and 15f saying how fast to move.
            rigidbody2D.velocity = new Vector2(xPush, yPush);
        }
    }

    private void LockBallToPaddle() {
        Vector2 paddlePosition = new Vector2(paddle.transform.position.x, paddle.transform.position.y);
        transform.position = paddlePosition + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Vector2 velocityTweak = new Vector2(GenerateRandomFloat(), GenerateRandomFloat());
        if (hasStarted) {
            AudioClip clip = ballSounds[Random.Range(0, ballSounds.Length)];
            audioSource.PlayOneShot(clip);
            rigidbody2D.velocity += velocityTweak;
        }
    }

    private float GenerateRandomFloat() {
        return Random.Range(0f, randomFactor);
    }
}
