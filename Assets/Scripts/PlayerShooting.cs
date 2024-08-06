using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerShooting : MonoBehaviour
{
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Target;

    public Button chargeShootBallButton;
    public Button launchBallButton;

    public PlayerMovement playerMovement;

    public float timeRange;

    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;
    private bool isCharging = false;

    public Slider chargeSlider;
    public float sliderSpeed = 2.0f;
    private bool isIncreasing = true;

    float errorRange = 4.0f;
    float errorRangeX;
    float errorRangeZ;

    void Start()
    {
        launchBallButton.gameObject.SetActive(false);
        chargeSlider.gameObject.SetActive(false);
        chargeShootBallButton.onClick.AddListener(StartCharging);
        launchBallButton.onClick.AddListener(Shoot);
    }

    void Update()
    {
        // condition if the ball is not on the player hand, shoot button is disabled
        if (!Ball)
        {
            IsBallInHands = false;
            chargeShootBallButton.interactable = false;
        }
        else
        {
            chargeShootBallButton.interactable = true;
        }

        // condition if the ball is in the players hand, it will bounce and if charging it will be on the players over head
        if (IsBallInHands)
        {
            if (isCharging)
            {
                Ball.position = PosOverHead.position;
                LookAtTarget(Target.transform);
            }
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));

            }
        }

        // condition if the ball is threw and in the air, it would run the logic that it will goes towards
        // the ring in a parabolic way.

        if (IsBallFlying)
        {
            Debug.Log("Slider Value: " + chargeSlider.value);

            Ball.GetComponent<BallController>().isStateFlying = true;


            T += Time.deltaTime;
            float duration = 0.86f;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B;

            if (chargeSlider.value > 0.50f && chargeSlider.value < 0.60f)
            {
                B = Target.position;
            }
            else
            {
               
                B = Target.position + new Vector3(errorRangeX, 0, errorRangeZ);
            }
            Debug.Log("Slider Value: " + B);
            Vector3 pos = Vector3.Lerp(A, B, t01);
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * Mathf.PI); 
            Ball.position = pos + arc;

            if (t01 >= timeRange)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                Ball.GetComponent<BallController>().isStateFlying = false;

                Ball = null;

            }
            Debug.Log(t01);
        }

        // condition for the slider charge
        if (isCharging)
        {
            Charging();
        }
    }

    void Charging()
    {
        if (isIncreasing)
        {
            chargeSlider.value += sliderSpeed * Time.deltaTime;
            if (chargeSlider.value >= 1.0f)
            {
                chargeSlider.value = 1.0f;
                isIncreasing = false;
            }
        }
        else
        {
            chargeSlider.value -= sliderSpeed * Time.deltaTime;
            if (chargeSlider.value <= 0.0f)
            {
                chargeSlider.value = 0.0f;
                isIncreasing = true;
            }
        }
    }

    private void StartCharging()
    {
        if (isCharging)
        {
            return;
           
        }

        if (!IsBallInHands)
        {
            return;
        }

        chargeShootBallButton.gameObject.SetActive(false);

        isCharging = true;
        chargeSlider.value = 0;

        chargeSlider.gameObject.SetActive(true);

        launchBallButton.gameObject.SetActive(true);

        LookAtTarget(Target.transform);

        playerMovement.canMove = false;
    }

    void LookAtTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;
            }
        }
    }

    private void Shoot()
    {
        if (IsBallInHands)
        {

            chargeShootBallButton.gameObject.SetActive(true);
            launchBallButton.gameObject.SetActive(false);

            if (!isCharging)
                return;

            isCharging = false;
            chargeSlider.gameObject.SetActive(false);

            IsBallInHands = false;

            errorRangeX = Random.Range(-errorRange, errorRange);
            errorRangeZ = Random.Range(-errorRange, errorRange);

            IsBallInHands = false;
            IsBallFlying = true;

            T = 0;
            playerMovement.canMove = true;

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            if (!Ball)
            {
                Ball = other.gameObject.transform;
                IsBallInHands = true;
            }
        }
    }
}
