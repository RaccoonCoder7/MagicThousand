using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TouchMgr : MonoBehaviour
{
    private Transform tr;
    private LineRenderer line;

    private Ray ray;
    private RaycastHit hit;
    private Camera cam;
    private int layerBT;

    private string[] texts;
    private List<string> dialogueList;
    private Text uiText;
    private State nowState;
    private int nextDialogue = 0;
    private int SkipNextCount = 0;

    private GameObject dialogObj;
    public GameObject skipObj;
    private GameObject moon;
    private GameObject sun;
    private GameObject fire;
    private GameObject tree;
    private GameObject soil;
    private GameObject brushPoint;

    private Animator ani;

    enum State
    {
        Idle,
        Playing,
        Next,
    }

    void Start()
    {
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        layerBT = 1 << LayerMask.NameToLayer("DIALOGUE");
        cam = Camera.main;
        brushPoint = GameObject.Find("brushPoint");

        dialogueList = new List<string>();

        uiText = GameObject.Find("DialogueText").GetComponent<Text>();
        dialogObj = GameObject.Find("Dialog");
        skipObj = GameObject.Find("Skip");
        moon = GameObject.Find("moon");
        sun = GameObject.Find("sun");
        fire = GameObject.Find("fire");
        tree = GameObject.Find("tree");
        soil = GameObject.Find("soil");

        TextAsset data = Resources.Load("DialogueText", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        ani = GameObject.Find("NPC").GetComponent<Animator>();

        string dialogueLine;
        dialogueLine = sr.ReadLine();
        while (dialogueLine != null)
        {
            dialogueList.Add(dialogueLine);
            dialogueLine = sr.ReadLine();
        }

        CreateDialogueText(dialogueList[SkipNextCount]);

        skipObj.SetActive(false);
        moon.SetActive(false);
        sun.SetActive(false);
        fire.SetActive(false);
        tree.SetActive(false);
        soil.SetActive(false);

        nowState = State.Next;
    }

    void CreateDialogueText(string dialogueText)
    {
        texts = dialogueText.Split('E');
    }

    void Update()
    {
        ray = new Ray(tr.position, tr.forward);
        if (Physics.Raycast(ray, out hit, 16.0f))
        {
            float dist = hit.distance;
            line.SetPosition(1, new Vector3(0, 0, dist));
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBT))
            {
                StartCoroutine("Run");
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("SKIP")))
            {
                EndDrawing();
            }
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("LINE")))
            {
                if (hit.collider.tag == "SECOND")
                {
                    hit.collider.transform.parent.GetComponent<LineSlider>().SetColors(hit.point);
                    return;
                }
                hit.collider.GetComponent<LineSlider>().SetColors(hit.point);
            }

        }
    }

    IEnumerator Run()
    {
        if (nextDialogue < texts.Length && nowState == State.Next)
        {
            yield return PlayLine(texts[nextDialogue]);
            nextDialogue++;
        }
        else if (nextDialogue == texts.Length)
        {
            dialogObj.SetActive(false);
            skipObj.SetActive(true);
            nextDialogue = 0;

            switch (SkipNextCount)
            {
                case 0:
                    moon.SetActive(true);
                    break;
                case 1:
                    sun.SetActive(true);
                    break;
                case 2:
                    fire.SetActive(true);
                    break;
                case 3:
                    tree.SetActive(true);
                    break;
                case 4:
                    soil.SetActive(true);
                    break;
            }

            if (SkipNextCount == dialogueList.Count - 1)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    IEnumerator PlayLine(string text)
    {
        if (ani.GetBool("CHAT"))
        {
            ani.SetBool("CHAT", false);
        }
        else
        {
            ani.SetBool("CHAT", true);
        }
        nowState = State.Playing;
        for (int i = 0; i < text.Length + 1; i += 1)
        {
            yield return new WaitForSeconds(0.02f);
            uiText.text = text.Substring(0, i);
        }

        yield return new WaitForSeconds(0.5f);
        nowState = State.Next;
    }

    public void EndDrawing()
    {
        SkipNextCount++;
        CreateDialogueText(dialogueList[SkipNextCount]);
        dialogObj.SetActive(true);
        skipObj.SetActive(false);
        moon.SetActive(false);
        sun.SetActive(false);
        fire.SetActive(false);
        tree.SetActive(false);
        soil.SetActive(false);
        brushPoint.transform.position = new Vector3(0, -3, 0);
        StartCoroutine("Run");
    }
}