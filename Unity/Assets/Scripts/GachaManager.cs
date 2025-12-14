using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text resultText;
    public Image itemImage;

    [Header("Server")]
    private string baseUrl = "http://localhost:4000";

    [Header("Item Sprite Mapping")]
    public ItemSpriteData[] itemSprites;

    // -------------------------------
    // 데이터 구조
    // -------------------------------

    [System.Serializable]
    public class GachaRequest
    {
        public int userId;
        public GachaRequest(int id) { userId = id; }
    }

    [System.Serializable]
    public class GachaItem
    {
        public string name;
        public string rank;
    }

    [System.Serializable]
    public class GachaResponse
    {
        public bool success;
        public GachaItem data;
    }

    [System.Serializable]
    public class ItemSpriteData
    {
        public string itemName;   // 서버에서 내려오는 name
        public Sprite sprite;     // 연결할 이미지
    }

    // -------------------------------
    // 버튼 이벤트
    // -------------------------------

    public void OnClickGacha()
    {
        if (UserData.Instance.userId == 0)
        {
            resultText.text = "먼저 로그인 해 주세요!";
            return;
        }

        StartCoroutine(GachaRoutine());
    }

    // -------------------------------
    // 가챠 요청
    // -------------------------------

    IEnumerator GachaRoutine()
    {
        string url = baseUrl + "/api/gacha/pull";

        GachaRequest body = new GachaRequest(UserData.Instance.userId);
        string json = JsonUtility.ToJson(body);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            resultText.text = "서버 오류: " + req.error;
            yield break;
        }

        GachaResponse res = JsonUtility.FromJson<GachaResponse>(req.downloadHandler.text);

        if (res.success && res.data != null)
        {
            // 텍스트 표시
            resultText.text = $"{res.data.rank} 등급 '{res.data.name}' 획득!";

            // 이미지 표시
            Sprite sprite = GetItemSprite(res.data.name);

            if (sprite != null)
            {
                itemImage.sprite = sprite;
                itemImage.enabled = true;
            }
            else
            {
                itemImage.enabled = false;
            }
        }
        else
        {
            resultText.text = "가챠 실패";
            itemImage.enabled = false;
        }
    }

    // -------------------------------
    // 아이템 이름 → Sprite 찾기
    // -------------------------------

    Sprite GetItemSprite(string itemName)
    {
        foreach (var item in itemSprites)
        {
            if (item.itemName == itemName)
                return item.sprite;
        }
        return null;
    }
}
