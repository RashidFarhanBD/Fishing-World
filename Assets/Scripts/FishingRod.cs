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

    [Space(2)]
    [Header("REEL")]

    public float minReelDist = 1;
    public float maxReelDist = 15;
    public float minDamp = 2;
    public float maxDamp = 5;
    public float reelSpeed = 4;

    public AnimationCurve reelCurve;
    public AnimationCurve reelCurve2;
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
        //atm no use but thanks
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
       // target.connectedBody = source.connectedBody;
       // target.damper = source.damper;
       // target.spring = source.spring;
      //  target.minDistance = source.minDistance;
       // target.maxDistance = source.maxDistance;
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
        var kb = Keyboard.current;  
        if (mouse.leftButton.wasPressedThisFrame && !fishCaught)
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
                lineRenderer.widthMultiplier = 1;
                lineRenderer.widthCurve = reelCurve;
            }
        }



        if (fishCaught && fish1)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.widthCurve = reelCurve2;

            lineRenderer.SetPosition(1,  fish1.transform.TransformPoint(  fish1.joint.anchor));
            lineRenderer.widthMultiplier = .5f;

            if ( mouse.rightButton.wasPressedThisFrame || kb.eKey.wasPressedThisFrame)
            {
                var maxdist = fish1.joint.maxDistance;
                fish1.joint.maxDistance = Mathf.Clamp(maxdist - reelSpeed * Time.deltaTime,minReelDist,maxReelDist);
                Vector3 dirToBait = (transform.position - fish1.GetRB().position).normalized;
                 // fish1.GetRB().AddForce(dirToBait * 1* Time.deltaTime, ForceMode.Force);

                Debug.Log("yes yes");

               

            }
            else
            {
                
               
            }




        }
    }
}
