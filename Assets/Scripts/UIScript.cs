using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hpText;

    private DataScript dataScript;

    private void Awake()
    {
        dataScript = GetComponent<DataScript>();
    }

    private void Start()
    {
        UIRefresh();
    }

    public void UIRefresh()
    {
        hpText.text = dataScript.hp.ToString();
        scoreText.text = dataScript.score.ToString();
    }
}
