using System.Collections;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;

    void Start()
    {
        StartCoroutine(SelfDestruct(duration));    
    }

    public IEnumerator SelfDestruct(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}