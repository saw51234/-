using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nicknameInput;
    public TMP_Text resultText;

    //  서버 주소
    private string baseUrl = "http://localhost:4000";

    [System.Serializable]
    public class LoginRequest
    {
        public string nickname;
        public LoginRequest(string nickname) { this.nickname = nickname; }
    }

    [System.Serializable]
    public class LoginUserData
    {
        public int userId;
        public string nickname;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public bool success;
        public string message;
        public LoginUserData data;
    }

    public void OnClickLogin()
    {
        string nick = nicknameInput.text;

        if (string.IsNullOrEmpty(nick))
        {
            resultText.text = "닉네임을 입력하세요!";
            return;
        }

        StartCoroutine(LoginRoutine(nick));
    }

    IEnumerator LoginRoutine(string nickname)
    {
        string url = baseUrl + "/api/auth/login";

        LoginRequest bodyData = new LoginRequest(nickname);
        string json = JsonUtility.ToJson(bodyData);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            resultText.text = "서버 연결 실패: " + req.error;
            yield break;
        }

        // 서버에서 온 JSON 파싱
        LoginResponse res = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);

        if (res.success)
        {
            UserData.Instance.userId = res.data.userId;
            UserData.Instance.nickname = res.data.nickname;

            resultText.text = $"{res.data.nickname} 로그인 성공! (ID: {res.data.userId})";
        }
        else
        {
            resultText.text = "로그인 실패: " + res.message;
        }
    }
}
