///Credit perchik
///Sourced from - http://forum.unity3d.com/threads/receive-onclick-event-and-pass-it-on-to-lower-ui-elements.293642/

using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UI.Extensions
{
    public enum AutoCompleteSearchType
    {
        ArraySort,
        Linq
    }

    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Extensions/AutoComplete ComboBox")]
    public class AutoCompleteComboBox : MonoBehaviour
    {
        public Color disabledTextColor;
        public DropDownListItem SelectedItem { get; private set; } 

        public List<string> AvailableOptions;
        private bool _hasDrawnOnce = false;

        private InputField _mainInput;
        private RectTransform _inputRT;
        private RectTransform _rectTransform;

        private RectTransform _overlayRT;
        private RectTransform _scrollPanelRT;
        private RectTransform _scrollBarRT;
        private RectTransform _slidingAreaRT;
        private RectTransform _itemsPanelRT;
        private Canvas _canvas;
        private RectTransform _canvasRT;

        private ScrollRect _scrollRect;

        private List<string> _panelItems;
        private List<string> _prunedPanelItems;

        private Dictionary<string, GameObject> panelObjects;
        
        private GameObject itemTemplate;

        public string Text { get; private set; }

        [SerializeField]
        private float _scrollBarWidth = 20.0f;
        public float ScrollBarWidth
        {
            get { return _scrollBarWidth; }
            set
            {
                _scrollBarWidth = value;
                RedrawPanel();
            }
        }

        [SerializeField]
        private int _itemsToDisplay;
        public int ItemsToDisplay
        {
            get { return _itemsToDisplay; }
            set
            {
                _itemsToDisplay = value;
                RedrawPanel();
            }
        }

		public bool SelectFirstItemOnStart = false;

		[SerializeField]
        [Tooltip("Change input text color based on matching items")]
        private bool _ChangeInputTextColorBasedOnMatchingItems = false;
		public bool InputColorMatching{
			get { return _ChangeInputTextColorBasedOnMatchingItems; }
			set 
			{
				_ChangeInputTextColorBasedOnMatchingItems = value;
				if (_ChangeInputTextColorBasedOnMatchingItems) {
					SetInputTextColor ();
				}
			}
		}

		public Color ValidSelectionTextColor = Color.green;
		public Color MatchingItemsRemainingTextColor = Color.black;
		public Color NoItemsRemainingTextColor = Color.red;

        public AutoCompleteSearchType autocompleteSearchType = AutoCompleteSearchType.Linq;

        private bool _selectionIsValid = false;

		[System.Serializable]
		public class SelectionChangedEvent :  UnityEngine.Events.UnityEvent<string, bool> {
		}

        [System.Serializable]
		public class SelectionTextChangedEvent :  UnityEngine.Events.UnityEvent<string> {
		}

		[System.Serializable]
		public class SelectionValidityChangedEvent :  UnityEngine.Events.UnityEvent<bool> {
		}

		public SelectionTextChangedEvent OnSelectionTextChanged;
		public SelectionValidityChangedEvent OnSelectionValidityChanged;
		public SelectionChangedEvent OnSelectionChanged;

        public void Awake()
        {
            Initialize();
        }
		public void Start()
		{
			if (SelectFirstItemOnStart && AvailableOptions.Count > 0) {
				OnItemClicked (AvailableOptions [0]);
			}
		}

        private bool Initialize()
        {
            bool success = true;
            try
            {
                _rectTransform = GetComponent<RectTransform>();
                _inputRT = _rectTransform.Find("InputField").GetComponent<RectTransform>();
                _mainInput = _inputRT.GetComponent<InputField>();

                _overlayRT = _rectTransform.Find("Overlay").GetComponent<RectTransform>();
                _overlayRT.gameObject.SetActive(false);


                _scrollPanelRT = _overlayRT.Find("ScrollPanel").GetComponent<RectTransform>();
                _scrollBarRT = _scrollPanelRT.Find("Scrollbar").GetComponent<RectTransform>();
                _slidingAreaRT = _scrollBarRT.Find("SlidingArea").GetComponent<RectTransform>();
                _itemsPanelRT = _scrollPanelRT.Find("Items").GetComponent<RectTransform>();

                _canvas = GetComponentInParent<Canvas>();
                _canvasRT = _canvas.GetComponent<RectTransform>();

                _scrollRect = _scrollPanelRT.GetComponent<ScrollRect>();
                _scrollRect.scrollSensitivity = _rectTransform.sizeDelta.y / 2;
                _scrollRect.movementType = ScrollRect.MovementType.Clamped;
                _scrollRect.content = _itemsPanelRT;

                itemTemplate = _rectTransform.Find("ItemTemplate").gameObject;
                itemTemplate.SetActive(false);
            }
            catch (System.NullReferenceException ex)
            {
                Debug.LogException(ex);
                Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Refernece Exception");
                success = false;
            }
            panelObjects = new Dictionary<string, GameObject>();

            _prunedPanelItems = new List<string>();
            _panelItems = new List<string>();

            RebuildPanel();
            return success;
        }
        

        /// <summary>
        /// Rebuilds the contents of the panel in response to items being added.
        /// </summary>
        private void RebuildPanel()
        {
            _panelItems.Clear();
            _prunedPanelItems.Clear();
            panelObjects.Clear();

            foreach (string option in AvailableOptions)
            {
                _panelItems.Add(option.ToLower());
            }

            List<GameObject> itemObjs = new List<GameObject>(panelObjects.Values);

            int indx = 0;
            while (itemObjs.Count < AvailableOptions.Count)
            {
                GameObject newItem = Instantiate(itemTemplate) as GameObject;
                newItem.name = "Item " + indx;
                newItem.transform.SetParent(_itemsPanelRT, false);
                itemObjs.Add(newItem);
                indx++;
            }

            for (int i = 0; i < itemObjs.Count; i++)
            {
                itemObjs[i].SetActive(i <= AvailableOptions.Count);
                if (i < AvailableOptions.Count)
                {
                    itemObjs[i].name = "Item " + i + " " + _panelItems[i];
                    itemObjs[i].transform.Find("Text").GetComponent<Text>().text = _panelItems[i]; //set the text value

                    Button itemBtn = itemObjs[i].GetComponent<Button>();
                    itemBtn.onClick.RemoveAllListeners();
                    string textOfItem = _panelItems[i]; //has to be copied for anonymous function or it gets garbage collected away
                    itemBtn.onClick.AddListener(() =>
                    {
                        OnItemClicked(textOfItem);
                    });
                    panelObjects[_panelItems[i]] = itemObjs[i];
                }
            }
			SetInputTextColor ();
        }

        /// <summary>
        /// what happens when an item in the list is selected
        /// </summary>
        /// <param name="item"></param>
        private void OnItemClicked(string item)
        {
            Text = item;
            _mainInput.text = Text;
        }
        
        private void RedrawPanel()
        {
            float scrollbarWidth = _scrollBarWidth;
            _scrollBarRT.gameObject.SetActive(_panelItems.Count > ItemsToDisplay);
            if (!_hasDrawnOnce || _rectTransform.sizeDelta != _inputRT.sizeDelta)
            {
                _hasDrawnOnce = true;
                _inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.sizeDelta.x);
                _inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _rectTransform.sizeDelta.y);

                _scrollPanelRT.SetParent(transform, true);
                _scrollPanelRT.anchoredPosition = new Vector2(0, -_rectTransform.sizeDelta.y);

                _overlayRT.SetParent(_canvas.transform, false);
                _overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _canvasRT.sizeDelta.x);
                _overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _canvasRT.sizeDelta.y);

                _overlayRT.SetParent(transform, true);
                _scrollPanelRT.SetParent(_overlayRT, true);
            }

            if (_panelItems.Count < 1) return;

            float dropdownHeight = _rectTransform.sizeDelta.y * Mathf.Min(_itemsToDisplay, _panelItems.Count);

            _scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownHeight);
            _scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.sizeDelta.x);

            _itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _scrollPanelRT.sizeDelta.x - scrollbarWidth - 5);
            _itemsPanelRT.anchoredPosition = new Vector2(5, 0);

            _scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollbarWidth);
            _scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownHeight);

            _slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            _slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownHeight - _scrollBarRT.sizeDelta.x);
        }

        public void OnValueChanged(string currText)
        {
            Text = currText;
            PruneItems(currText);
            RedrawPanel();
			bool validity_changed = (_panelItems.Contains (Text) != _selectionIsValid);
			_selectionIsValid = _panelItems.Contains (Text);
			OnSelectionChanged.Invoke (Text, _selectionIsValid);
			OnSelectionTextChanged.Invoke (Text);
			if(validity_changed){
				OnSelectionValidityChanged.Invoke (_selectionIsValid);
			}

			SetInputTextColor ();
        }

		private void SetInputTextColor(){
			if (InputColorMatching) {
				if (_selectionIsValid) {
					_mainInput.textComponent.color = ValidSelectionTextColor;
				} else if (_panelItems.Count > 0) {
					_mainInput.textComponent.color = MatchingItemsRemainingTextColor;
				} else {
					_mainInput.textComponent.color = NoItemsRemainingTextColor;
				}
			}
		}
        
        private void PruneItems(string currText)
        {
            if (autocompleteSearchType == AutoCompleteSearchType.Linq)
            {
                PruneItemsLinq(currText);
            }
            else
            {
                PruneItemsArray(currText);
            }
        }

        private void PruneItemsLinq(string currText)
        {
            currText = currText.ToLower();
            var toPrune = _panelItems.Where(x => !x.Contains(currText)).ToArray();
            foreach (string key in toPrune)
            {
                panelObjects[key].SetActive(false);
                _panelItems.Remove(key);
                _prunedPanelItems.Add(key);
            }

            var toAddBack = _prunedPanelItems.Where(x => x.Contains(currText)).ToArray();
            foreach (string key in toAddBack)
            {
                panelObjects[key].SetActive(true);
                _panelItems.Add(key);
                _prunedPanelItems.Remove(key);
            }
        }

        //Updated to not use Linq
        private void PruneItemsArray(string currText)
        {
            string _currText = currText.ToLower();

            for (int i = _panelItems.Count - 1; i >= 0; i--)
            {
                string _item = _panelItems[i];
                if (!_item.Contains(_currText))
                {
                    panelObjects[_panelItems[i]].SetActive(false);
                    _panelItems.RemoveAt(i);
                    _prunedPanelItems.Add(_item);
                }
            }
            for (int i = _prunedPanelItems.Count - 1; i >= 0; i--)
            {
                string _item = _prunedPanelItems[i];
                if (_item.Contains(_currText))
                {
                    panelObjects[_prunedPanelItems[i]].SetActive(true);
                    _prunedPanelItems.RemoveAt(i);
                    _panelItems.Add(_item);
                }
            }
        }
    }
}
