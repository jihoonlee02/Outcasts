using System.Collections.Generic;

public class TreeNode<T>
{
    private TreeNode<T> parentNode;
    private List<TreeNode<T>> childNodes;
    private T data;
    public T Data => data;
    public List<TreeNode<T>> ChildNodes => childNodes;
    public TreeNode<T> Parent 
    {
        get { return parentNode; }
        set { parentNode = value; }
    }
    public TreeNode(T data, TreeNode<T> parentNode = null, List<TreeNode<T>> childNodes = null)
    {
        this.data = data;
        this.parentNode = parentNode;
        this.childNodes = childNodes;
         
        if (this.childNodes == null) this.childNodes = new List<TreeNode<T>>();
    }
    public void AddChild(T child)
    {
        childNodes.Add(new TreeNode<T>(child, this));
    }
    public void AddChild(TreeNode<T> child)
    {
        child.Parent = this;
        childNodes.Add(child);
    }

    public void AddChildren(T[] children)
    {
        foreach (T child in children)
        {
            childNodes.Add(new TreeNode<T>(child, this));
        }
    }

    public void AddChildren(TreeNode<T>[] children)
    {
        foreach (TreeNode<T> child in children)
        {
            child.Parent = this;
            childNodes.Add(child);
        }
    }
}
