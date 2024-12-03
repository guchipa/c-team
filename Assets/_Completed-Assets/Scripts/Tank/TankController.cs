using UnityEngine;

public class TankController : MonoBehaviour
{
    public bool IsWarping { get; set; } // 現在ワープ中かどうか

    private Collider tankCollider;

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
}
