namespace BehaviorTreeEditor
{
    public enum OperationType
    {
        LoadWorkSpace,//加载
        NewWorkSpace,//新建工作区
        OpenWorkSpace,//打开工作区
        EditWorkSpace,//编辑工作区
        Save,//保存
        Refresh,//刷新
        AddBehaviorTree,//添加行为树
        CopyBehaviorTree,//复制行为树
        CopyGroup,//复制分组
        DeleteBehaviorTreeOrGroup,//删除行为树或者分组
        DeleteBehaviorTree,//删除行为树
        EditBehaviorTree,//编辑行为树
        SwapBehaviorTree,//交换行为树
        PasteBehaviorTree,//粘贴行为树
        PasteBehaviorTreeGroup,//粘贴行为树分组
        UpdateBehaviorTree,//刷新行为树
        Reset,//重置
        AddGroup,//添加分组
        EditGroup,//编辑分组
        DeleteGroup,//删除分组
        UpdateGroup,//更新分组
    }
}
