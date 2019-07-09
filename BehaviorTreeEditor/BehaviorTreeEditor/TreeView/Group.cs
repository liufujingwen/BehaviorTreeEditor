namespace BehaviorTreeEditor
{
    public class Group
    {
        public Group()
        {
        }

        public Group(string groupName)
        {
            GroupName = groupName;
        }

        public string GroupName = string.Empty;

        public override string ToString()
        {
            return GroupName;
        }
    }
}
