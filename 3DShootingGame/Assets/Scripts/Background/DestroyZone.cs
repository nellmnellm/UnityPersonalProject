using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            Destroy(other.gameObject);
        
        other.gameObject.SetActive(false);
    }

 
}
