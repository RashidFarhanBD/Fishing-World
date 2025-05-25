using System.Drawing;
using UnityEngine;
using DG.Tweening;
public class Bait : MonoBehaviour
{
    bool isBaitDead;
    [SerializeField]float allowedBaitSubmergeLevel=10;
    [SerializeField]LineRenderer lineRenderer;
   [SerializeField] Transform fishingPoint;
    Planet planetInstance;
    Rigidbody rb;
    SpringJoint joint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       planetInstance = Planet.Instance;
       rb = GetComponent<Rigidbody>();
       joint = GetComponent<SpringJoint>();

    }

    public void SetBaitDead()
    {
        isBaitDead = true;
        joint.connectedBody = null;
        rb.detectCollisions = false;
        transform.DOScale(Vector3.zero, 1).SetEase(Ease.Linear);
        
    }


    public void InitBait(LineRenderer line, Transform p, Rigidbody rigid)
    {
        if (rigid) Debug.Log("SIKE");
        if (line) Debug.Log("line fouind");
        if (p) Debug.Log("p here");
        planetInstance = Planet.Instance;
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<SpringJoint>();
        lineRenderer = line;
        fishingPoint = p;
        joint.connectedBody = rigid;

        lineRenderer.SetPosition(0, fishingPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, fishingPoint.position);

        lineRenderer.SetPosition(1, transform.position);
        joint.autoConfigureConnectedAnchor = true;
        joint.connectedAnchor = fishingPoint.position;
        if (planetInstance.GetSubmergedValue(transform) > allowedBaitSubmergeLevel)
        {

            // rb.constraints = RigidbodyConstraints.FreezeRotation;
            //rb.constraints = RigidbodyConstraints.FreezeAll;
           // rb.linearVelocity = Vector3.zero;
           // rb.angularVelocity = Vector3.zero;
            GetComponent<FauxGravityBody>().ToggleGravity(false);

        }
        else
        {

            GetComponent<FauxGravityBody>().ToggleGravity(true);
        }


    }
}
