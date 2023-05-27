using WebApplication2.Models;
using WebApplication2.Models.TreeNodes;

namespace WebApplication2.Services
{
    public class GenerateTreeNodeListServices
    {
        private List<Master>? _masters;
        private List<TreeNode>? _nodes;
        public List<TreeNode> GetTreeNode(MyDatabaseContext context, List<Service> services)
        {
            _masters = context.Masters.ToList();
            _nodes = new List<TreeNode>();
            TreeNode nodes = new TreeNode();
            nodes.Data = new NodeData();
            nodes.Children = new List<NodeData>();

            foreach (var item in services)
            {
                var flag = _nodes.Where(p => p.Data.Name == item.Category).Count() > 0;
                if (flag)
                {
                    NodeData child = new NodeData();
                    child.Data = new NodeData();
                    child.Data.Name = item.Name;
                    child.Data.Id = item.Id;
                    child.Data.Name = item.Name;
                    child.Data.MasterName = _masters.First(p => p.Id == item.MasterId).Name;
                    child.Data.MasterId = item.MasterId;
                    child.Data.Price = item.Price;
                    _nodes.First(p => p.Data.Name == item.Category).Children.Add(child);
                }
                else
                {
                    nodes = new TreeNode();
                    nodes.Data = new NodeData();
                    nodes.Children = new List<NodeData>();
                    NodeData child = new NodeData();
                    child.Data = new NodeData();
                    child.Data.Name = item.Name;
                    child.Data.Id = item.Id;
                    child.Data.Name = item.Name;
                    child.Data.MasterName = _masters.First(p => p.Id == item.MasterId).Name;
                    child.Data.MasterId = item.MasterId;
                    child.Data.Price = item.Price;
                    nodes.Children.Add(child);
                    nodes.Data.Name = item.Category;
                    _nodes.Add(nodes);
                }


            }

            return _nodes;
        }
    }
}
