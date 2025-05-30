using Unity.Cinemachine;
using UnityEngine;

public class Orbitter : MonoBehaviour
{
    public float speed;
    Rigidbody body;
    [Range(-20,20)]
    [SerializeField]private float offset = 20;
    [SerializeField] private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();   
        
    }

    // Update is called once per frame



    private void Update()
    {
        //PLANET ER Y AXIS DIYE ROTATING
      //  Tangential();

        OrbiterLogic();

        //    body.AddForce(  (( correctedPos + forward) - transform.position).normalized * speed * Time.deltaTime);

        //  body.MovePosition(body.position + (forward*1).normalized *speed*Time.deltaTime);





    }

    private void Tangential()
    {
        float desiredDistanceFromCenter = Planet.Instance.transform.localScale.y * 0.5f + offset;
        Vector3 up = (transform.position - Planet.Instance.GetPlanetPos()).normalized;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 tangentToPlayer = Vector3.ProjectOnPlane(toPlayer, up);
        Vector3 orbitDir = Vector3.Cross(up, tangentToPlayer).normalized;
        Debug.DrawLine(transform.position, transform.position + tangentToPlayer.normalized * 5, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + orbitDir.normalized * 5, Color.blue);

        body.MovePosition(transform.position + orbitDir * speed * Time.fixedDeltaTime);


        float waterLevelRadius = Planet.Instance.GetPlanetScale() * .5f + offset;
        // transform.position = Vector3.zero + (transform.position - Vector3.zero).normalized * waterLevelRadius;
    }

    private void OrbiterLogic()
    {
        //PLANET ER X AXIS DIYE ROTATING
        float desiredDistanceFromCenter = Planet.Instance.transform.localScale.y * 0.5f + offset;
        Vector3 up = (transform.position - Planet.Instance.GetPlanetPos()).normalized;
        Vector3 targetPosition = Planet.Instance.GetPlanetPos() + up * desiredDistanceFromCenter;


        Vector3 correctedPos = Vector3.Lerp(body.position, targetPosition, 0.1f);
        Vector3 forward = Vector3.Cross(transform.right, up).normalized;
        Vector3 move = forward * speed * Time.deltaTime;
        body.MovePosition(correctedPos + move);
    }
}
