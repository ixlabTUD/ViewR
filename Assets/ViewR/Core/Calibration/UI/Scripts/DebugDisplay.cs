using UnityEngine;
using UnityEngine.UI;



public class DebugDisplay : MonoBehaviour
{
    public Text displayText;
    [SerializeField] public float RVelocity;
    [SerializeField] public float LVelocity;
    [SerializeField] public float LAvgVelocity;
    [SerializeField] public float RAvgVelocity;
    [SerializeField] public float LKalmanVelocity;
    [SerializeField] public float RKalmanVelocity;
    [SerializeField] public float distanceBetweenFingerPoints;
    [SerializeField] public float timeElapsed;
    [SerializeField] public float recordingTimeElapsed;
    [SerializeField] public float distanceBetweenHeadAndHand;

    // Start is called before the first frame update
    void Start()
    {
        displayText = this.GetComponent<Text>();  
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = "LVelocity:"+LVelocity.ToString();
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nRVelocity:"+RVelocity.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nLAVGVelocity:"+LAvgVelocity.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nRAVGVelocity:"+RAvgVelocity.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nLKalmanVelocity:"+LKalmanVelocity.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nRKalmanVelocity:"+RKalmanVelocity.ToString());
        
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nDistanceBtwnWrists:"+distanceBetweenFingerPoints.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nDistanceBtwnHeadAndHand:"+distanceBetweenHeadAndHand.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nTimeElapsed:"+timeElapsed.ToString());
        displayText.text = displayText.text.Insert(displayText.text.Length,"\nRecordingTimeElapsed:"+recordingTimeElapsed.ToString());
    }

 
}
