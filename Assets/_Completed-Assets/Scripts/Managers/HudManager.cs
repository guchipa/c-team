using UnityEngine;
using Complete;  // Complete名前空間を使用

namespace Complete  // HudManagerも同じ名前空間に入れる
{
    public class HudManager : MonoBehaviour
    {
        // SerializeFieldでInspectorから設定可能に
        [SerializeField]
        private PlayerStockArea m_Player1StockArea;  // プレイヤー1のストックエリア

        [SerializeField]
        private PlayerStockArea m_Player2StockArea;  // プレイヤー2のストックエリア

        [SerializeField]
        private GameManager m_GameManager;  // GameManagerへの参照

        private void Start()
        {
            if (m_GameManager != null)
            {
                // イベントの購読を開始
                m_GameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }

            // 初期状態は非表示
            SetHudVisibility(false);
        }

        private void OnDestroy()
        {
            // イベントの購読を解除
            if (m_GameManager != null)
            {
                m_GameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            // プレイ中のみHUDを表示
            bool shouldShow = (newState == GameManager.GameState.RoundPlaying);
            SetHudVisibility(shouldShow);
        }

        private void SetHudVisibility(bool visible)
        {
            // 両プレイヤーのストックエリアの表示/非表示を設定
            if (m_Player1StockArea != null)
                m_Player1StockArea.gameObject.SetActive(visible);
            
            if (m_Player2StockArea != null)
                m_Player2StockArea.gameObject.SetActive(visible);
        }
    }
}