using UnityEngine.UI;

namespace ViewR.Core.UI.Visuals.AnimatedImageFill
{
    [System.Serializable]
    public class ImageFillConfig
    {
        public const Image.Type TYPE = Image.Type.Filled;
        public Image.FillMethod fillMethod = Image.FillMethod.Horizontal;
        public Image.Origin90 origin90;
        public Image.Origin180 origin180;
        public Image.Origin360 origin360;
        public Image.OriginHorizontal originHorizontal;
        public Image.OriginVertical originVertical;


        public void SetImageToConfig(Image targetImage)
        {
            targetImage.type = TYPE;
            targetImage.fillMethod = fillMethod;

            switch ((Image.FillMethod)fillMethod)
            {
                case Image.FillMethod.Horizontal:
                    targetImage.fillOrigin = (int)(Image.OriginHorizontal)fillMethod;
                    break;
                case Image.FillMethod.Vertical:
                    targetImage.fillOrigin = (int)(Image.OriginVertical)fillMethod;
                    break;
                case Image.FillMethod.Radial90:
                    targetImage.fillOrigin = (int)(Image.Origin90)fillMethod;
                    break;
                case Image.FillMethod.Radial180:
                    targetImage.fillOrigin = (int)(Image.Origin180)fillMethod;
                    break;
                case Image.FillMethod.Radial360:
                    targetImage.fillOrigin = (int)(Image.Origin360)fillMethod;
                    break;
            }
        }
    }
}