using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarpGate")) // ワームホールゲートに当たった場合
        {
            Destroy(gameObject); // 砲弾を消去
        }
    }
}
