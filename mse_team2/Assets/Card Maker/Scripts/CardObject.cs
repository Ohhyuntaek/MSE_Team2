using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    [Header("-----Get Values From Unit-----")]
    //[SerializeField] private GameObject unitPrefab;
    public GameObject[] prefabs;
    public int selectedPrefabIndex = 0;

    [SerializeField] private Transform generationPoint;
    private GameObject unitObject;
    private Animator unitAnimator;
    private UnitValueManager unitValueManager;
    [SerializeField] private string name_unit = "Name";
    [SerializeField] private string habitat_unit = "Habitat";
    [SerializeField] private string role_unit = "Role";
    [SerializeField] private int HP_unit = 100;
    [SerializeField] private int AP_unit = 100;

    [Header("-----Edge materials-----")]
    [SerializeField] private Image edgeImage;
    [SerializeField] private Material grass_EM;
    [SerializeField] private Material mountain_EM;
    [SerializeField] private Material aquatic_EM;

    [Header("-----Bbackground Images-----")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite grass_BGI;
    [SerializeField] private Sprite mountain_BGI;
    [SerializeField] private Sprite aquatic_BGI;

    [Header("-----HP & AP Values-----")]
    [SerializeField] private TextMeshProUGUI Name_Role;
    [SerializeField] private TextMeshProUGUI HP_Text;
    [SerializeField] private GameObject HP_Bar;
    [SerializeField] private TextMeshProUGUI AP_Text;
    [SerializeField] private GameObject AP_Bar;


    private void Awake()
    {
        unitObject = Instantiate(prefabs[selectedPrefabIndex], generationPoint);
        unitValueManager = unitObject.GetComponent<UnitValueManager>();
        unitAnimator = unitObject.GetComponent<Animator>();
        unitAnimator.Play("Idle_A");

        UpdateValues();
    }

    private void OnEnable()
    {
        unitAnimator.Play("Idle_A");
    }

    void Start()
    {
        UpdateBackground();
    }

    void Update()
    {
        
    }

    private void UpdateValues()
    {
        name_unit = unitValueManager.unitName;
        habitat_unit = unitValueManager.habitat;
        role_unit = unitValueManager.role;
        HP_unit = unitValueManager.HP;
        AP_unit = unitValueManager.AP;

        gameObject.name = "CardObject["+ selectedPrefabIndex + "]_" + name_unit;
        Name_Role.text = name_unit + " - " + role_unit;
        HP_Text.text = HP_unit.ToString();
        HP_Bar.GetComponent<ValueBar>().basicValue = HP_unit;
        AP_Text.text = AP_unit.ToString();
        AP_Bar.GetComponent<ValueBar>().basicValue = AP_unit;
    }
    private void UpdateBackground()
    {
        switch (role_unit)
        {
            case "Attacker":
                edgeImage.material = grass_EM;
                backgroundImage.sprite = grass_BGI;
                break;
            case "Tanker":
                edgeImage.material = mountain_EM;
                backgroundImage.sprite = mountain_BGI;
                break;
            case "Buffer":
                edgeImage.material = aquatic_EM;
                backgroundImage.sprite = aquatic_BGI;
                break;

            default:
                break;
        }

        void OnGUI()
        {
            // Create drop-down menus for selecting different Prefabs
            selectedPrefabIndex = UnityEditor.EditorGUILayout.Popup("Select Prefab", selectedPrefabIndex, GetPrefabNames());

            // 根据选择的索引实例化对应的 Prefab
            //if (GUILayout.Button("Instantiate Prefab"))
            //{
            //    Instantiate(prefabs[selectedPrefabIndex], transform.position, Quaternion.identity);
            //}
        }

        // Get the names of all Prefabs
        string[] GetPrefabNames()
        {
            string[] names = new string[prefabs.Length];
            for (int i = 0; i < prefabs.Length; i++)
            {
                names[i] = prefabs[i].name;
            }
            return names;
        }

    } 
}
