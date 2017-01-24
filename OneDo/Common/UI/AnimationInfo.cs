using Windows.UI.Composition;

namespace OneDo.Common.UI
{
    public class AnimationInfo
    {
        public AnimationInfo(string propertyName, CompositionAnimation animation)
        {
            PropertyName = propertyName;
            Animation = animation;
        }

        public string PropertyName { get; }

        public CompositionAnimation Animation { get; }

        public void Start(Visual visual)
        {
            visual.StartAnimation(PropertyName, Animation);
        }

        public void Stop(Visual visual)
        {
            visual.StopAnimation(PropertyName);
        }
    }
}
