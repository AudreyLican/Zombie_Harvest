using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit " + objectWeHit.gameObject.name + " !");

            createBulletImpactEffect(objectWeHit);

            Destroy(gameObject); //Destroy bullet when target is touch
        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");

            createBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }
    }

    void createBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        //we pass the prefab
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

}
