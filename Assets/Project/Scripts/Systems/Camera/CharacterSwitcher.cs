using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Character Models")]
    [SerializeField] private UnityEngine.GameObject modelKing;
    [SerializeField] private UnityEngine.GameObject modelQueen;

    [Header("Avatars")]
    [SerializeField] private Avatar kingAvatar;
    [SerializeField] private Avatar queenAvatar;

    private void Start()
    {
        string character = PlayFabProfileManager.Instance.CharacterType;

        bool isKing = character == "King";

        modelKing.SetActive(isKing);
        modelQueen.SetActive(!isKing);

        animator.avatar = isKing ? kingAvatar : queenAvatar;
    }
}
