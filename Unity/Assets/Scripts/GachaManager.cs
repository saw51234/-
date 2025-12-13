using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GachaManager : MonoBehaviour
{
    public TMP_Text resultText;

    private string baseUrl = "http://localhost:4000";

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

    public void OnClickGacha()
    {
        if (UserData.Instance.userId == 0)
        {
            resultText.text = "먼저 로그인 해 주세요!";
            return;
        }

        StartCoroutine(GachaRoutine());
    }

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
            resultText.text = $"{res.data.rank} 등급 '{res.data.name}' 획득!";
        }
        else
        {
            resultText.text = "가챠 실패";
        }
    }
}
