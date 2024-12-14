using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinArea : MonoBehaviour
{
    [SerializeField] private GameObject WinImg;

    private List<GameObject> winImgs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 勝利数のUIを更新
    public void UpdateWinCount(int winCount)
    {
        Debug.Log("UpdateWinCount: " + winCount);
        foreach (GameObject icon in winImgs)
        {
            Destroy(icon);
        }
        winImgs.Clear();

        for (int i = 0; i < winCount; i++)
        {
            GameObject winImg = Instantiate(WinImg, transform);
            winImgs.Add(winImg);
        }
    }
}
