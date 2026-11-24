using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// The only script that loads a scene. Loads in the background behind a bar (Lecture 10).
// Build order: MainMenu 0, Level01-05 1-5, Credits 6. After Credits it wraps back to the menu.
public class LevelLoader : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider progressBar;
    [SerializeField] TMP_Text percent;
    [SerializeField] int lastInPlayOrder = 6;   // the credits
    bool loading;

    public void LoadNext()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next > lastInPlayOrder) next = 0;
        Load(next);
    }

    public void Reload() => Load(SceneManager.GetActiveScene().buildIndex);
    public void LoadMenu() => Load(0);

    public void Load(int buildIndex)
    {
        if (loading) return;
        loading = true;
        Time.timeScale = 1f;              // survives a load, so put it back first
        StartCoroutine(LoadAsync(buildIndex));
    }

    IEnumerator LoadAsync(int buildIndex)
    {
        loadingPanel.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex);
        op.allowSceneActivation = false;   // hold the switch until the bar has reached the end
        while (op.progress < 0.9f)
        {
            Show(Mathf.Clamp01(op.progress / 0.9f));
            yield return null;
        }
        Show(1f);
        yield return new WaitForSecondsRealtime(0.25f);
        op.allowSceneActivation = true;
    }

    void Show(float value)
    {
        progressBar.value = value;
        percent.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
