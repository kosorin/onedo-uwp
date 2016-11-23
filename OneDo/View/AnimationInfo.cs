using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace OneDo.View
{
    public class AnimationInfo
    {
        public AnimationInfo(string propertyName, CompositionAnimation animation)
        {
            PropertyName = propertyName;
            Animation = animation;
        }

        public string PropertyName { get; set; }

        public CompositionAnimation Animation { get; set; }

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
