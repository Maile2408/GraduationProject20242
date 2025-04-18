using TMPro;
using UnityEngine;

public class LeaderboardSlot : MonoBehaviour, IPoolable
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text cityNameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetInfo(int rank, string userName, string cityName, int score)
    {
        rankText.text = rank.ToString();
        userNameText.text = string.IsNullOrEmpty(userName) ? "No Name" : userName;
        cityNameText.text = string.IsNullOrEmpty(cityName) ? "No Name" : cityName;
        scoreText.text = score.ToString();
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        rankText.text = "";
        userNameText.text = "";
        cityNameText.text = "";
        scoreText.text = "";
    }
}
