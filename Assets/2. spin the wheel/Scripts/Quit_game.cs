using UnityEngine;
using UnityEngine.UI;
public class Quit_game : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(quitgame);
    }

    // Update is called once per frame
   private void quitgame()
    {
        Application.Quit();
    }
}
