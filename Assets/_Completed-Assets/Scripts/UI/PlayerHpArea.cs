using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpArea : MonoBehaviour
{
    [SerializeField] private Slider m_hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHpArea(float hp)
    {
        Debug.Log("UpdateHpArea: " + hp);
        m_hpSlider.value = hp / 100;
    }
}
