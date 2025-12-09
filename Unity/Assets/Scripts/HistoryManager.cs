using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour
{
    public GameObject panelHistory;
    public Transform contentParent;     // ScrollView -> Viewport -> Content
    public GameObject historyItemPrefab;

    private string baseUrl = "http://localhost:4000";

    [System.Serializable]
    public class HistoryItemData
    {
        public string item_name;
        public string rank_grade;
        public string created_at;
    }

    [System.Serializable]
    public class HistoryResponse
    {
        public bool success;
        public HistoryItemData[] data;
    }

    public void OpenHistory()
    {
        panelHistory.SetActive(true);
        StartCoroutine(LoadHistory());
    }

    public void CloseHistory()
    {
        panelHistory.SetActive(false);

        // 내용 삭제
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator LoadHistory()
    {
        string url = $"{baseUrl}/api/gacha/history/{UserData.Instance.userId}";

        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("History Load Error: " + req.error);
            yield break;
        }

        HistoryResponse res = JsonUtility.FromJson<HistoryResponse>(req.downloadHandler.text);

        if (!res.success)
        {
            Debug.Log("History Load Failed");
            yield break;
        }

        foreach (var item in res.data)
        {
            GameObject newItem = Instantiate(historyItemPrefab, contentParent);
            TMP_Text txt = newItem.GetComponent<TMP_Text>();

            txt.text = $"{item.rank_grade} 등급 {item.item_name} | {item.created_at}";
        }
    }
}
