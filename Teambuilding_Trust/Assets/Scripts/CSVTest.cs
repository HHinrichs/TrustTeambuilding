using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVInteractions;
public class CSVTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(addRecords());
    }

    IEnumerator addRecords()
    {
        yield return new WaitForSeconds(5f);
        //CSVWriter.addRecord("dsd",Time.time.ToString(), Time.time, 123, 3, 3, false);
        Debug.Log("LinesWritten");
    }

}
