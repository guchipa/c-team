using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStockArea : MonoBehaviour
{
    // 弾数 HUD の各エリアを格納する変数
    public GameObject shellStock10Area; // 10発単位のHUDエリア
    public GameObject shellStock1Area;  // 1発単位のHUDエリア
    public GameObject mineStockArea;    // 地雷HUDエリア

    // 各 stockArea が持つべき画像を格納するリスト
    private List<Image> shell10List = new List<Image>(); // 10発単位の画像リスト
    private List<Image> shell1List = new List<Image>();  // 1発単位の画像リスト
    private List<Image> mineList = new List<Image>();    // 地雷用画像リスト

    private void Start()
    {
        // 各 stockArea の子オブジェクトから Image を取得してリストに格納
        if (shellStock10Area != null)
        {
            shell10List.AddRange(shellStock10Area.GetComponentsInChildren<Image>());
        }

        if (shellStock1Area != null)
        {
            shell1List.AddRange(shellStock1Area.GetComponentsInChildren<Image>());
        }

        if (mineStockArea != null)
        {
            mineList.AddRange(mineStockArea.GetComponentsInChildren<Image>());
        }
    }

    public void UpdatePlayerStockArea(int stockCount, string weaponName)
    {
        if (weaponName == "shell")
        {
            UpdateShellStockArea(stockCount);
        }
        else if (weaponName == "mine")
        {
            UpdateMineStockArea(stockCount);
        }
    }

    private void UpdateShellStockArea(int stockCount)
    {
        // 10発単位の画像更新
        int tens = stockCount / 10; // 10発単位のカウント
        for (int i = 0; i < shell10List.Count; i++)
        {
            shell10List[i].enabled = i <= tens; // tens 以下の要素を有効化、それ以外は無効化
        }

        // 1発単位の画像更新
        int ones = stockCount % 10; // 残りの1発単位のカウント
        for (int i = 0; i < shell1List.Count; i++)
        {
            shell1List[i].enabled = i <= ones; // ones 以下の要素を有効化、それ以外は無効化
        }
    }

    private void UpdateMineStockArea(int stockCount)
    {
        // MineStock 用の画像を更新
        for (int i = 0; i < mineList.Count; i++)
        {
            mineList[i].enabled = i <= stockCount; // stockCount 以下の要素を有効化、それ以外は無効化
        }
    }
}
