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
                // GameManagerのイベント購読
                m_GameManager.OnGameStateChanged += HandleGameStateChanged;

                // 各タンクのイベント購読
                foreach (var tank in m_GameManager.m_Tanks)
                {
                    tank.OnWeaponStockChanged += HandleWeaponStockChanged;
                }
            }
            SetHudVisibility(false);
        }

        private void OnDestroy()
        {
            if (m_GameManager != null)
            {
                m_GameManager.OnGameStateChanged -= HandleGameStateChanged;

                // イベント購読の解除
                foreach (var tank in m_GameManager.m_Tanks)
                {
                    tank.OnWeaponStockChanged -= HandleWeaponStockChanged;
                }
            }
        }

        private void HandleWeaponStockChanged(int playerNumber, int currentStock, string weaponName)
        {
            // プレイヤー番号に応じて対応するUIを更新
            // 砲弾のUIを更新
            if (weaponName == WeaponNames.shell)
            {
                if (playerNumber == 1 && m_Player1StockArea != null)
                {
                    m_Player1StockArea.UpdatePlayerStockArea(currentStock);
                }
                else if (playerNumber == 2 && m_Player2StockArea != null)
                {
                    m_Player2StockArea.UpdatePlayerStockArea(currentStock);
                }
            }

            if (weaponName == WeaponNames.mine)
            {

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