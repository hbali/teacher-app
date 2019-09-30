using UnityEngine;
using UnityEngine.UI;

namespace UI.LoginView
{
    public class FacebookInfoHandler : MonoBehaviour
    {

        [SerializeField] private Image profImg;
        [SerializeField] private Text fbName;

        public Text FbName
        {
            get
            {
                return fbName;
            }
        }

        public void SetPicture(Texture2D img)
        {
            profImg.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
        }

        public void SetName(string name)
        {
            fbName.text = name;
        }

        public Texture2D GetPicture()
        {
            return profImg.sprite.texture;
        }
    }
}