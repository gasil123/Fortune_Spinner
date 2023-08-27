using UnityEngine;
using UnityEngine.UI;

public class spinnerManager : MonoBehaviour
{

    public GameObject[] innerwinnings;
    public GameObject[] outerwinnings;
    public Transform InnerspinnerTransform;
    public Transform OuterspinnerTransform;
    public float spinSpeed;
    public float maxSpinDuration;
    public AnimationCurve spinCurve;

    AudioSource audio;
    Button button;
    private bool isSpinning = false;
    private float currentSpinTime = 0f;
    private float targetSpinSpeed = 0f;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        button.onClick.AddListener(StartSpin);
    }

    void Update()
    {
        Debug.Log(InnerspinnerTransform.rotation + "  " + OuterspinnerTransform.rotation);
        if (isSpinning)
        {
            //audio.Play();
            currentSpinTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(currentSpinTime / maxSpinDuration);
            float currentSpin = Mathf.Lerp(targetSpinSpeed, 0f, spinCurve.Evaluate(normalizedTime));
            InnerspinnerTransform.Rotate(Vector3.forward * currentSpin * Time.deltaTime);
            OuterspinnerTransform.Rotate(Vector3.back * currentSpin * Time.deltaTime);

            if (currentSpinTime >= maxSpinDuration)
            {
                //audio.Stop();
                isSpinning = false;
                // Determine the section the spinner stopped on
                DetermineFinalSection();
            }
        }
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            foreach(GameObject innerwinning in innerwinnings)
            {
                innerwinning.SetActive(false);
            } 
            foreach(GameObject outerwinning in outerwinnings)
            {
                outerwinning.SetActive(false);
            }
            // Generate a random spin speed for variety
            targetSpinSpeed = Random.Range(spinSpeed * 0.8f, spinSpeed * 1.2f);
            currentSpinTime = 0f;
            isSpinning = true;
        }
    }

    private void DetermineFinalSection()
    {
        // Calculate the angle the spinner ended at
        float innerspinnerAngle = InnerspinnerTransform.eulerAngles.z;
        float outerspinnerAngle = OuterspinnerTransform.eulerAngles.z;

        // Normalize the angle to a positive value
        if (innerspinnerAngle < 0)
        {
            innerspinnerAngle += 360f;
        } 
        if (outerspinnerAngle < 0)
        {
            outerspinnerAngle += 360f;
        }
        // Calculate the angle per section based on the number of sections (12 in our case)
        float anglePerSection = 360f / 12;

        // Calculate the section index where the spinner stopped
        int sectionIndex1 = Mathf.FloorToInt(innerspinnerAngle / anglePerSection);
        int sectionIndex2 = Mathf.FloorToInt(outerspinnerAngle / anglePerSection);

        // Determine the reward based on the section index
        DetermineReward(sectionIndex1,sectionIndex2);
    }
    private void DetermineReward(int sectionIndex1, int sectionIndex2)
    {
        innerwinnings[sectionIndex1].SetActive(true);
        outerwinnings[sectionIndex2].SetActive(true);
    }
}
