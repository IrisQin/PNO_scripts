using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class UsingTapeEmitter : VRTK_InteractableObject
{

    [Header("Custom Variables", order = 1)]
    public float ScaleFactor = 50f; // The factor need to be adjusted according to the world scale
    public enum AxisTypes { X, Z };
    public AxisTypes axistype;

    GameObject Tape;
    GameObject TapeStart;
    Vector3 StartTapeScale;
    Vector3 StartTapePos;
    RaycastHit StartHit;
    Vector3 StartEmitterPos;
    private bool StartDraw;
    private bool FinishTapeOnce;
    private bool HasClone;
    private bool TriggerHolding;
    private int TapeIndex;

    Vector3 PillarFrontNormal = new Vector3(0.0f, 0.0f, -1.0f);
    Vector3 PillarBackNormal = new Vector3(0.0f, 0.0f, 1.0f);
    Vector3 PillarRightNormal = new Vector3(1.0f, 0.0f, 0.0f);
    Vector3 PillarLeftNormal = new Vector3(-1.0f, 0.0f, 0.0f);

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        
        TriggerHolding = true;
        base.StartUsing(usingObject);
        
    }

    public override void StopUsing(VRTK_InteractUse previousUsingObject)
    {
        TriggerHolding = false;
        base.StopUsing(previousUsingObject);

        // Release the button in the middle way
        if (StartDraw && !FinishTapeOnce)
        {
            StartDraw = false;
            Tape.transform.localScale = StartTapeScale;
            TapeStart.SetActive(false);

        }

        // After draw and release button, then clone a new Tape
        if (!HasClone && !StartDraw && FinishTapeOnce)
        {
            HasClone = true;
            Vector3 currScale = Tape.transform.localScale;
            Tape.transform.localScale = StartTapeScale;
            TapeStart.SetActive(false);
            GameObject clone = Instantiate(Tape, StartTapePos, Tape.transform.rotation);
            TapeIndex++;
            clone.name = "Tape" + TapeIndex;
            TapeStart.SetActive(true);
            Tape.transform.localScale = currScale;
            InitializeANewTape();
        }
    }


    // Use this for initialization
    void Start () {
        TapeIndex = 1;
        InitializeANewTape();
    }
	
	// Update is called once per frame
	void Update () {
        
        // Set the start point
        if (!StartDraw && !FinishTapeOnce)
        {
            RaycastPillar();
        }

        if (TriggerHolding) {

            // When you hold the button and begin to tape
            if (StartDraw)
            {

                StretchTape();
            }

            // Detect finish
            if ((axistype == AxisTypes.X && StartHit.normal == PillarFrontNormal || StartHit.normal == PillarBackNormal) || (axistype == AxisTypes.Z && StartHit.normal == PillarRightNormal || StartHit.normal == PillarLeftNormal))
            {
                if (!FinishTapeOnce && StartDraw)
                {
                    if (CollidePillar())
                    {
                        if (Mathf.Abs(Tape.transform.localScale.x) > (StartTapeScale.x * 10f))
                        {
                            StartDraw = false;
                            FinishTapeOnce = true;
                        }
                    }
                }

            }
        }
    }

    public bool getTapeFinish()
    {
        return FinishTapeOnce;
    }

    private void InitializeANewTape() {
        Tape = GameObject.Find("Tape" + TapeIndex);
        TapeStart = Tape.transform.Find("TapeStart").gameObject;
        StartDraw = false;
        StartTapeScale = Tape.transform.localScale;
        FinishTapeOnce = false;
        HasClone = false;
        TriggerHolding = false;
    }

    private bool CollidePillar()
    {
        DetectCollide detect = Tape.GetComponentInChildren<DetectCollide>();
        return detect.CollidePillar;
    }

    private void StretchTape()
    {
        if (StartHit.normal == PillarFrontNormal || StartHit.normal == PillarBackNormal)
        {
            float length = transform.position.x - StartEmitterPos.x;
            float newScaleX = length * ScaleFactor;    
            Tape.transform.localScale = new Vector3(newScaleX, StartTapeScale.y, StartTapeScale.z);
        }

        else if (StartHit.normal == PillarLeftNormal)
        {
            float length = transform.position.z - StartEmitterPos.z;
            float newScaleX = -length * ScaleFactor;
            Tape.transform.localScale = new Vector3(newScaleX, StartTapeScale.y, StartTapeScale.z);
        }
        else if (StartHit.normal == PillarRightNormal)
        {
            float length = transform.position.z - StartEmitterPos.z;
            float newScaleX = length * ScaleFactor;
            Tape.transform.localScale = new Vector3(newScaleX, StartTapeScale.y, StartTapeScale.z);
        }
    }

    private void RaycastPillar()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {

            if (hit.collider.gameObject.CompareTag("Pillar"))
            {
                if (!TapeStart.activeSelf )
                    TapeStart.SetActive(true);
                Tape.transform.position = hit.point;
                Tape.transform.rotation = Quaternion.LookRotation(-hit.normal, hit.normal);

                if (TriggerHolding) 
                {
                    StartDraw = true;
                    StartHit = hit;
                    StartEmitterPos = transform.position;
                    StartTapePos = Tape.transform.position;
                }
                
            }
        }
        else
        {
            if (TapeStart.activeSelf)
                TapeStart.SetActive(false);
        }
    }

}
