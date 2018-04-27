using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class UsingMouse : VRTK_InteractableObject
{
    GameObject PC;
    GameObject NameButton1;
    GameObject ReportButton1;
    GameObject CloseButton1;
    GameObject CheckBox1;

    Transform Cursor;
    Transform UI;

    DetectCheck DetectCursor;

    public Dictionary<string, bool> ReportData;

    [Header("Custom Variables", order = 1)]
    public float CursorTransformFactor = 1.85f;
    [SerializeField]
    private float ScreenWidth = 0.43f;
    [SerializeField]
    private float ScreenHeight = 0.43f;

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        
        base.StartUsing(usingObject);
        Click();

    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject)
    {
       
        base.StopUsing(previousUsingObject);
    }

        // Use this for initialization
    void Start () {
        PC = GameObject.Find("PC");
        UI = PC.transform.Find("Screen/UI");
        Cursor = PC.transform.Find("Screen/UI/Cursor");
        DetectCursor = Cursor.GetComponent<DetectCheck>();
        Cursor.transform.localPosition = new Vector3(Cursor.transform.localPosition.x, 0,0); // Place the cursor in the center of screen. 
        ReportData = new Dictionary<string, bool>();
    }
	
	void Update () {

        // Move the cursor with the mouse.
        float newX = Cursor.localPosition.x;
        float newY = -transform.localPosition.x * CursorTransformFactor;
        newY = Mathf.Clamp(newY, -ScreenHeight, ScreenHeight);
        float newZ = transform.localPosition.z * CursorTransformFactor;
        newZ = Mathf.Clamp(newZ, -ScreenWidth, ScreenWidth);
        Cursor.localPosition = new Vector3(newX, newY, newZ);
    }


    private void Click() {

        if (DetectCursor.TriggerStay) {

            Collider col = DetectCursor.CurrentCollider;

            if (col.gameObject.CompareTag("Checkbox"))
            {
                GameObject check = col.gameObject.transform.Find("Check").gameObject;
                if (!check.activeInHierarchy)
                {
                    check.SetActive(true);
                }
                else
                {
                    check.SetActive(false);
                }
            }

            else if (UI.transform.Find("Watchlist").gameObject.activeInHierarchy) {

                if (col.name == "NameButton")
                {
                    UI.transform.Find("Watchlist").gameObject.SetActive(false);
                    if (col.gameObject.transform.Find("name").GetComponent<TextMesh>().text == "KIAN SANTOS")
                    {
                        UI.transform.Find("VictimDetails").gameObject.SetActive(true);
                    }
                    else {
                        UI.transform.Find("GoBack").gameObject.SetActive(true);
                    }
                }

            }

            else if (UI.transform.Find("VictimDetails").gameObject.activeInHierarchy)
            {

                if (col.name == "NextButton")
                {
                    UI.transform.Find("VictimDetails").gameObject.SetActive(false);
                    UI.transform.Find("ModusOperandi").gameObject.SetActive(true);
                }


            }

            else if (UI.transform.Find("GoBack").gameObject.activeInHierarchy)
            {

                if (col.name == "CloseButton")
                {
                    UI.transform.Find("GoBack").gameObject.SetActive(false);
                    UI.transform.Find("Watchlist").gameObject.SetActive(true);
                }


            }

            else if (UI.transform.Find("ModusOperandi").gameObject.activeInHierarchy)
            {

                if (col.name == "CloseButton")
                {
                    UI.transform.Find("ModusOperandi").gameObject.SetActive(false);
                    UI.transform.Find("Watchlist").gameObject.SetActive(true);
                }

                if (col.name == "NextButton")
                {
                    UI.transform.Find("Evidence").gameObject.SetActive(true);
                    UI.transform.Find("ModusOperandi").gameObject.SetActive(false);
                }

            }

            else if (UI.transform.Find("Evidence").gameObject.activeInHierarchy)
            {

                if (col.name == "CloseButton")
                {
                    UI.transform.Find("Evidence").gameObject.SetActive(false);
                    UI.transform.Find("Watchlist").gameObject.SetActive(true);
                }

                if (col.name == "PreviousButton")
                {
                    UI.transform.Find("Evidence").gameObject.SetActive(false);
                    UI.transform.Find("ModusOperandi").gameObject.SetActive(true);
                }

                if (col.name == "NextButton")
                {
                    UI.transform.Find("Evidence").gameObject.SetActive(false);
                    UI.transform.Find("TypeOfKilling").gameObject.SetActive(true);
                }

            }

            else if (UI.transform.Find("TypeOfKilling").gameObject.activeInHierarchy)
            {

                if (col.name == "CloseButton")
                {
                    UI.transform.Find("TypeOfKilling").gameObject.SetActive(false);
                    UI.transform.Find("Watchlist").gameObject.SetActive(true);
                }

                if (col.name == "PreviousButton")
                {
                    UI.transform.Find("TypeOfKilling").gameObject.SetActive(false);
                    UI.transform.Find("Evidence").gameObject.SetActive(true);
                }

                if (col.name == "NextButton")
                {
                    UI.transform.Find("TypeOfKilling").gameObject.SetActive(false);
                    UI.transform.Find("OfficerSummary").gameObject.SetActive(true);
                }

            }

            else if (UI.transform.Find("OfficerSummary").gameObject.activeInHierarchy)
            {

                if (col.name == "CloseButton")
                {
                    UI.transform.Find("OfficerSummary").gameObject.SetActive(false);
                    UI.transform.Find("Watchlist").gameObject.SetActive(true);
                }

                if (col.name == "PreviousButton")
                {
                    UI.transform.Find("OfficerSummary").gameObject.SetActive(false);
                    UI.transform.Find("TypeOfKilling").gameObject.SetActive(true);
                }

                if (col.name == "SubmitButton")
                {
                    UI.transform.Find("OfficerSummary").gameObject.SetActive(false);
                    UI.transform.Find("Submitted").gameObject.SetActive(true);
                    SaveData();
                    PrintData();
                }

            }

            else if (UI.transform.Find("Submitted").gameObject.activeInHierarchy)
            {

                if (col.name == "CloseButton")
                {
                    UI.transform.Find("Submitted").gameObject.SetActive(false);
                    UI.transform.Find("Watchlist").gameObject.SetActive(true);
                }


            }

        }
        
    }

    // Save all the entries in all the forms
    private void SaveData()
    {
        ReportData.Add("DrugOd", UI.transform.Find("ModusOperandi/CheckBox1/Check").gameObject.activeSelf);
        ReportData.Add("Firearm", UI.transform.Find("ModusOperandi/CheckBox2/Check").gameObject.activeSelf);
        ReportData.Add("CuttingTool", UI.transform.Find("ModusOperandi/CheckBox3/Check").gameObject.activeSelf);
        ReportData.Add("Poison", UI.transform.Find("ModusOperandi/CheckBox4/Check").gameObject.activeSelf);
        ReportData.Add("Asphyxiation", UI.transform.Find("ModusOperandi/CheckBox5/Check").gameObject.activeSelf);

        ReportData.Add("Drugs", UI.transform.Find("Evidence/CheckBox1/Check").gameObject.activeSelf);
        ReportData.Add("Weapon", UI.transform.Find("Evidence/CheckBox2/Check").gameObject.activeSelf);
        ReportData.Add("Prints", UI.transform.Find("Evidence/CheckBox3/Check").gameObject.activeSelf);
        ReportData.Add("Description", UI.transform.Find("Evidence/CheckBox4/Check").gameObject.activeSelf);

        ReportData.Add("DrugBust", UI.transform.Find("TypeOfKilling/CheckBox1/Check").gameObject.activeSelf);
        ReportData.Add("SelfDefense", UI.transform.Find("TypeOfKilling/CheckBox2/Check").gameObject.activeSelf);
        ReportData.Add("Vigilante", UI.transform.Find("TypeOfKilling/CheckBox3/Check").gameObject.activeSelf);
    }
    private void PrintData()
    {
        foreach (KeyValuePair<string, bool> kv in ReportData)
        {
            Debug.Log(kv.Key+" "+kv.Value.ToString()+"\n");
            
        }
        
    }
}
