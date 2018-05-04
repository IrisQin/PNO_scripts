using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchAvater : MonoBehaviour {
    public bool startPlayback = false;
    public bool startRecord = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (startPlayback)
        {
            GetComponent<RecordPlayback>().enabled = true;
            GetComponent<RecordPlayback>().LocalAvatar.GetComponent<OvrAvatar>().ShowFirstPerson = false;
            GetComponent<RecordPlayback>().LoopbackAvatar.GetComponent<OvrAvatar>().ShowFirstPerson = true;

        }
        if (!startPlayback&&!startRecord)
        {
            GetComponent<RecordPlayback>().LocalAvatar.GetComponent<OvrAvatar>().ShowFirstPerson = true;
            GetComponent<RecordPlayback>().LoopbackAvatar.GetComponent<OvrAvatar>().ShowFirstPerson = false;
            GetComponent<RecordPlayback>().enabled = false;
            
        }
        if (startRecord)
        {
            GetComponent<RecordPlayback>().enabled = true;

        }




    }
}
