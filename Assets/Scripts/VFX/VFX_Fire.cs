using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Fire : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public IEnumerator StartVFX()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        prefab.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

}
