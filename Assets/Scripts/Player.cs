using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Touch touch;
    private Vector2 beginTouchPosition, endTouchPosition;

    public float flipSpeed = 0.01f;
    public float wrongMovementDelay = 0.1f;
    public float flipAngleStep = 3f;
    public float jumpHeight = 2f;
    public float jumpSpeed = 7f;
    public float fallSpeed = 12f;

    public Transform center;
    public Transform left;
    public Transform right;

    private GameManager gameManager;
    private GameObject dustParticles;

    private bool canMove = true;
    public bool isJumping = false;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        center = gameObject.transform.Find("Center");
        left = center.Find("Left");
        right = center.Find("Right");
        dustParticles = center.Find("Dust Particles").gameObject;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (canMove)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        beginTouchPosition = touch.position;
                        break;

                    case TouchPhase.Ended:
                        endTouchPosition = touch.position;
                        if (Vector2.Distance(beginTouchPosition, endTouchPosition) < 20f)
                        {
                            canMove = false;
                            isJumping = true;
                            StartCoroutine("Jump");
                        }
                        else
                        {
                            if (beginTouchPosition.x < endTouchPosition.x)
                            {
                                canMove = false;
                                if (transform.position.x > 0.1f)
                                    StartCoroutine("WrongMovement");
                                else
                                    StartCoroutine("MoveRight");
                            }
                            else if (beginTouchPosition.x > endTouchPosition.x)
                            {
                                canMove = false;
                                if (transform.position.x < -0.1f)
                                    StartCoroutine("WrongMovement");
                                else
                                    StartCoroutine("MoveLeft");
                            }
                        }
                        break;
                }
            }
        }
    }

    IEnumerator MoveLeft()
    {
        dustParticles.SetActive(false);

        float angle = 0f;
        while (angle < 90f)
        {
            //float delta = flipAngleStep * gameManager.gameSpeed;
            angle += flipAngleStep;
            gameObject.transform.RotateAround(left.position, Vector3.forward, flipAngleStep);
            yield return new WaitForSeconds(flipSpeed / gameManager.gameSpeed);
        }
        gameObject.transform.RotateAround(left.position, Vector3.forward, 90f - angle);

        gameManager.ResetBonusParticlesPosition();
        center.rotation = Quaternion.identity;
        canMove = true;
        dustParticles.SetActive(true);
    }

    IEnumerator MoveRight()
    {
        dustParticles.SetActive(false);

        float angle = 0f;
        while (angle < 90f)
        {
            //float delta = flipAngleStep * gameManager.gameSpeed;
            angle += flipAngleStep;
            gameObject.transform.RotateAround(right.position, Vector3.back, flipAngleStep);
            yield return new WaitForSeconds(flipSpeed / gameManager.gameSpeed);
        }
        gameObject.transform.RotateAround(right.position, Vector3.back, 90f - angle);

        gameManager.ResetBonusParticlesPosition();
        center.rotation = Quaternion.identity;
        canMove = true;
        dustParticles.SetActive(true);
    }
    IEnumerator Jump()
    {
        StartCoroutine(FadeOutDust(0.2f));

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
        Vector3 velocity = Vector3.zero;

        while (true)
        {
            if (transform.position.y >= initialPosition.y + jumpHeight)
                isJumping = false;
            if (isJumping)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Time.deltaTime, jumpSpeed * gameManager.gameSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.05f, 1.2f, 1f), Vector3.Distance(transform.position, initialPosition)/Vector3.Distance(initialPosition, targetPosition));
            }
            else if (!isJumping)
            {
                transform.position = Vector3.SmoothDamp(transform.position, initialPosition, ref velocity, Time.deltaTime, fallSpeed * gameManager.gameSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Vector3.Distance(transform.position, targetPosition)/Vector3.Distance(initialPosition, targetPosition));
                if (transform.position == initialPosition)
                {
                    canMove = true;
                    StartCoroutine(FadeInDust(0.1f));
                    StopAllCoroutines();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WrongMovement()
    {
        iTween.ShakeScale(gameObject, new Vector3(0.1f, 0.1f, 0.1f), wrongMovementDelay);
        yield return new WaitForSeconds(wrongMovementDelay);
        canMove = true;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.51f);
    }

    IEnumerator FadeInDust(float time)
    {
        if (dustParticles)
        {
            dustParticles.SetActive(true);
            iTween.FadeTo(dustParticles, 1f, time);
            yield return null;
        }
    }

    IEnumerator FadeOutDust(float time)
    {
        if (dustParticles)
        {
            iTween.FadeTo(dustParticles, 0f, time);
            yield return new WaitForSeconds(time);
            dustParticles.SetActive(false);
        }
    }
}
