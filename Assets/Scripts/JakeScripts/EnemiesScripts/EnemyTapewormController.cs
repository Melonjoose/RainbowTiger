using System.Collections;
using UnityEngine;
using System;

public class EnemyTapewormController : MonoBehaviour
{
    #region REFERENCES
    [Header("REFERENCES")]
    [SerializeField] private GameObject sprite;
    #endregion

    #region PARAMETERS
    [Header("PARAMETERS")]
    [SerializeField] private float windupDuration;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackReach;
    #endregion

    #region COLLISION CHECKS
    [Header("COLLISION CHECKS")]
    [SerializeField] private Transform aggroCheck;
    [SerializeField] private Vector2 aggroCheckSize;

    private RaycastHit2D hits;
    [SerializeField] private LayerMask playerLayer;
    #endregion

    #region ENEMY STATES
    public enum EnemyState
    {
        Idle,
        Windup,
        Attacking,
        Reloading
    }

    [Header("ENEMY STATE")]
    public EnemyState state;

    public bool isFlipped;
    private float isFacingRightScale = 1;
    private float elapsedTime;
    #endregion

    private void OnEnable()
    {
        sprite = gameObject.transform.GetChild(0).gameObject;
        elapsedTime = 0;
        state = EnemyState.Idle;

        if (isFlipped && isFacingRightScale == 1 || !isFlipped && isFacingRightScale == -1)
        {
            isFacingRightScale *= -1;

            Vector3 scale = sprite.transform.localScale;
            scale.x *= isFacingRightScale;
            sprite.transform.localScale = scale;
            sprite.transform.localPosition *= new Vector2(isFacingRightScale, 1);
        }
    }

    private void Update()
    {
        #region TIMERS
        elapsedTime += Time.deltaTime;
        if (state == EnemyState.Idle)
        {
            elapsedTime = 0;
        }

        if (elapsedTime >= windupDuration && state == EnemyState.Windup)
        {
            elapsedTime = 0;
            state = EnemyState.Attacking;
            Attack();
        }
        if (elapsedTime >= attackDuration && state == EnemyState.Attacking)
        {
            elapsedTime = 0;
            state = EnemyState.Reloading;
            Reload();
        }
        if (elapsedTime >= attackDuration + attackCooldown && state == EnemyState.Reloading)
        {
            elapsedTime = 0;
            state = EnemyState.Idle;
        }

        #endregion

        if (Physics2D.OverlapBox(aggroCheck.position, aggroCheckSize, 0, playerLayer))
        {
            if (state == EnemyState.Idle)
            {
                state = EnemyState.Windup;
                WindupAttack();
            }
        }
    }

    #region ATTACK METHODS
    private void WindupAttack()
    {
        LeanTween.moveLocalX(sprite, -3 * isFacingRightScale, windupDuration).setEaseOutBack();
    }

    private void Attack()
    {
        LeanTween.moveLocalX(sprite, (-2 + attackReach) * isFacingRightScale, 0.5f).setEaseOutBack();
    }

    private void Reload()
    {
        LeanTween.moveLocalX(sprite, -2 * isFacingRightScale, attackCooldown);
    }
    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(aggroCheck.position, aggroCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(1.5f,1));
    }
    #endregion
}
