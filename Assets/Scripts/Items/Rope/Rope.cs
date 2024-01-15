using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Rope : MonoBehaviour
{
    [SerializeField] float targetTopHeight;
    float targetBottomHeight;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] BoxCollider2D ropeCol;

    [Header("이미지")]
    [SerializeField] SpriteRenderer ropeTop;
    [SerializeField] SpriteRenderer ropeMiddle;
    [SerializeField] SpriteRenderer ropeBottom;

    [SerializeField] Sprite[] replacedImg;
    [Header("첫 로프 설치 보간")]
    [SerializeField] float upperExpandDuration;
    [Header("중간로프 설치 보간")]
    [SerializeField] float underExpandDuration;

    float initColOffsetY;
    float initColSizeY;

    float currentSizeHeight;
    float currentPosHeight;

    float currentWaitTime = 0;

    

    Vector2 startPos;
    Vector2 placedPos;

    bool isPlaced;

    private void Awake()
    {
        initColOffsetY = ropeCol.offset.y;
        initColSizeY = ropeCol.size.y;
    }

    private void OnEnable()
    {
        targetBottomHeight = targetTopHeight;

        startPos = transform.position;
        CheckRoof();
    }

    private void Update()
    {
        if (isPlaced == false)
        {
            RopeUp();
        }
        else
        {
            Debug.DrawRay(startPos, Vector2.down * targetTopHeight, Color.red);
            RopeDown();
        }

    }

    private void OnDisable()
    {
        ropeTop.sprite = replacedImg[0];
        targetBottomHeight = 0;
        ropeMiddle.size = new Vector2(ropeMiddle.size.x, 0);
        isPlaced = false;
        ropeTop.transform.localPosition = ropeMiddle.transform.localPosition = ropeBottom.transform.localPosition = Vector3.zero;

        ReinitializeVariables();
    }

    void CheckRoof()
    {
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.up, targetTopHeight, targetLayer);

        if (hit.collider == null)
        {
            placedPos = startPos + new Vector2(0, targetTopHeight);
        }
        else
        {
            TilemapCollider2D tilemapCollider = hit.collider.GetComponent<TilemapCollider2D>();

            if (tilemapCollider != null)
            {
                placedPos = tilemapCollider.transform.TransformPoint(hit.point); // 충돌 위치에 대한 좌표를 월드좌표로 반환 하는 함수
            }
            else
            {
                placedPos = hit.point;
            }
        }
    }
    void RopeUp()
    {
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime <= upperExpandDuration)
        {
            float lerpU = currentWaitTime / upperExpandDuration;

            float newTopPosHeight = Mathf.Lerp(startPos.y, placedPos.y, lerpU);

            transform.position = new Vector2(transform.position.x, newTopPosHeight);
        }

        else
        {
            transform.position = new Vector2(transform.position.x, placedPos.y);

            ropeTop.sprite = replacedImg[1];
            isPlaced = true;

            ReinitializeVariables();
            CheckBottom();
        }
    }

    void ReinitializeVariables()
    {
        currentWaitTime = 0;
        currentSizeHeight = ropeMiddle.size.y;
        currentPosHeight = ropeMiddle.transform.position.y;
        startPos = transform.position;
    }


    void CheckBottom()
    {
        RaycastHit2D hit = Physics2D.Raycast(startPos - new Vector2(0, 0.5f), Vector2.down, targetTopHeight, targetLayer);

        if (hit.collider == null)
        {
            placedPos = startPos - new Vector2(0, targetTopHeight);
            targetBottomHeight = targetTopHeight;
        }
        else
        {
            TilemapCollider2D tilemapCollider = hit.collider.GetComponent<TilemapCollider2D>();

            if (tilemapCollider != null)
            {
                placedPos = tilemapCollider.transform.TransformPoint(hit.point); // 충돌 위치에 대한 좌표를 월드좌표로 반환 하는 함수
                targetBottomHeight = startPos.y - placedPos.y;
            }
            else
            {
                placedPos = hit.point;
                targetBottomHeight = startPos.y - placedPos.y;
            }
        }

    }


    void RopeDown()
    {
        currentWaitTime += Time.deltaTime;
        float targetPosHeight = currentPosHeight - (targetBottomHeight / 2) + 0.5f;

        if (currentWaitTime <= underExpandDuration)
        {
            float lerpD = currentWaitTime / underExpandDuration;


            float newSizeHeight = Mathf.Lerp(currentSizeHeight, targetBottomHeight, lerpD);
            float newMiddlePosHeight = Mathf.Lerp(currentPosHeight, targetPosHeight, lerpD); // + 0.5f
            float newBottomPosHeight = Mathf.Lerp(currentPosHeight, targetPosHeight - (targetBottomHeight / 2) - 0.3f, lerpD);

            float newColliderSizeY = Mathf.Lerp(initColSizeY, initColSizeY + targetBottomHeight, lerpD);
            float newCollideroffsetY = Mathf.Lerp(initColOffsetY, initColOffsetY - targetBottomHeight / 2, lerpD);

            ropeCol.size = new Vector2(ropeCol.size.x, newColliderSizeY); // 콜라이더 크기를 보간된 값으로 설정
            ropeCol.offset = new Vector2(ropeCol.offset.x, newCollideroffsetY + 0.3f);

            ropeMiddle.size = new Vector2(ropeMiddle.size.x, newSizeHeight); // ropeSprite - tiled 상태에서 size를 늘리면 상하로 늘어나므로 늘어나는 속도에 맞게 sprite를 아래로 조절
            ropeMiddle.transform.position = new Vector2(ropeMiddle.transform.position.x, newMiddlePosHeight);
            ropeBottom.transform.position = new Vector2(ropeMiddle.transform.position.x, newBottomPosHeight);
        }
        else
        {
            ropeMiddle.size = new Vector2(ropeMiddle.size.x, targetBottomHeight);
            ropeMiddle.transform.position = new Vector2(ropeMiddle.transform.position.x, targetPosHeight);
            ropeBottom.transform.position = new Vector2(ropeMiddle.transform.position.x, targetPosHeight - (targetBottomHeight / 2) - 0.3f);

            ropeCol.size = new Vector2(ropeCol.size.x, initColSizeY + targetBottomHeight);
            ropeCol.offset = new Vector2(ropeCol.offset.x, initColOffsetY - targetBottomHeight / 2 + 0.3f);
        }
    }

    void SetCollider(float offsetY, float sizeX, float sizeY)
    {
        ropeCol.size = new Vector2(sizeX, sizeY);
        ropeCol.offset = new Vector2(ropeCol.offset.x, offsetY);
    }
}