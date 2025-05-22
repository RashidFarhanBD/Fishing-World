using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] 
    Rigidbody rb;
    [SerializeField] 
    Transform planet;
    [SerializeField]
    float floatHeight = 1;
    [SerializeField]
    float waterLeveOffset = 3f;

    [SerializeField] 
    Vector3 FloaterToPlanetCenter;
    [SerializeField]
    float CentreToFloaterDistance;
    [SerializeField] 
    float WaterSurfaceheight;
    [SerializeField] 
    float boatSubmergedValue;
    [SerializeField]
    float floaterCount =1;
    private float bouyancy;
    [SerializeField]
    private float waterDamping = .1f ;
    [SerializeField]
    private float waterAngularDamp =.25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()

    {

        float waveHeight = WaveManager.Instance.GetWaveheight(rb.position.x);


        FloaterToPlanetCenter = (transform.position - planet.position);
        var up = FloaterToPlanetCenter.normalized;

        CentreToFloaterDistance = FloaterToPlanetCenter.magnitude;
        WaterSurfaceheight = planet.localScale.y * 0.5f + waterLeveOffset;
        boatSubmergedValue = WaterSurfaceheight - CentreToFloaterDistance + waveHeight;
        // waveHeight = WaveManager.Instance.GetWaveheight(transform.position.x);
        //   rb.AddForceAtPosition(-up * Physics.gravity.magnitude, transform.position, ForceMode.Acceleration);

        rb.AddForceAtPosition((Physics.gravity.y/ floaterCount) * up, transform.position, ForceMode.Acceleration);

        if ( isUnderWater())
            
         {
             bouyancy = Mathf.Clamp01(boatSubmergedValue / floatHeight);
          
            //rb.AddForce(up * Physics.gravity.magnitude * bouyancy, ForceMode.Acceleration);
            rb.AddForceAtPosition(up * Physics.gravity.magnitude * bouyancy,transform.position,ForceMode.Acceleration);
            rb.AddForce(bouyancy * -rb.linearVelocity *  waterDamping * Time.fixedDeltaTime, ForceMode.VelocityChange); ;
            rb.AddTorque(bouyancy * -rb.angularVelocity * waterAngularDamp * Time.fixedDeltaTime, ForceMode.VelocityChange);


        }
    }


    public bool isUnderWater()
    {

        if (boatSubmergedValue >0 )
        {
            Debug.Log("under");

            return true;
        }


        //Physics.Raycast(transform.position, -transform.up,out RaycastHit hit,1);
        //if (hit.collider)
        //{

            





        //}

        return false ;
    }


    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position,transform.position + -transform.up*1);
    }
}
