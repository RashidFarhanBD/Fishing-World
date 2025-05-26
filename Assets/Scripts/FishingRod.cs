using UnityEngine;
using UnityEngine.InputSystem;
public class FishingRod : MonoBehaviour
{
    float throwDist;
    bool baitThrown;
    bool fishCaught;
    public Bait baitPrefab;
    [SerializeField]LineRenderer lineRenderer;
    [SerializeField] Rigidbody rigid ;
    [SerializeField] Bait oldBait;
    Fish fish1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        Fish.OnFishCaught += FishCaught;
      
    }


    private void OnDisable()
    {
        Fish.OnFishCaught -= FishCaught;
    }
    void Start()
    {
         lineRenderer = GetComponent<LineRenderer>();
         rigid = GetComponent<Rigidbody>() ;
        
    }

    public void FishCaught(Fish fish)
    {
        fish1 = fish;
        fishCaught = true;
        CopySpringJoint(oldBait.joint, fish.joint);
        fish.joint.connectedBody = transform.parent.GetComponent<Rigidbody>() ;   
        Destroy(oldBait.gameObject);
        oldBait = null;
       

    }


    public void CopySpringJoint(SpringJoint source, SpringJoint target)
    {
       // target.autoConfigureConnectedAnchor = source.autoConfigureConnectedAnchor;
       // target.anchor = source.anchor;
       // target.connectedAnchor = source.connectedAnchor;
        target.connectedBody = source.connectedBody;
       // target.damper = source.damper;
       // target.spring = source.spring;
        target.minDistance = source.minDistance;
        target.maxDistance = source.maxDistance;
        target.tolerance = source.tolerance;
        target.enableCollision = source.enableCollision;
        target.enablePreprocessing = source.enablePreprocessing;
        target.breakForce = source.breakForce;
        target.breakTorque = source.breakTorque;
        target.massScale = source.massScale;
        target.connectedMassScale = source.connectedMassScale;
    }


    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            {
                if (oldBait)
                {
                    oldBait.SetBaitDead();
                }
                Debug.Log("throow");

                var baitObj = Instantiate  (baitPrefab, transform.position + transform.forward* 3,Quaternion.identity);
              var rbBait =   baitObj.GetComponent<Rigidbody>();
                rbBait.AddForce( transform.parent.forward * 50);
                baitObj.InitBait(lineRenderer, this.transform, rigid);
                oldBait = baitObj;
            }
        }



        if (fishCaught && fish1)
        {
            lineRenderer.SetPosition(0, transform.position);

            lineRenderer.SetPosition(1, fish1.transform.position);

        }
    }
}
