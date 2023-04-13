using UnityEngine;
using ViewR.Managers;

namespace ViewR.Core.OVR.Passthrough.Scripts
{
    public class PlaneLookAt : MonoBehaviour
    {

        private  Transform lookAtTarget;
        // Start is called before the first frame update
        void Start()
        {
            lookAtTarget = ReferenceManager.Instance.CenterEye;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(lookAtTarget);
            
            if (transform.rotation.eulerAngles.x > 45 && transform.rotation.eulerAngles.x < 180)
            {
                transform.eulerAngles = new Vector3(45.0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else if (transform.rotation.eulerAngles.x > 180 && transform.rotation.eulerAngles.x < 315)
            {
                transform.eulerAngles = new Vector3(-45.0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }
}
