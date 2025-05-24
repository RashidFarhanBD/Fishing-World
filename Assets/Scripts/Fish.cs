using UnityEngine;
using DG.Tweening;
public class Fish : MonoBehaviour
{
    [SerializeField]
    Transform planet;

    [SerializeField]
    float moveRadius;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float fishSpeed;
    [SerializeField]
    Vector3 spawnPos;
    [SerializeField]
    Vector3 nextPos;
    [SerializeField]
    fishbehaviour state= fishbehaviour.idle;
    [SerializeField]

    private float waterOffset=.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPos = transform.position;
        nextPos = FindNextPosition();
        //nextPos = spawnPos +  Random.insideUnitSphere * moveRadius * Random.Range(.5f, 1);
        //if (CheckifNextPositionIsUnderWater())
        //{
        //    nextPos = spawnPos + Random.insideUnitSphere * moveRadius * Random.Range(.5f, 1);

        //}
        //transform.DOLookAt(nextPos, .1f);
        //transform.DOMove(nextPos, 3).OnComplete(() => CheckifNextPositionIsUnderWater());
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(spawnPos, moveRadius);
    }

    bool CheckifNextPositionIsUnderWater()
    {
       var nextPosToPlanetCoreDist =   Vector3.Distance(nextPos, planet.position);
        if(nextPosToPlanetCoreDist > (planet.localScale.y*0.5f + (float )waterOffset))
        {

            return true;

        }


        return false;
    }

    // Update is called once per frame
    void Update()
    {




        //public Vector3 targetPosition;
        //public float moveSpeed = 5f;
        //public float waveAmplitude = 1f;
        //public float waveFrequency = 2f;

        //private void Update()
        //{
        //    // Smooth movement towards the target
        //    Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //    // Add the sine wave to the Y-position
        //    float sineValue = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        //    newPosition.y += sineValue;

        //    // Apply the combined position
        //    transform.position = newPosition;
        //}
  



        switch (state)
        {
            case fishbehaviour.idle:

                var dist = Vector3.Distance(transform.position, nextPos);
                var dir = nextPos -transform.position;
                if(dist >0.1f)
                {
                   // transform.DOLookAt(nextPos,1.5f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir.normalized),Time.deltaTime*7); 
                    transform.position += dir.normalized * fishSpeed * Time.deltaTime;
                }
               
                   
                else
                {
                    nextPos = FindNextPosition();
                }
              

               

                break;
            case fishbehaviour.attracted:
                break;
            case fishbehaviour.baited:
                break;
            case fishbehaviour.dead:
                break;
        }
    }

    private Vector3 FindNextPosition()
    {
       return spawnPos + Random.insideUnitSphere * moveRadius * Random.Range(.5f, 1);
    }

    public enum fishbehaviour
{

    idle,
    attracted,
    baited,
    dead,
}
}