using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class CartridgeData
{
    public GameObject m_CartridgePrefab;        // カートリッジのプレハブ
    public float m_CartridgeSpawnArea = 100f;   // カートリッジの生成範囲（正方形の一辺）
    public float m_SpawnHeight = 0.5f;          // 生成する高さ
    public float m_CartridgeSpawnInterval = 5f; // カートリッジの生成間隔
}
