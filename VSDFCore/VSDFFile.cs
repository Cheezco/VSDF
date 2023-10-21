namespace VSDFCore;

public struct VSDFFile
{
    public int Order { get; set; }
    public string Name { get; set; }

    public VSDFFile(int order, string name)
    {
        Order = order;
        Name = name;
    }
}