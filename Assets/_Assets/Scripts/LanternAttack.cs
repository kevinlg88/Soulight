using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class LanternAttack : MonoBehaviour
{
    [SerializeField] GameObject lanternLight;
    [SerializeField] float TimeToCompleteAnim = 5f;
    [SerializeField] float hitDelay;
    [SerializeField] float attackCooldown = 4f;
    [SerializeField] float attackDamage = 5f;

    Transform targetEnemy;
    bool isAttacking = false;
    bool canAttack = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy in range");
            targetEnemy = other.transform;
            canAttack = true;
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

        DOTween.Kill(transform);
        Sequence seq = DOTween.Sequence();

        // Go to enemy
        seq.Append(go.transform.DOMove(targetEnemy.position, TimeToCompleteAnim).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Debug.Log("Enemy take damage");

            }));

        seq.AppendInterval(hitDelay);

        seq.Append(go.transform.DOMove(transform.position, TimeToCompleteAnim).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                Debug.Log("Back to the lantern");
            }));
        seq.AppendInterval(hitDelay);
        seq.OnComplete(() =>
        {
            Destroy(go, 1);
        });
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
