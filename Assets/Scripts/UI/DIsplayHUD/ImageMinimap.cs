using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageMinimap : MonoBehaviour, IPointerClickHandler, IMinimapSubject, IPauseObserver
{
    public void Init()
    {
        ArrayPauseCommand.Use(EPauseCommand.REGIST,this);
        imageMinimap = GetComponent<Image>();
        tex2d = new Texture2D(256, 256);
        //tex2d.name = "Set Pixel";
        texW = tex2d.width;
        texH = tex2d.height;

        texRect = new Rect(0, 0, texW, texH);
        pivotVec = new Vector2(0.5f, 0.5f);

        listStructureNode = new List<PF_Node>();

        int idx = 0;

        while (idx < texW * texH)
        {
            tex2d.SetPixel(idx % texW, idx / texH, Color.black);
            ++idx;
        }

        StartCoroutine("UpdateMinimap");
    }

    public void Init(float _worldSizeX, float _worldSizeY)
    {
        worldSizeX = _worldSizeX;
        worldSizeY = _worldSizeY;
    }

    public void AddStructureNodeToMinimap(PF_Node _node)
    {
        listStructureNode.Add(_node);
    }

    public void RemoveStructureNodeFromMinimap(PF_Node _node)
    {
        listStructureNode.Remove(_node);
    }

    private IEnumerator UpdateMinimap()
    {
        while (true)
        {
            while (isPause)
                yield return null;

            UpdateTexture(ref tex2d);
            imageMinimap.sprite = Sprite.Create(tex2d, texRect, pivotVec);
            
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateTexture(ref Texture2D tex2d)
    {
        for(int i = 0; i < tempFriendlyNodeList.Count; ++i) 
            tex2d.SetPixel(tempFriendlyNodeList[i].gridX, tempFriendlyNodeList[i].gridY, Color.black);

        for(int i = 0; i < tempEnemyNodeList.Count; ++i)
            tex2d.SetPixel(tempEnemyNodeList[i].gridX, tempEnemyNodeList[i].gridY, Color.black);

        tempFriendlyNodeList.Clear();
        tempEnemyNodeList.Clear();

        foreach (PF_Node node in SelectableObjectManager.DicNodeUnderFriendlyUnit.Values)
        {
            tempFriendlyNodeList.Add(node);
            tex2d.SetPixel(node.gridX, node.gridY, Color.green);
        }

        foreach (PF_Node node in SelectableObjectManager.DicNodeUnderEnemyUnit.Values)
        {
            tempEnemyNodeList.Add(node);
            tex2d.SetPixel(node.gridX, node.gridY, Color.red);
        }

        PF_Node tempNode = null;

        for (int i = 0; i < listStructureNode.Count; ++i)
        {
            tempNode = listStructureNode[i];
            tempFriendlyNodeList.Add(tempNode);
            tex2d.SetPixel(tempNode.gridX, tempNode.gridY, Color.green);
        }

        tex2d.Apply();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // 마우스 클릭 위치를 RectTransform의 로컬 좌표계로 변환합니다.
        Vector2 localMousePosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out localMousePosition);

        // 미니맵의 크기의 반을 계산합니다.
        float halfWidth = rectTransform.rect.width * 0.5f;
        float halfHeight = rectTransform.rect.height * 0.5f;

        // 중심을 (0,0)으로 하고 최하단(-0.5, -0.5), 최우측(0.5, -0.5), 최상단(0.5, 0.5), 최좌측(-0.5, 0.5)에 대한 상대적인 좌표를 계산합니다.
        float relativeX = (localMousePosition.x + halfWidth) / rectTransform.rect.width - 0.5f;
        float relativeY = (localMousePosition.y + halfHeight) / rectTransform.rect.height - 0.5f;

        // 출력 예시 (0,0)이 좌하단, (1,1)이 우상단이라고 가정하고 출력합니다.
        //Debug.Log("마우스로 클릭한 위치 (상대적인 좌표): X=" + relativeX + ", Y=" + relativeY);
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            foreach (IMinimapObserver ob in listObserver)
                ob.GetCameraTargetPos(new Vector3(relativeX * worldSizeX, 0f, relativeY * worldSizeY));
        }
        else if (eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            foreach (IMinimapObserver ob in listObserver)
                ob.GetUnitTargetPos(new Vector3(relativeX * worldSizeX, 0f, relativeY * worldSizeY));
        }
    }

    public void RegisterPauseObserver(IMinimapObserver _observer)
    {
        listObserver.Add(_observer);
    }

    public void RemovePauseObserver(IMinimapObserver _observer)
    {
        listObserver.Remove(_observer);
    }

    public void CheckPause(bool _isPause)
    {
        isPause = _isPause;
    }

    private Image imageMinimap = null;
    private Texture2D tex2d = null;
    private Rect texRect;
    private Vector2 pivotVec;
    private List<PF_Node> listStructureNode = null;

    private int texH = 0;
    private int texW = 0;
    private List<IMinimapObserver> listObserver = new List<IMinimapObserver>();
    private List<PF_Node> tempFriendlyNodeList = new List<PF_Node>();
    private List<PF_Node> tempEnemyNodeList = new List<PF_Node>();

    private float worldSizeX = 0f; // 미니맵에 표시할 월드의 가로길이
    private float worldSizeY = 0f; // 미니맵에 표시할 월드의 세로길이

    private bool isPause = false;
    
}
