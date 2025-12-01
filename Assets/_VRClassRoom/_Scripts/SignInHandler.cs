
using FishNet.Managing;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInHandler : MonoBehaviour
{
    public static SignInHandler Instance;

    public enum PlayerType
    {
        None,
        Instructor,
        Trainee
    }

    public PlayerType selectedPlayer;

    [SerializeField] Button btn_Instructor;
    [SerializeField] Button btn_Trainee;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        btn_Instructor.onClick.AddListener(() =>
        {
            selectedPlayer = PlayerType.Instructor;
            SceneManager.LoadScene("ClassroomScene");
        });

        btn_Trainee.onClick.AddListener(() =>
        {
            selectedPlayer = PlayerType.Trainee;
            SceneManager.LoadScene("ClassroomScene");
        });



    }
}
