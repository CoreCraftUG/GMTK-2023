using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace JamCraft.GMTK2023.Code
{
    public class CustomizationUI : MonoBehaviour
    {
        public static CustomizationUI Instance { get; private set; }

        [Header("UI Buttons")]
        [SerializeField] private Button _tableCustomizationButton;
        [SerializeField] private Button _cardCustomizationButton;
        [SerializeField] private Button _decorationCustomizationButton;
        [SerializeField] private Button _backButton;

        [Header("UI Customization Panels")] 
        [SerializeField] private GameObject _tableCustomizationPanel;
        [SerializeField] private GameObject _cardCustomizationPanel;
        [SerializeField] private GameObject _decorationCustomizationPanel;

        [Header("Camera")] 
        [SerializeField] private CinemachineVirtualCamera _uiCamera;

        [Header("Table Type Buttons")] 
        [SerializeField] private Button _tableTypeButton1;
        [SerializeField] private Button _tableTypeButton2;
        [SerializeField] private Button _tableTypeButton3;

        [Header("Table Top Customization Buttons")] 
        [SerializeField] private Button _tableTopCustomizationButton1;
        [SerializeField] private Button _tableTopCustomizationButton2;
        [SerializeField] private Button _tableTopCustomizationButton3;
        [SerializeField] private Button _tableTopCustomizationButton4;
        [SerializeField] private Button _tableTopCustomizationButton5;
        [SerializeField] private Button _tableTopCustomizationButton6;

        [Header("Table Frame Customization Buttons")] 
        [SerializeField] private Button _tableFrameCustomizationButton1;
        [SerializeField] private Button _tableFrameCustomizationButton2;
        [SerializeField] private Button _tableFrameCustomizationButton3;
        [SerializeField] private Button _tableFrameCustomizationButton4;
        [SerializeField] private Button _tableFrameCustomizationButton5;
        [SerializeField] private Button _tableFrameCustomizationButton6;

        private GameObject _selectedTableType;
        private GameObject _selectedTableTop;
        private GameObject _selectedTableFrame;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is more than one {this} instance in the scene!");
            }

            Instance = this;

            SetupButtons();

            Hide();
            _cardCustomizationPanel.SetActive(false);
            _decorationCustomizationPanel.SetActive(false);
        }

        private void SetupButtons()
        {
            _tableCustomizationButton.onClick.AddListener(() =>
            {
                _tableCustomizationPanel.SetActive(true);
                _cardCustomizationPanel.SetActive(false);
                _decorationCustomizationPanel.SetActive(false);
            });

            _cardCustomizationButton.onClick.AddListener(() =>
            {
                _tableCustomizationPanel.SetActive(false);
                _cardCustomizationPanel.SetActive(true);
                _decorationCustomizationPanel.SetActive(false);
            });

            _decorationCustomizationButton.onClick.AddListener(() =>
            {
                _tableCustomizationPanel.SetActive(false);
                _cardCustomizationPanel.SetActive(false);
                _decorationCustomizationPanel.SetActive(true);
            });

            _backButton.onClick.AddListener(() =>
            {
                Hide();

                if (MainMenuUI.Instance != null)
                {
                    MainMenuUI.Instance.Show();
                }
            });

            _tableTypeButton1.onClick.AddListener(() =>
            {
                if (_selectedTableType != null)
                {
                    _selectedTableType.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableType(0);

                _selectedTableType = _tableTypeButton1.gameObject;

                _selectedTableType.GetComponent<Image>().color = Color.yellow;
            });
            
            _tableTypeButton2.onClick.AddListener(() =>
            {
                if (_selectedTableType != null)
                {
                    _selectedTableType.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableType(1);

                _selectedTableType = _tableTypeButton2.gameObject;

                _selectedTableType.GetComponent<Image>().color = Color.yellow;
            });
            
            _tableTypeButton3.onClick.AddListener(() =>
            {
                if (_selectedTableType != null)
                {
                    _selectedTableType.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableType(2);

                _selectedTableType = _tableTypeButton3.gameObject;

                _selectedTableType.GetComponent<Image>().color = Color.yellow;
            });

            _tableTopCustomizationButton1.onClick.AddListener(() =>
            {
                if (_selectedTableTop != null)
                {
                    _selectedTableTop.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableTopVisual(0);

                _selectedTableTop = _tableTopCustomizationButton1.gameObject;

                _selectedTableTop.GetComponent<Image>().color = Color.yellow;
            });

            _tableFrameCustomizationButton1.onClick.AddListener(() =>
            {
                if (_selectedTableFrame != null)
                {
                    _selectedTableFrame.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableFrameVisual(0);

                _selectedTableFrame = _tableFrameCustomizationButton1.gameObject;

                _selectedTableFrame.GetComponent<Image>().color = Color.yellow;
            });

            _tableTopCustomizationButton2.onClick.AddListener(() =>
            {
                if (_selectedTableTop != null)
                {
                    _selectedTableTop.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableTopVisual(1);

                _selectedTableTop = _tableTopCustomizationButton2.gameObject;

                _selectedTableTop.GetComponent<Image>().color = Color.yellow;
            });

            _tableFrameCustomizationButton2.onClick.AddListener(() =>
            {
                if (_selectedTableFrame != null)
                {
                    _selectedTableFrame.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableFrameVisual(1);

                _selectedTableFrame = _tableFrameCustomizationButton2.gameObject;

                _selectedTableFrame.GetComponent<Image>().color = Color.yellow;
            });

            _tableTopCustomizationButton3.onClick.AddListener(() =>
            {
                if (_selectedTableTop != null)
                {
                    _selectedTableTop.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableTopVisual(2);

                _selectedTableTop = _tableTopCustomizationButton3.gameObject;

                _selectedTableTop.GetComponent<Image>().color = Color.yellow;
            });

            _tableFrameCustomizationButton3.onClick.AddListener(() =>
            {
                if (_selectedTableFrame != null)
                {
                    _selectedTableFrame.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableFrameVisual(2);

                _selectedTableFrame = _tableFrameCustomizationButton3.gameObject;

                _selectedTableFrame.GetComponent<Image>().color = Color.yellow;
            });

            _tableTopCustomizationButton4.onClick.AddListener(() =>
            {
                if (_selectedTableTop != null)
                {
                    _selectedTableTop.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableTopVisual(3);

                _selectedTableTop = _tableTopCustomizationButton4.gameObject;

                _selectedTableTop.GetComponent<Image>().color = Color.yellow;
            });

            _tableFrameCustomizationButton4.onClick.AddListener(() =>
            {
                if (_selectedTableFrame != null)
                {
                    _selectedTableFrame.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableFrameVisual(3);

                _selectedTableFrame = _tableFrameCustomizationButton4.gameObject;

                _selectedTableFrame.GetComponent<Image>().color = Color.yellow;
            });

            _tableTopCustomizationButton5.onClick.AddListener(() =>
            {
                if (_selectedTableTop != null)
                {
                    _selectedTableTop.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableTopVisual(4);

                _selectedTableTop = _tableTopCustomizationButton5.gameObject;

                _selectedTableTop.GetComponent<Image>().color = Color.yellow;
            });

            _tableFrameCustomizationButton5.onClick.AddListener(() =>
            {
                if (_selectedTableFrame != null)
                {
                    _selectedTableFrame.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableFrameVisual(4);

                _selectedTableFrame = _tableFrameCustomizationButton5.gameObject;

                _selectedTableFrame.GetComponent<Image>().color = Color.yellow;
            });

            _tableTopCustomizationButton6.onClick.AddListener(() =>
            {
                if (_selectedTableTop != null)
                {
                    _selectedTableTop.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableTopVisual(5);

                _selectedTableTop = _tableTopCustomizationButton6.gameObject;

                _selectedTableTop.GetComponent<Image>().color = Color.yellow;
            });

            _tableFrameCustomizationButton6.onClick.AddListener(() =>
            {
                if (_selectedTableFrame != null)
                {
                    _selectedTableFrame.GetComponent<Image>().color = Color.white;
                }

                TableVisualManager.Instance.SetTableFrameVisual(5);

                _selectedTableFrame = _tableFrameCustomizationButton6.gameObject;

                _selectedTableFrame.GetComponent<Image>().color = Color.yellow;
            });
        }

        private void Start()
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);

            CinemachineComponentBase componentBase = _uiCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = 2.5f;
            }
        }
    }
}