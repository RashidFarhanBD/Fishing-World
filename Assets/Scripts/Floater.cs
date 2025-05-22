using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform planet;
    [SerializeField] float floatHeight;
    [SerializeField] Vector3 FloaterToPlanetCenter;
    [SerializeField] float CentreToFloaterDistance;
    [SerializeField] float waterLeveOffset = 3f;
    [SerializeField] float WaterSurfaceheight;
    [SerializeField] float boatSubmergedValue;

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

        rb.AddForceAtPosition((Physics.gravity.y/4) * up, transform.position, ForceMode.Acceleration);

        if ( isUnderWater())
            
         {
            float bouyancy = Mathf.Clamp01(boatSubmergedValue / floatHeight);
          
            //rb.AddForce(up * Physics.gravity.magnitude * bouyancy, ForceMode.Acceleration);
            rb.AddForceAtPosition(up * Physics.gravity.magnitude * bouyancy,transform.position,ForceMode.Acceleration);
           


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
