using System.Collections;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public bool IsWarping { get; private set; }
    private Renderer tankRenderer;
    private Collider tankCollider;
    private float originalFireCooldown;

    private void Start()
    {
        tankRenderer = GetComponent<Renderer>();
        tankCollider = GetComponent<Collider>();
    }

    public void StartWarping(float duration)
    {
        IsWarping = true;
        StartCoroutine(WarpEffect(duration));
        DisableCombat();
    }

    private IEnumerator WarpEffect(float duration)
    {
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < duration)
        {
            isVisible = !isVisible;
            tankRenderer.enabled = isVisible; // 点滅効果
            elapsed += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }

        tankRenderer.enabled = true; // 点滅終了
    }

    public void EndWarping()
    {
        IsWarping = false;
        EnableCombat();
    }

    private void DisableCombat()
    {
        // 戦車の攻撃を無効化するロジック
        tankCollider.enabled = false; // 無敵状態
        // 砲弾や地雷の設置を無効化 (必要に応じてアタッチするコンポーネントに応じて処理)
    }

    private void EnableCombat()
    {
        tankCollider.enabled = true; // 無敵解除
        // 攻撃を再度有効化
    }
}
