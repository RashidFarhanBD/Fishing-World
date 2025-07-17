using Unity.Cinemachine;
using UnityEngine;

public class Orbitter : MonoBehaviour
{
    public float speed;
    Rigidbody body;
    [Range(-20, 20)]
    [SerializeField] private float offset = 20;
    [SerializeField] private Transform player;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();

    }

    // Update is called once per frame



    private void Update()
    {

        // Tangential();
        //  OrbiterLogic();
        OrbiterLogic2();

        //    body.AddForce(  (( correctedPos + forward) - transform.position).normalized * speed * Time.deltaTime);

        //  body.MovePosition(body.position + (forward*1).normalized *speed*Time.deltaTime);





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


    private void Tangential()
    {
        //PLANET ER Y AXIS DIYE ROTATING

        float desiredDistanceFromCenter = Planet.Instance.transform.localScale.y * 0.5f + offset;
        Vector3 up = (transform.position - Planet.Instance.GetPlanetPos()).normalized;
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 tangentToPlayer = Vector3.ProjectOnPlane(toPlayer, up);  // this moves to it 
        Vector3 orbitDir = Vector3.Cross(up, tangentToPlayer).normalized; // orbit dir is giving me the transform right..... we wont move to the place but we will move around the place
        Debug.DrawLine(transform.position, transform.position + tangentToPlayer.normalized * 5, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + orbitDir.normalized * 5, Color.blue);

        body.MovePosition(transform.position + orbitDir * speed * Time.fixedDeltaTime);


        float waterLevelRadius = Planet.Instance.GetPlanetScale() * .5f + offset;
        // transform.position = Vector3.zero + (transform.position - Vector3.zero).normalized * waterLevelRadius;
    }
    private void OrbiterLogic2()
    {
        if (Vector3.Distance(transform.position, target.position) < 0.5f) return;
        Vector3 center = Vector3.zero;

        // "Up" is from planet center to fish
        Vector3 up = (transform.position - center).normalized;

        // Direction from fish to target
        Vector3 toTarget = (target.position - transform.position).normalized;

        // Project movement onto tangent plane (to keep movement on the sphere)
        Vector3 tangentDir = Vector3.ProjectOnPlane(toTarget, up).normalized;

        // Move in the tangent direction (around the sphere)
        Vector3 newPos = body.position + tangentDir * speed * Time.fixedDeltaTime;

        // Re-project to maintain fixed radius from planet center
        Vector3 fixedRadiusPos = center + (newPos - center).normalized * Planet.Instance.GetDistFromPlanet(target.position);
        body.MovePosition(newPos);

        // Rotate to face movement direction
        Quaternion targetRot = Quaternion.LookRotation(tangentDir, up);
        body.rotation = Quaternion.Slerp(body.rotation, targetRot, Time.fixedDeltaTime * 5f);
    }



    void Orbit3()
    {
       /* Vector3 toCenter = (transform.position - planet.position);
        Vector3 up = toCenter.normalized;

        //// Project a forward direction tangential to the planet's surface
        Vector3 forward = Vector3.Cross(transform.right, up).normalized;

        //// Zigzag pattern around the surface
        Vector3 zigzag = Vector3.Cross(up, forward) * Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;

        //// Combine forward and zigzag movement
        Vector3 moveDir = (forward + zigzag).normalized;

        //// Move the fish while staying on the shell
        Vector3 targetPos = planet.position + up * orbitRadius;

        //// Move toward the tangent direction
        rb.MovePosition(rb.position + moveDir * orbitSpeed * Time.fixedDeltaTime);

        //// Keep fish at constant radius
        Vector3 newPos = planet.position + (rb.position - planet.position).normalized * orbitRadius;
        rb.position = newPos;

        //// Rotate to face movement
        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(moveDir, up), Time.fixedDeltaTime * 5f);*/
    }

  
}
