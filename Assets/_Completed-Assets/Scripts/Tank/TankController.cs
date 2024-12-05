using UnityEngine;
using System.Collections;
public class TankController : MonoBehaviour
{
    public bool IsWarping { get; set; } // 現在ワープ中かどうか

    private Collider tankCollider;
    public float invincibilityDuration = 2.0f; // 無敵状態の継続時間
    private bool isInvincible = false;
    private void Start()
    {
        tankCollider = GetComponent<Collider>();
    }

    public void DisableCombat()
    {
        // 無敵状態および攻撃無効
        tankCollider.enabled = false;
        // 必要に応じて砲弾や地雷の設置を無効化
    }

    public void EnableCombat()
    {
        // 無敵解除および攻撃再開
        tankCollider.enabled = true;
        // 必要に応じて砲弾や地雷の設置を再有効化
    }
    public void StartInvincibility()
    {  
        if (!isInvincible)
        {
        isInvincible = true;
        StartCoroutine(InvincibilityCoroutine());
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        float elapsedTime = 0f;
        Renderer tankRenderer = GetComponentInChildren<Renderer>();
        if (tankRenderer == null)
        {
            Debug.LogError("Tank Renderer not found! Ensure the tank model has a Renderer component.");
            yield break;
        }

        while (elapsedTime < invincibilityDuration)
        {
            // 点滅エフェクト
            tankRenderer.enabled = !tankRenderer.enabled;
            yield return new WaitForSeconds(0.1f); // 点滅間隔
            elapsedTime += 0.1f;
        }

        // 無敵状態終了
        isInvincible = false;
        tankRenderer.enabled = true;
    }
}
