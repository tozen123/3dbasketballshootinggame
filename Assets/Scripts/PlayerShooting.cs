using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;
    public Slider chargeSlider; // Slider to show the charge level
    public Button startChargeButton;  // Button to start charging
    public Button shootButton;  // Button to release and shoot

    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;
    private float currentChargeTime = 0f;
    private bool isCharging = false;
    [SerializeField] private float maxChargeTime = 2f; // Max time to fully charge the shot
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 20f;

    void Start()
    {
        chargeSlider.gameObject.SetActive(false); // Hide the slider initially
        startChargeButton.onClick.AddListener(StartCharging);
        shootButton.onClick.AddListener(Shoot);
    }

    void Update()
    {
        // Ball in hands
        if (IsBallInHands)
        {
            // Hold over head
            if (isCharging)
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // Look towards the target
                transform.LookAt(Target.position);

                ContinueCharging();
            }
            else
            {
                // Dribbling
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }
        }

        // Ball in the air
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // Move to target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // Move in arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // Moment when ball arrives at the target
            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void StartCharging()
    {
        if (!isCharging)
        {
            isCharging = true;
            currentChargeTime = 0f;
            chargeSlider.value = 0f; // Reset the slider value
            chargeSlider.gameObject.SetActive(true); // Show the slider when charging starts
        }
    }

    private void ContinueCharging()
    {
        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
            float chargePercent = Mathf.Clamp01(currentChargeTime / maxChargeTime);
            chargeSlider.value = chargePercent; // Update the slider value based on charge percentage
        }
    }

    private void Shoot()
    {
        if (!isCharging)
            return;

        isCharging = false;
        float chargePercent = Mathf.Clamp01(currentChargeTime / maxChargeTime);
        float shootForce = Mathf.Lerp(minForce, maxForce, chargePercent);

        // Check if the player released at the correct time (0.5 on the slider)
        bool isAccurateShot = Mathf.Abs(chargePercent - 0.5f) <= 0.1f;

        // Calculate direction to the target
        Vector3 directionToTarget = (Target.position - PosOverHead.position).normalized;

        Ball.position = PosOverHead.position;
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.GetComponent<Rigidbody>().isKinematic = false;

        if (isAccurateShot)
        {
            Ball.GetComponent<Rigidbody>().AddForce(directionToTarget * shootForce, ForceMode.Impulse);
        }
        else
        {
            // Add some random inaccuracy to the shot
            directionToTarget.x += Random.Range(-0.1f, 0.1f);
            directionToTarget.y += Random.Range(-0.1f, 0.1f);
            Ball.GetComponent<Rigidbody>().AddForce(directionToTarget * shootForce, ForceMode.Impulse);
        }

        currentChargeTime = 0f;
        chargeSlider.value = 0f; // Reset the slider value
        chargeSlider.gameObject.SetActive(false); // Hide the slider after shooting

        IsBallInHands = false;
        IsBallFlying = true;
        T = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying)
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
            Ball.position = PosDribble.position; // Reset ball position to dribble position
        }
    }
}
