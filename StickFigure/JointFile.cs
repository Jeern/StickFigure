using System.Collections.Generic;
using Newtonsoft.Json;
using StickFigure.Drawing;

namespace StickFigure
{
    public class JointFile
    {
        [JsonIgnore]
        public bool IsLast { get; set; }

        public List<ConcreteJoint> ConcreteJoints { get; set; }
        public List<Line> Lines { get; set; }
    }
}
