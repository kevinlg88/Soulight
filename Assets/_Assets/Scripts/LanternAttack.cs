using UnityEngine;
using System.Collections;

public class LanternAttack : MonoBehaviour
{
    [SerializeField] GameObject lanternLight;
    [SerializeField] float speed = 10f;
    [SerializeField] float hitDelay = 0.5f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float attackDamage = 5f;

    Transform targetEnemy;
    bool isAttacking = false;
    bool canAttack = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetEnemy = other.transform;
            canAttack = true;

            if (!isAttacking)
                StartCoroutine(Attack());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            canAttack = false;
        }
    }

    IEnumerator Attack()
    {
        if (!canAttack || isAttacking) yield break;
        isAttacking = true;

        GameObject go = Instantiate(lanternLight, transform.position, Quaternion.identity);

        while (targetEnemy != null && Vector3.Distance(go.transform.position, targetEnemy.position) > 0.1f)
        {
            go.transform.position = Vector3.MoveTowards(go.transform.position, targetEnemy.position, speed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Enemy take damage");
        yield return new WaitForSeconds(hitDelay);

        while (Vector3.Distance(go.transform.position, transform.position) > 0.1f)
        {
            go.transform.position = Vector3.MoveTowards(go.transform.position, transform.position, speed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Back to the lantern");
        Destroy(go,2f);
        while (go != null)
        {
            go.transform.position = Vector3.MoveTowards(go.transform.position, transform.position, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
