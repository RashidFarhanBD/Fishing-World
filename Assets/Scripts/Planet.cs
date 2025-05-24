using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    public static Planet Instance { get; private set; }


    private void Awake()
    {
        if(Instance==null) Instance = this;
    }
    public float GetPlanetScale()
    {
        return transform.localScale.y;
    }
    public float GetDistFromPlanet(Vector3 pos)
    {

        return Vector3.Distance(transform.position, pos);
    }
    public Vector3 GetDirFromPlanet(Vector3 from)
    {

        return from - transform.position;
    }


    public float GetSubmergedValue(Transform t)
    {
        var tpos = t.position;
        var dist = GetDistFromPlanet(tpos);

        return (GetPlanetScale() * .5f -dist);
    }

    public bool CheckIfObjectBelowPlanet(Transform t,float offset=0)
    {
      
        if((GetSubmergedValue(t) + offset > 0))
        {

            return true;
        }
        return false;

    }


}
