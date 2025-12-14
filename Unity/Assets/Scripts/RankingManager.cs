using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class RankingManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panelRanking;
    public Transform contentParent;
    public GameObject rankingItemPrefab;

    private string baseUrl = "http://localhost:4000";

    [System.Serializable]
    public class RankingItemData
    {
        public string nickname;
        public int total_pulls;
        public string highest_rank;
    }

    [System.Serializable]
    public class RankingResponse
    {
        public bool success;
        public RankingItemData[] data;
    }

    // ∑©≈∑ √¢ ø≠±‚
    public void OpenRanking()
    {
        panelRanking.SetActive(true);
        StartCoroutine(LoadRanking());
    }

    // ∑©≈∑ √¢ ¥›±‚
    public void CloseRanking()
    {
        panelRanking.SetActive(false);

        // ±‚¡∏ ∏ÆΩ∫∆Æ ¡§∏Æ
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator LoadRanking()
    {
        string url = $"{baseUrl}/api/gacha/ranking";

        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Ranking Load Error: " + req.error);
            yield break;
        }

        RankingResponse res = JsonUtility.FromJson<RankingResponse>(req.downloadHandler.text);

        if (res == null || !res.success || res.data == null)
        {
            Debug.Log("Ranking load failed or empty");
            yield break;
        }

        int rank = 1;

        foreach (var item in res.data)
        {
            GameObject row = Instantiate(rankingItemPrefab, contentParent);
            TMP_Text txt = row.GetComponent<TMP_Text>();

            txt.text = $"{rank}¿ß | {item.nickname} | √÷∞ÌµÓ±ﬁ: {item.highest_rank} | √— ªÃ±‚: {item.total_pulls}";
            rank++;
        }
    }
}
