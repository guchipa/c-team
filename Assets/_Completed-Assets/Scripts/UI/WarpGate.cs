using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class WarpGate : MonoBehaviour
{
    public WarpGate linkedGate; 
    public float warpDelay = 1.5f;
    public float cooldownTime = 10f;
    public float warpExitOffset = 2f;
    private HashSet<TankController> cooldownTanks = new HashSet<TankController>(); // クールダウン中の戦車リスト
    private bool isGateActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isGateActive) return; // ゲートが無効な場合は処理しない
        if (other.CompareTag("Tank")) 
        { 
            Debug.Log("touch Warmhole");
            TankController tank = other.GetComponent<TankController>();
            if (tank != null && !tank.IsWarping&& !cooldownTanks.Contains(tank)) 
            {
                StartCoroutine(WarpTank(tank));
            }
        }
        Debug.Log("Not touch Warmhole");
    }

    private IEnumerator WarpTank(TankController tank)
    {
         // このゲートを一時的に無効化
        isGateActive = false;

        // クールダウン開始
        cooldownTanks.Add(tank);

        // ワープ中の状態を設定
        tank.IsWarping = true;
        tank.DisableCombat(); // 戦闘無効化

        // 一時的に戦車のコライダーを無効化
        Collider tankCollider = tank.GetComponent<Collider>();
        if (tankCollider != null)
        {
            tankCollider.enabled = false;
        }

        // Rigidbody の取得と速度リセット
        Rigidbody tankRigidbody = tank.GetComponent<Rigidbody>();
        if (tankRigidbody != null)
        {
            tankRigidbody.velocity = Vector3.zero; // 速度をリセット
            tankRigidbody.angularVelocity = Vector3.zero; // 回転速度をリセット
        }

        // ワープにかかる時間待機
        yield return new WaitForSeconds(warpDelay);

        // ワープ先の位置に移動
        Vector3 targetPosition = linkedGate.transform.position + linkedGate.transform.forward * warpExitOffset;

        // レイキャストで地形の高さを調整
        if (Physics.Raycast(targetPosition + Vector3.up, Vector3.down, out RaycastHit hit, 10f))
        {
            targetPosition.y = hit.point.y; // 地面の高さを設定
        }
        tank.transform.position = targetPosition;

        // 無敵状態を開始
        tank.StartInvincibility();
        // ワープ先のゲートを一時的に無効化
        if (linkedGate != null)
        {
            linkedGate.DeactivateGateTemporarily(cooldownTime);
        }

        // ワープ完了
        if (tankCollider != null) tankCollider.enabled = true; // コライダーを再度有効化
        tank.EnableCombat(); // 戦闘再開
        tank.IsWarping = false;
        
        // クールダウン期間待機
        yield return new WaitForSeconds(cooldownTime);

        // クールダウン解除
        cooldownTanks.Remove(tank);

        // このゲートを再度有効化
        isGateActive = true;
    
        
    }
    public void DeactivateGateTemporarily(float duration)
    {
        StartCoroutine(DeactivateGateCoroutine(duration));
    }

    private IEnumerator DeactivateGateCoroutine(float duration)
    {
        isGateActive = false;
        yield return new WaitForSeconds(duration);
        isGateActive = true;
    }
}

