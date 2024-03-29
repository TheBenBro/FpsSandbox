using UnityEngine;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public VisualEffect muzzleVFX;
    public LayerMask mask;
    public float impulseForce;
    public float impulseRadius;
    bool isEquipped;
    public Collider[] colliders;
    public GameObject[] children;
    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent != null)
        {
            if (this.transform.parent.tag == "GunPosition")
            {
                isEquipped = true;
                foreach (GameObject obj in children)
                {
                    obj.layer = 8;
                }
            }
        }
        else
        {
            isEquipped = false;
            foreach (GameObject obj in children)
            {
                obj.layer = 7;
            }
        }
        if (isEquipped)
        {
            fpsCam =  GetComponentInParent<Camera>();
            foreach(Collider col in colliders)
            {
                col.enabled = false;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                
                Shoot();
            }
        }
        else
        {
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
        }
    }

    void Shoot()
    {
        
        if (isEquipped)
        {
            //muzzleFlash.Play();
            muzzleVFX.Play();
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, mask,QueryTriggerInteraction.Ignore))
            {
                Target target = hit.transform.GetComponent<Target>();
                Target myCharacter = this.transform.GetComponentInParent<Target>();
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                if (target != null && target.team != myCharacter.team)
                {
                    target.TakeDamage(damage);
                }
                if(rb != null)
                {
                    rb.AddExplosionForce(impulseForce, -hit.normal, impulseRadius,25f, ForceMode.Impulse);
                }
            }
        }
    }
}
