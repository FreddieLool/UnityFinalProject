using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestText : MonoBehaviour
{
    [SerializeField] TextMeshPro Text;
    private void Start()
    {
        StartCoroutine(thisAnimation());
    }


    private IEnumerator thisAnimation()
    {
        for (float f = 1; f > 0; f -= 0.05f)
        {
            this.transform.localScale = new Vector3(f, f, 0);
            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, f);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        Destroy(this.gameObject);
        yield break;
    }
}
