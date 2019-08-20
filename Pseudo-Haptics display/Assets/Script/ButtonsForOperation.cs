using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ButtonsForOperation : MonoBehaviour
{
    private ExperimentManager experimentManager;
    public ToggleGroup displayConditionToggleGroup;
    public ToggleGroup CDratioToggleGroup;
    public Toggle groundTruth;
 
    // Start is called before the first frame update
    void Start()
    {
        experimentManager = GameObject.Find("Manager").GetComponent<ExperimentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTaskSetButton()
    {
        if (groundTruth.GetComponent<Toggle>().isOn == true)
        {
            experimentManager.expCondition = ExperimentManager.ExpCondition.Visual;
            experimentManager.CDratio = 1.0f;
        }
        else
        {
            //cited:https://qiita.com/JunShimura/items/453ab90ecafd2dd7abd8
            string selectedDisplayCondition = displayConditionToggleGroup.ActiveToggles()
                .First().GetComponentsInChildren<Text>()
                .First(t => t.name == "Label").text;
            
            if (selectedDisplayCondition == "Visual_") experimentManager.expCondition = ExperimentManager.ExpCondition.Visual_;
            else if (selectedDisplayCondition == "Physical_") experimentManager.expCondition = ExperimentManager.ExpCondition.Physical_;
            else experimentManager.expCondition = ExperimentManager.ExpCondition.Visual_Physical_;

            string selectedCDRatio = CDratioToggleGroup.ActiveToggles()
                .First().GetComponentsInChildren<Text>()
                .First(t => t.name == "Label").text;

            if (selectedCDRatio == "0.4") experimentManager.CDratio = 0.4f;
            else if (selectedCDRatio == "0.6") experimentManager.CDratio = 0.6f;
            else if (selectedCDRatio == "0.8") experimentManager.CDratio = 0.8f;
            else if (selectedCDRatio == "1.2") experimentManager.CDratio = 1.2f;
            else if (selectedCDRatio == "1.4") experimentManager.CDratio = 1.4f;
            else experimentManager.CDratio = 1.6f;
        }
    }
}
