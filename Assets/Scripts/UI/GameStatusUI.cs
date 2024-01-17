using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatusUI : MonoBehaviour
{
    [Header("현재 아이템 상태")]
    [SerializeField] TextMeshProUGUI lifeCountText;
    [SerializeField] TextMeshProUGUI bombCountText;
    [SerializeField] TextMeshProUGUI ropeCountText;

    [Header("현재 스테이지 상태")]
    [SerializeField] TextMeshProUGUI currentTimeText;
    [SerializeField] TextMeshProUGUI currentStageText;

    Player player;

    private void Start()
    {
        player = GameManager.Instance.player;

        lifeCountText.text = GameManager.Instance.PlayerHp.ToString();
        bombCountText.text = player.inven.GetItemCount(ItemType.Bomb).ToString();
        ropeCountText.text = player.inven.GetItemCount(ItemType.Rope).ToString();
    }

    private void Update()
    {
        PrintCurrentTime();
        currentStageText.text = $"{GameManager.Instance.WorldStageLevel} - {GameManager.Instance.SubStageLevel}";
    }

    public void OnChangeLifeCount()
    {
        lifeCountText.text = GameManager.Instance.PlayerHp.ToString();
    }

    public void OnChangeBombCount()
    {
        bombCountText.text = player.inven.GetItemCount(ItemType.Bomb).ToString();
    }

    public void OnChangeRopeCount()
    {
        ropeCountText.text = player.inven.GetItemCount(ItemType.Rope).ToString();
    }

    void PrintCurrentTime()
    {
        // currentTimeText.text = $"{(int) (Time.time) / 60} : {(int) Time.time}";
        currentTimeText.text = $"{((int)Time.time / 60):00}:{((int)Time.time % 60):00}";
    }
}
