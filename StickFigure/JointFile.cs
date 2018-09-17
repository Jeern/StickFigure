using System.Collections.Generic;
using Newtonsoft.Json;

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
