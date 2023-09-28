using TMPro;
using UnityEngine;

public class TutorialBoard : MonoBehaviour
{

    public GameObject mobileForwardButton;
    public GameObject mobileBackButton;

    public GameObject pcForwardButton;
    public GameObject pcBackButton;

    public GameObject textIndicator;
    public TextMeshProUGUI indicatorText;
    public GameObject mobileTutorialButton;

    public GameObject page1Mobile;
    public GameObject page2Mobile;
    public GameObject page1Pc;
    public GameObject page2Pc;
    public GameObject bookCanvas;
    public bool canInspect = false;

    // Start is called before the first frame update
    void Start()
    {
        

        if (Application.isMobilePlatform)
        {
            pcForwardButton.SetActive(false);
            pcBackButton.SetActive(false);
            indicatorText.text = "[Touch] - Show Tutorial";
            mobileTutorialButton.SetActive(true);
            bookCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2 (1955.51f, 1160.80f);

        } else
        {
            mobileBackButton.SetActive(false);
            mobileForwardButton.SetActive(false);
            indicatorText.text = "[F] - Show Tutorial";
            mobileTutorialButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canInspect && Input.GetKeyDown(KeyCode.F) && !Application.isMobilePlatform)
        {
            bookCanvas.SetActive(true);
            page1Pc.SetActive(true);
            page2Pc.SetActive(false);
            page1Mobile.SetActive(false);
            page2Mobile.SetActive(false);
        }
    }

    public void PageFlipForward()
    {
        page1Pc.SetActive(false);
        page2Pc.SetActive(true);
        page1Mobile.SetActive(false);
        page2Mobile.SetActive(false);
    }


    public void PageFlipBackward()
    {
        page1Pc.SetActive(true);
        page2Pc.SetActive(false);
        page1Mobile.SetActive(false);
        page2Mobile.SetActive(false);
    }

    public void PageFlipMobileForward()
    {
        page1Pc.SetActive(false);
        page2Pc.SetActive(false);
        page1Mobile.SetActive(false);
        page2Mobile.SetActive(true);
    }


    public void PageFlipMobileBackward()
    {
        page1Pc.SetActive(false);
        page2Pc.SetActive(false);
        page1Mobile.SetActive(true);
        page2Mobile.SetActive(false);
    }

    public void MobileTutorialShow()
    {
        if (canInspect)
        {
            bookCanvas.SetActive(true);
            page1Pc.SetActive(false);
            page2Pc.SetActive(false);
            page1Mobile.SetActive(true);
            page2Mobile.SetActive(false);
        } 
    }

    public void TutorialClose()
    {
        bookCanvas.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInspect = true;
            textIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInspect = false;
            textIndicator.SetActive(false);
            bookCanvas.SetActive(false);
        }
    }
}
