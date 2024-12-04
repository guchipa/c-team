using UnityEngine;
using System.Collections;

namespace Complete 
{
    public class CartridgeSpawner : MonoBehaviour
    {
        [SerializeField]
        private CartridgeData m_ShellCartridgeData;   // 砲弾カートリッジデータ
        
        [SerializeField]
        private CartridgeData m_MineCartridgeData;   // 地雷カートリッジデータ

        private GameManager m_GameManager;
        private Coroutine m_SpawnCoroutine;

        private void Start()
        {
            m_GameManager = FindObjectOfType<GameManager>();
            
            if (m_GameManager != null)
            {
                // イベントの購読を開始
                m_GameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            CartridgeData shellCartridgeData = m_ShellCartridgeData;
            CartridgeData mineCartridgeData = m_MineCartridgeData;

            if (newState == GameManager.GameState.RoundPlaying)
            {
                // プレイ中ならスポーン開始
                if (m_SpawnCoroutine == null)
                {
                    m_SpawnCoroutine = StartCoroutine(SpawnRoutine(shellCartridgeData));
                    m_SpawnCoroutine = StartCoroutine(SpawnRoutine(mineCartridgeData));
                }
            }
            else
            {
                // それ以外なら停止
                if (m_SpawnCoroutine != null)
                {
                    StopCoroutine(m_SpawnCoroutine);
                    m_SpawnCoroutine = null;
                }
            }
        }

        private void OnDestroy()
        {
            // イベントの購読を解除
            if (m_GameManager != null)
            {
                m_GameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        // カートリッジ生成メソッド
        private void SpawnCartridge(CartridgeData data)
        {
            // 正方形エリア内でランダムな位置を計算
            float halfSize = data.m_CartridgeSpawnArea * 0.5f;
            float xPos = Random.Range(-halfSize, halfSize);
            float zPos = Random.Range(-halfSize, halfSize);
            
            // 生成位置を設定
            Vector3 spawnPosition = new Vector3(xPos, data.m_SpawnHeight, zPos);

            // カートリッジを生成
            Instantiate(data.m_CartridgePrefab, spawnPosition, Quaternion.identity);
        }

        // スポーン用コルーチン
        private IEnumerator SpawnRoutine(CartridgeData data)
        {
            while (true) 
            {
                SpawnCartridge(data);  // カートリッジ生成

                // 次の生成まで待機
                yield return new WaitForSeconds(data.m_CartridgeSpawnInterval);
            }
        }

        // デバッグ用：生成範囲の可視化
        private void OnDrawGizmosSelected()
        {
            // 正方形の範囲を表示
            Gizmos.color = Color.yellow;
            float halfSize = 100f * 0.5f;
            Vector3 center = transform.position;
            
            Vector3 p1 = center + new Vector3(-halfSize, 0, -halfSize);
            Vector3 p2 = center + new Vector3(-halfSize, 0, halfSize);
            Vector3 p3 = center + new Vector3(halfSize, 0, halfSize);
            Vector3 p4 = center + new Vector3(halfSize, 0, -halfSize);
            
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }
    }
}