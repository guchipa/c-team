using System.Collections;
using UnityEngine;

public class WarpGate : MonoBehaviour
{
    public WarpGate linkedGate; 
    public float warpDelay = 1.5f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tank")) 
        {
            TankController tank = other.GetComponent<TankController>();
            if (tank != null && !tank.IsWarping) 
            {
                StartCoroutine(WarpTank(tank));
            }
        }
    }

    private IEnumerator WarpTank(TankController tank)
    {
        tank.StartWarping(warpDelay); 
        yield return new WaitForSeconds(warpDelay); 
        tank.transform.position = linkedGate.transform.position; 
        tank.EndWarping(); 
    }
}
