using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerMovement))]
public class PlayerShooting : MonoBehaviour
{

    [Header("References")]

    [SerializeField] private Transform Ball;
    [SerializeField] private Transform PosDribble;
    [SerializeField] private Transform PosOverHead;
    [SerializeField] private Transform Target;


    [Header("UI References")]
    [SerializeField] private Button chargeShootBallButton;
    [SerializeField] private Button launchBallButton;

    [Header("Script References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Shooting Attributes")]
    [SerializeField] private float timeRange;

    [SerializeField] private bool IsBallInHands = true;
    [SerializeField] private bool IsBallFlying = false;
    [SerializeField] private float T = 0;
    [SerializeField] private bool isCharging = false;


    [SerializeField] private float ballTravelSpeed;

    [Header("Shooting Mechanics Attributes")]
    [SerializeField] private Slider chargeSlider;
    [SerializeField] private float sliderSpeed = 2.0f;
    [SerializeField] private bool isIncreasing = true;


 

    [Header("Dribbling Attributes")]
    [SerializeField] private float dribblingSpeed = 5f;
    [SerializeField] private float dribblingHeight = 2f;

    [Header("Animator Reference")]
    public Animator animator;


    // privates
    private float errorRange = 6.0f;
    private float errorRangeX;
    private CameraSystem cameraSystem;
    void Start()
    {
        cameraSystem = Camera.main.GetComponent<CameraSystem>();

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
                Ball.position = PosDribble.position + new Vector3(0, dribblingHeight, 0) * Mathf.Abs(Mathf.Sin(Time.time * dribblingSpeed));
                playerMovement.haveBall = true;
            }
        }
        else
        {
            playerMovement.haveBall = false;
        }

        // condition if the ball is threw and in the air, it would run the logic that it will goes towards
        // the ring in a parabolic way.

        if (IsBallFlying)
        {
            Debug.Log("Slider Value: " + chargeSlider.value);

            Ball.GetComponent<BallController>().isStateFlying = true;

            

            T += Time.deltaTime;
            //float duration = 0.86f;
            float duration = ballTravelSpeed;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B;

            // slider mechanics for accuracy of the shot
            // also determines if the ball is going to the net accurately or not
            if (chargeSlider.value > 0.50f && chargeSlider.value < 0.60f)
            {
                B = Target.position;
            }
            else
            {
               
                B = Target.position + new Vector3(errorRangeX, 0, -3.0f);

                PlayerPointingSystem.Instance.ResetPoint();
            }


            // make the ball travel in arc position 
            Debug.Log("Slider Value: " + B);
            Vector3 pos = Vector3.Lerp(A, B, t01); // add lerp to the ball overhead position to the target destination which is the net
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * Mathf.PI);  // this line adds arc path to the ball to travel
            Ball.position = pos + arc; // move the ball

            // if the ball completed its travelling using the interpolation variable t01, set the ball into not flying and etc.
            if (t01 >= timeRange)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                Ball.GetComponent<BallController>().isStateFlying = false;
                Ball.GetComponent<BallController>().OnPlayer = false;
                Ball = null;
                
                cameraSystem.SetTarget(transform);
            }
            Debug.Log(t01);
        }

        // condition for the slider charge
        if (isCharging)
        {
            Charging();
        }
    }

    // Slider Mechanics
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
        animator.SetBool("ShootCharge", true);

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
            cameraSystem.SetTarget(Ball);

            animator.SetBool("ShootCharge", false);
            chargeShootBallButton.gameObject.SetActive(true);
            launchBallButton.gameObject.SetActive(false);

            if (!isCharging)
                return;

            isCharging = false;
            chargeSlider.gameObject.SetActive(false);

            IsBallInHands = false;

            errorRangeX = Random.Range(-errorRange, errorRange);
            //errorRangeZ = Random.Range(-errorRange, errorRange);

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

                other.gameObject.GetComponent<BallController>().OnPlayer = true;
            }
        }
    }
}
