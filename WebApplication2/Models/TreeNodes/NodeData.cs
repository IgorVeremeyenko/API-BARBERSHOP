namespace WebApplication2.Models.TreeNodes
{
    public class NodeData
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public int MasterId { get; set; }
        public decimal Price { get; set; }
        public string MasterName { get; set; }
        public NodeData Data { get; internal set; }
    }
}
