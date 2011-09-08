using System;

namespace ripple
{
    public class BuildSolution : IRippleStep
    {
        private readonly Solution _solution;

        public BuildSolution(Solution solution)
        {
            _solution = solution;
        }

        public Solution Solution
        {
            get { return _solution; }
        }

        public bool Equals(BuildSolution other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._solution, _solution);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (BuildSolution)) return false;
            return Equals((BuildSolution) obj);
        }

        public override int GetHashCode()
        {
            return (_solution != null ? _solution.GetHashCode() : 0);
        }

        public RippleStepResult Execute(IRippleRunner runner)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("Build {0}", _solution);
        }
    }
}