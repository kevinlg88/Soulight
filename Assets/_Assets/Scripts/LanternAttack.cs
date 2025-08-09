using UnityEngine;
using DG.Tweening;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy in range");
            targetEnemy = other.transform;
            canAttack = true;
            Attack();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            canAttack = false;
        }
    }

    public void Attack()
    {
        if (!canAttack || isAttacking) return;
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
            isAttacking = false;
            Destroy(go,1);
        });

    }
}
