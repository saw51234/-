using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData Instance;

    public int userId;
    public string nickname;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
